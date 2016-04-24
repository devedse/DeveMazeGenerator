using DeveMazeGenerator.InnerMaps;
using Hjg.Pngcs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator
{
    public partial class Maze
    {
        private void SaveMazeAsImageDeluxePng(String fileName, List<MazePointPos> pathPosjes, Action<int, int> lineSavingProgress)
        {
            pathPosjes.Sort((first, second) =>
            {
                if (first.Y == second.Y)
                {
                    return first.X - second.X;
                }
                return first.Y - second.Y;
            });





            ImageInfo imi = new ImageInfo(this.Width - 1, this.Height - 1, 8, false); // 8 bits per channel, no alpha 
            // open image for writing 
            PngWriter png = FileHelper.CreatePngWriter(fileName, imi, true);
            // add some optional metadata (chunks)
            png.GetMetadata().SetDpi(100.0);
            png.GetMetadata().SetTimeNow(0); // 0 seconds fron now = now
            png.CompLevel = 4;
            //png.GetMetadata().SetText(PngChunkTextVar.KEY_Title, "Just a text image");
            //PngChunk chunk = png.GetMetadata().SetText("my key", "my text .. bla bla");
            //chunk.Priority = true; // this chunk will be written as soon as possible


            int curpos = 0;

            for (int y = 0; y < this.Height - 1; y++)
            {
                ImageLine iline = new ImageLine(imi);

                for (int x = 0; x < this.Width - 1; x++)
                {

                    int r = 0;
                    int g = 0;
                    int b = 0;

                    MazePointPos curPathPos;
                    if (curpos < pathPosjes.Count)
                    {
                        curPathPos = pathPosjes[curpos];
                        if (curPathPos.X == x && curPathPos.Y == y)
                        {
                            r = curPathPos.RelativePos;
                            g = 255 - curPathPos.RelativePos;
                            b = 0;
                            curpos++;
                        }
                        else if (this.innerMap[x, y])
                        {
                            r = 255;
                            g = 255;
                            b = 255;
                        }
                    }
                    else if (this.innerMap[x, y])
                    {
                        r = 255;
                        g = 255;
                        b = 255;
                    }

                    ImageLineHelper.SetPixel(iline, x, r, g, b);
                }
                png.WriteRow(iline, y);
                lineSavingProgress(y, this.Height - 2);
            }
            png.End();
        }


        private void SaveMazeAsImageDeluxePngWithDynamicallyGeneratedPath(String fileName, IEnumerable<MazePointPos> pathPosjes, Action<int, int> lineSavingProgress)
        {
            ImageInfo imi = new ImageInfo(this.Width - 1, this.Height - 1, 8, false); // 8 bits per channel, no alpha 
            // open image for writing 
            PngWriter png = FileHelper.CreatePngWriter(fileName, imi, true);
            // add some optional metadata (chunks)
            png.GetMetadata().SetDpi(100.0);
            png.GetMetadata().SetTimeNow(0); // 0 seconds fron now = now
            png.CompLevel = 4;
            //png.GetMetadata().SetText(PngChunkTextVar.KEY_Title, "Just a text image");
            //PngChunk chunk = png.GetMetadata().SetText("my key", "my text .. bla bla");
            //chunk.Priority = true; // this chunk will be written as soon as possible




            for (int yChunkStart = 0; yChunkStart < this.Height - 1; yChunkStart += Maze.LineChunks)
            {
                var yChunkEnd = Math.Min(yChunkStart + Maze.LineChunks, this.Height - 1);

                var pathPointsHere = pathPosjes.Where(t => t.Y >= yChunkStart && t.Y < yChunkEnd).ToList();
                pathPointsHere.Sort((first, second) =>
                {
                    if (first.Y == second.Y)
                    {
                        return first.X - second.X;
                    }
                    return first.Y - second.Y;
                });
                int curpos = 0;

                for (int y = yChunkStart; y < yChunkEnd; y++)
                {
                    ImageLine iline = new ImageLine(imi);

                    for (int x = 0; x < this.Width - 1; x++)
                    {

                        int r = 0;
                        int g = 0;
                        int b = 0;

                        MazePointPos curPathPos;
                        if (curpos < pathPointsHere.Count)
                        {
                            curPathPos = pathPointsHere[curpos];
                            if (curPathPos.X == x && curPathPos.Y == y)
                            {
                                r = curPathPos.RelativePos;
                                g = 255 - curPathPos.RelativePos;
                                b = 0;
                                curpos++;
                            }
                            else if (this.innerMap[x, y])
                            {
                                r = 255;
                                g = 255;
                                b = 255;
                            }
                        }
                        else if (this.innerMap[x, y])
                        {
                            r = 255;
                            g = 255;
                            b = 255;
                        }

                        ImageLineHelper.SetPixel(iline, x, r, g, b);
                    }
                    png.WriteRow(iline, y);
                    lineSavingProgress(y, this.Height - 2);
                }
            }
            png.End();
        }


        /// <summary>
        /// This method performs a preanalysis on the path to make sure there's no super high memory usage for a certain area
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="pathPosjes"></param>
        /// <param name="lineSavingProgress"></param>
        /// <param name="debugMessageCallback"></param>
        private void SaveMazeAsImageDeluxePngWithDynamicallyGeneratedPathWithAnalysis(string fileName, IEnumerable<MazePointPos> pathPosjes, Action<int, int> lineSavingProgress, Action<string> debugMessageCallback = null)
        {
            if (debugMessageCallback == null)
            {
                debugMessageCallback = (x) => { };
            }

            debugMessageCallback("Performing path analysis...");

            var pathPointsPerRow = new int[this.Height];
            long totalPathLength = 0;

            for (int i = 0; i < this.Height; i++)
            {
                pathPointsPerRow[i] = 0;
            }

            foreach (var pathPos in pathPosjes)
            {
                pathPointsPerRow[pathPos.Y]++;
                totalPathLength++;
            }

            debugMessageCallback(string.Format("Path analysis completed. Total path length: {0}, this would take up {1}mb.", totalPathLength, Math.Round(totalPathLength * 9.0 / 1024.0 / 1024.0, 2)));

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var compinfo = new Microsoft.VisualBasic.Devices.ComputerInfo();
            var memoryFree = compinfo.AvailablePhysicalMemory;

            debugMessageCallback(string.Format("Memory free: {0}mb", memoryFree / 1024 / 1024));
            memoryFree = (ulong)(memoryFree * 0.4);
            debugMessageCallback(string.Format("Setting max usage to 40% of this: {0}mb", memoryFree / 1024 / 1024));

            debugMessageCallback("Determining desired rows to generate each path cycle...");
            int rowsPerPathDeterminingCycle = FindTheMinimalRowsToWriteForPng(debugMessageCallback, pathPointsPerRow, memoryFree);



            int tiffTileSize = HybridInnerMap.GridSize;

            if (rowsPerPathDeterminingCycle < tiffTileSize)
            {
                debugMessageCallback(string.Format("We can't work with the default tilesize of '{0}' so we have to scale it back to RowsPerCycle: '{1}'", tiffTileSize, rowsPerPathDeterminingCycle));
                tiffTileSize = rowsPerPathDeterminingCycle;
            }

            debugMessageCallback(string.Format("TiffTileSize: {0}", tiffTileSize));

            debugMessageCallback("Starting generation of Maze Path and saving maze...");

            //Should actually be Width -1 -1 but since we use the full Width it's only once -1
            //This will count the amount of tiles per line so if it's 15 Pixels we still want 2 tiles of 8
            int tilesInWidth = (((this.Width - 1) / tiffTileSize) + 1);



            ImageInfo imi = new ImageInfo(this.Width - 1, this.Height - 1, 8, false); // 8 bits per channel, no alpha 
            // open image for writing 
            PngWriter png = FileHelper.CreatePngWriter(fileName, imi, true);
            // add some optional metadata (chunks)
            png.GetMetadata().SetDpi(100.0);
            png.GetMetadata().SetTimeNow(0); // 0 seconds fron now = now
            png.CompLevel = 4;
            //png.GetMetadata().SetText(PngChunkTextVar.KEY_Title, "Just a text image");
            //PngChunk chunk = png.GetMetadata().SetText("my key", "my text .. bla bla");
            //chunk.Priority = true; // this chunk will be written as soon as possible




            //int stepsPerLoop = rowsPerPathDeterminingCycle;

            int partNumber = 0;

            int yChunkStart = 0;
            while (yChunkStart < this.Height - 1)
            {
                //We must use rowsperpathdeterminingcycle here instead of tifftilesize because else you might get into a scenario where the first 4 values and the second 4 values are beneath 1000. But if we would take value 2 to 6 which are also 4 values we would go above 1000.
                //And yes I thought about this pretty well, it needs to be like this because you get forced into reading 500 lines of path from for example 1000 to 1500 where the other thing is 2000, hmmmm...
                //Or not I really need to think about this a bit more. Because if the chunk size is 1000 then you can never end up reading something smaller then that which works because the rowsperpath is always bigger.
                //So yeah, because rows per path is always a multiple or equal to tifftilesize you can never go out of sync becuase no matter what happens, e.g. tifftile = 500 and perpath = 2000. When you're at 2500 you just need to read 500. And you are never forced in reading anything that was
                //not measured. Because you can't end up in having to read somewhere from 1250 to 1750 because of the multiple thingy. Ok I'm quite sure now it needs to be tiffTileSize.
                //
                //Additional note, it always needs to be a multiple of tiffTileSize because we write tiles at a time (we can't write half tiles). So that's why we don't want some stupidly small numbers here.
                int stepsThisLoop = FindTheMaxPathRowsThatWouldFitInMemoryFromHerePng(debugMessageCallback, pathPointsPerRow, yChunkStart, tiffTileSize, memoryFree);

                var yChunkEnd = Math.Min(yChunkStart + stepsThisLoop, this.Height - 1);
                stepsThisLoop = yChunkEnd - yChunkStart;

                var wObtainPathPart = Stopwatch.StartNew();

                //We don't use a ToList here because we do actually know the expected list size beforehand. This way we make sure we don't have to do any internal Array Resizing.
                var expectedPathCount = pathPointsPerRow.Skip(yChunkStart).Take(yChunkEnd - yChunkStart).Sum();
                var pathPointsHere = new List<MazePointPos>(expectedPathCount);
                int currentPathPosPoint = 0;
                foreach (var pathPos in pathPosjes.Where(t => t.Y >= yChunkStart && t.Y < yChunkEnd))
                {
                    pathPointsHere.Add(pathPos);
                    currentPathPosPoint++;
                }
                wObtainPathPart.Stop();

                if (pathPointsHere.Count != expectedPathCount)
                {
                    debugMessageCallback(string.Format("Warning: Something strange is happening where the actual path point count '{0}' is not equal to the expected path point count '{1}' (Maze will still save correctly but it uses more memory then expected)", pathPointsHere.Count, expectedPathCount));
                }

                var wSort = Stopwatch.StartNew();
                pathPointsHere.Sort((first, second) =>
                {
                    if (first.Y == second.Y)
                    {
                        return first.X - second.X;
                    }
                    return first.Y - second.Y;
                });
                wSort.Stop();



                var wGmemorifiedPieceOpMap = Stopwatch.StartNew();

                var innerMapTemporaryInMemoryCopy = new BitArreintjeFastInnerMap(this.Width, stepsThisLoop);





                for (int startY = yChunkStart; startY < yChunkEnd; startY += tiffTileSize)
                {

                    for (int startX = 0; startX < this.Width - 1; startX += tiffTileSize)
                    {
                        int yStart = startY - yChunkStart;
                        int yEnd = yStart + tiffTileSize;

                        for (int y = startY, othery = yStart; othery < yEnd; y++, othery++)
                        {
                            for (int x = startX, otherx = 0; otherx < tiffTileSize; x++, otherx++)
                            {
                                innerMapTemporaryInMemoryCopy[x, othery] = innerMap[x, y];
                            }
                        }
                    }


                }



                wGmemorifiedPieceOpMap.Stop();








                int curpos = 0;

                var wSaveAsImage = Stopwatch.StartNew();

                var yChunkMaxRealEnzo = Math.Min(yChunkEnd, this.Height - 1);

                for (int startY = yChunkStart, y = 0; startY < yChunkMaxRealEnzo; startY += 1, y++)
                {
                    ImageLine iline = new ImageLine(imi);

                    //int xMax = Math.Min(this.Width - 1 - startX, tiffTileSize);
                    int yMax = Math.Min(this.Height - 1 - startY, tiffTileSize);
                    for (int x = 0, otherx = 0; otherx < this.Width - 1; x++, otherx++)
                    {
                        byte r = 0;
                        byte g = 0;
                        byte b = 0;

                        MazePointPos curPathPos;
                        if (curpos < pathPointsHere.Count)
                        {
                            curPathPos = pathPointsHere[curpos];
                            if (curPathPos.X == x && curPathPos.Y == y)
                            {
                                r = curPathPos.RelativePos;
                                g = (byte)(255 - curPathPos.RelativePos);
                                b = 0;
                                curpos++;
                            }
                            else if (innerMapTemporaryInMemoryCopy[x, y])
                            {
                                r = 255;
                                g = 255;
                                b = 255;
                            }
                        }
                        else if (innerMapTemporaryInMemoryCopy[x, y])
                        {
                            r = 255;
                            g = 255;
                            b = 255;
                        }



                        ImageLineHelper.SetPixel(iline, x, r, g, b);
                    }



                    //var result = tif.WriteEncodedTile(tileNumber, color_ptr, tiffTileSize * tiffTileSize * 3);
                    //var result = tif.WriteTile(color_ptr, startX / tileSize, startY / tileSize, 0, 0);
                    //var result = tif.WriteRawTile(tileNumber, color_ptr, tileSize * tileSize * 3);
                    //Result should not be -1

                    //lineSavingProgress((int)Math.Min((tileNumber + 1L) * tiffTileSize / tilesInWidth, this.Height - 2), this.Height - 2);
                    png.WriteRow(iline, y + yChunkStart);
                    lineSavingProgress(y + yChunkStart, this.Height - 2);
                }



                wSaveAsImage.Stop();

                debugMessageCallback(string.Format("{0}: YChunkStart: {1}, YChunkEnd: {2}, Rows written: {3}, Count: {4}, Time to generate this part: {5} sec, Time to sort this part: {6} sec, Time to put this part in memory: {7}, Time to save this part in the image: {8} sec, Combined time: {9} sec, Size: {10}mb",
                    partNumber,
                    yChunkStart,
                    yChunkEnd,
                    stepsThisLoop,
                    pathPointsHere.Count,
                    Math.Round(wObtainPathPart.Elapsed.TotalSeconds, 2),
                    Math.Round(wSort.Elapsed.TotalSeconds, 2),
                    Math.Round(wGmemorifiedPieceOpMap.Elapsed.TotalSeconds, 2),
                    Math.Round(wSaveAsImage.Elapsed.TotalSeconds, 2),
                    Math.Round(wObtainPathPart.Elapsed.TotalSeconds + wSort.Elapsed.TotalSeconds + wSaveAsImage.Elapsed.TotalSeconds, 2),
                    Math.Round(pathPointsHere.Count * 9.0 / 1024.0 / 1024.0, 3)));
                partNumber++;

                yChunkStart += stepsThisLoop;

                //Do some forced garbage collection since we're finished with this loop
                pathPointsHere = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            png.End();

            //    tif.FlushData();
            //}
        }

        /// <summary>
        /// Note: This method returns the amount of rows it can obtain for the path in one go based on available memory, not the tifftilesize that should be used.
        /// </summary>
        /// <param name="debugMessageCallback"></param>
        /// <param name="pathPointsPerRow"></param>
        /// <param name="memoryFree"></param>
        /// <returns></returns>
        private int FindTheMinimalRowsToWriteForPng(Action<string> debugMessageCallback, int[] pathPointsPerRow, ulong memoryFree)
        {
            int rowsPerPathDeterminingCycle = 16;

            long max = 0;

            while (true)
            {
                int newTestSize = rowsPerPathDeterminingCycle * 2;
                long maxSizeCur = 0;
                for (int i = 0; i < this.Height; i += newTestSize)
                {
                    long curSize = 0;
                    var endRow = Math.Min(this.Height, i + newTestSize);
                    for (int y = i; y < endRow; y++)
                    {
                        curSize += pathPointsPerRow[y];
                    }

                    maxSizeCur = Math.Max(curSize, maxSizeCur);
                }

                if (maxSizeCur > GlobalVars.MaxArraySize)
                {
                    debugMessageCallback(string.Format("We would have to create a list bigger then int.MaxValue with RowsPerCycle '{0}', so we take '{1}'", newTestSize, rowsPerPathDeterminingCycle));
                    break;
                }

                var expectedMemoryUsageForPath = maxSizeCur * 9; //9 bytes per path pos
                int chunksInWidth = this.Height / newTestSize;
                var expectedMemoryUsageForChunks = (newTestSize * newTestSize / 8) * chunksInWidth;
                var totalMemoryUsage = expectedMemoryUsageForChunks + expectedMemoryUsageForPath;
                if ((ulong)totalMemoryUsage > memoryFree)
                {
                    debugMessageCallback(string.Format("Memory would be full with RowsPerCycle '{0}', so we take '{1}'", newTestSize, rowsPerPathDeterminingCycle));
                    break;
                }

                rowsPerPathDeterminingCycle = newTestSize;
                max = maxSizeCur;

                if (rowsPerPathDeterminingCycle >= this.Height)
                {
                    debugMessageCallback(string.Format("This RowsPerCycle is chosen because it fits in memory and we can do the complete maze in it: '{0}'", rowsPerPathDeterminingCycle));
                    break;
                }

            }

            debugMessageCallback(string.Format("Max size of the minimal rows to write would be: {0}, taking up {1}mb of memory.", max, Math.Round(max * 9.0 / 1024.0 / 1024.0, 2)));

            return rowsPerPathDeterminingCycle;
        }

        private int FindTheMaxPathRowsThatWouldFitInMemoryFromHerePng(Action<string> debugMessageCallback, int[] pathPointsPerRow, int startRow, int stepsToIncreaseWithSize, ulong memoryFree)
        {
            int rowsThatFitInThisCycle = stepsToIncreaseWithSize;

            int maxRowsToCheck = stepsToIncreaseWithSize;
            while (true)
            {
                var newTestSize = maxRowsToCheck + stepsToIncreaseWithSize;
                long curSize = 0;
                var endRow = Math.Min(this.Height, startRow + newTestSize);
                for (int y = startRow; y < endRow; y++)
                {
                    curSize += pathPointsPerRow[y];
                }


                if (curSize > GlobalVars.MaxArraySize)
                {
                    break;
                }

                var expectedMemoryUsageForPath = curSize * 9; //9 bytes per path pos
                int chunksInWidth = this.Height / newTestSize;
                var expectedMemoryUsageForChunks = (newTestSize * newTestSize / 8) * chunksInWidth;
                var totalMemoryUsage = expectedMemoryUsageForChunks + expectedMemoryUsageForPath;
                if ((ulong)totalMemoryUsage > memoryFree)
                {
                    break;
                }

                maxRowsToCheck = newTestSize;

                if (maxRowsToCheck >= (this.Height - startRow))
                {
                    break;
                }
            }

            return maxRowsToCheck;
        }
    }
}
