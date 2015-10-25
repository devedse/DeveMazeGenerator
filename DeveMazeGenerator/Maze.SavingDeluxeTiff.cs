using BitMiracle.LibTiff.Classic;
using DeveMazeGenerator.Generators;
using DeveMazeGenerator.InnerMaps;
using Hjg.Pngcs;
using Hjg.Pngcs.Chunks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator
{
    public partial class Maze
    {

        private ushort[][] GetColorMaps()
        {
            int colorMapSize = 256;
            int colorMapSizeMinusTwo = colorMapSize - 2;

            ushort[] colorMapRed = new ushort[colorMapSize];
            ushort[] colorMapGreen = new ushort[colorMapSize];
            ushort[] colorMapBlue = new ushort[colorMapSize];

            for (int i = 0; i < colorMapSize - 2; i++)
            {
                colorMapRed[i] = (ushort)(i * colorMapSize);
                colorMapGreen[i] = (ushort)((colorMapSizeMinusTwo - i) * colorMapSize);
                colorMapBlue[i] = 0;
            }

            colorMapRed[colorMapSize - 2] = 0;
            colorMapGreen[colorMapSize - 2] = 0;
            colorMapBlue[colorMapSize - 2] = 0;

            colorMapRed[colorMapSize - 1] = ushort.MaxValue;
            colorMapGreen[colorMapSize - 1] = ushort.MaxValue;
            colorMapBlue[colorMapSize - 1] = ushort.MaxValue;

            return new ushort[][] { colorMapRed, colorMapGreen, colorMapBlue };
        }



        private void SaveMazeAsImageDeluxeTiff(String fileName, List<MazePointPos> pathPosjes, Action<int, int> lineSavingProgress)
        {
            pathPosjes.Sort((first, second) =>
            {
                if (first.Y == second.Y)
                {
                    return first.X - second.X;
                }
                return first.Y - second.Y;
            });



            using (var tif = Tiff.Open(fileName, "w"))
            {
                if (tif == null)
                {
                    throw new InvalidOperationException("Tif file could not be opened. It is probably in use: " + fileName);
                }

                tif.SetField(TiffTag.IMAGEWIDTH, this.Width - 1);
                tif.SetField(TiffTag.IMAGELENGTH, this.Height - 1);
                tif.SetField(TiffTag.BITSPERSAMPLE, 8);
                tif.SetField(TiffTag.SAMPLESPERPIXEL, 3);
                tif.SetField(TiffTag.PHOTOMETRIC, Photometric.RGB);
                tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                tif.SetField(TiffTag.ROWSPERSTRIP, 1);
                tif.SetField(TiffTag.COMPRESSION, Compression.LZW);


                int curpos = 0;

                byte[] color_ptr = new byte[(this.Width - 1) * 3];

                for (int y = 0; y < this.Height - 1; y++)
                {

                    for (int x = 0; x < this.Width - 1; x++)
                    {
                        byte r = 0;
                        byte g = 0;
                        byte b = 0;

                        MazePointPos curPathPos;
                        if (curpos < pathPosjes.Count)
                        {
                            curPathPos = pathPosjes[curpos];
                            if (curPathPos.X == x && curPathPos.Y == y)
                            {
                                r = curPathPos.RelativePos;
                                g = (byte)(255 - curPathPos.RelativePos);
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

                        color_ptr[x * 3 + 0] = r;
                        color_ptr[x * 3 + 1] = g;
                        color_ptr[x * 3 + 2] = b;
                    }
                    tif.WriteScanline(color_ptr, y);
                    lineSavingProgress(y, this.Height - 2);
                }

                tif.FlushData();
            }
        }


        private void SaveMazeAsImageDeluxeTiffWithChunks(String fileName, List<MazePointPos> pathPosjes, Action<int, int> lineSavingProgress)
        {
            const int tileSize = HybridInnerMap.GridSize;

            //Should actually be Width -1 -1 but since we use the full Width it's only once -1
            //This will count the amount of tiles per line so if it's 15 Pixels we still want 2 tiles of 8
            int tilesInWidth = (((this.Width - 1) / tileSize) + 1);

            pathPosjes.Sort((first, second) =>
            {
                int firstXTile = first.X / tileSize;
                int firstYTile = first.Y / tileSize;

                int secondXTile = second.X / tileSize;
                int secondYTile = second.Y / tileSize;

                if (firstYTile != secondYTile)
                {
                    return firstYTile - secondYTile;
                }
                if (firstXTile != secondXTile)
                {
                    return firstXTile - secondXTile;
                }

                int firstXInTile = first.X % tileSize;
                int firstYInTile = first.Y % tileSize;

                int secondXInTile = second.X % tileSize;
                int secondYInTile = second.Y % tileSize;

                if (firstYInTile == secondYInTile)
                {
                    return firstXInTile - secondXInTile;
                }
                return firstYInTile - secondYInTile;
            });

            using (var tif = Tiff.Open(fileName, "w"))
            {
                if (tif == null)
                {
                    throw new InvalidOperationException("Tif file could not be opened. It is probably in use: " + fileName);
                }

                tif.SetField(TiffTag.IMAGEWIDTH, this.Width - 1);
                tif.SetField(TiffTag.IMAGELENGTH, this.Height - 1);
                tif.SetField(TiffTag.BITSPERSAMPLE, 8);
                tif.SetField(TiffTag.SAMPLESPERPIXEL, 3);
                tif.SetField(TiffTag.PHOTOMETRIC, Photometric.RGB);
                tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                //tif.SetField(TiffTag.ROWSPERSTRIP, 1);
                tif.SetField(TiffTag.COMPRESSION, Compression.LZW);

                tif.SetField(TiffTag.TILEWIDTH, tileSize);
                tif.SetField(TiffTag.TILELENGTH, tileSize);

                int curpos = 0;

                byte[] color_ptr = new byte[tileSize * tileSize * 3];

                int tileNumber = 0;
                for (int startY = 0; startY < this.Height - 1; startY += tileSize)
                {

                    for (int startX = 0; startX < this.Width - 1; startX += tileSize)
                    {
                        int xMax = Math.Min(this.Width - 1 - startX, tileSize);
                        int yMax = Math.Min(this.Height - 1 - startY, tileSize);

                        for (int y = startY, othery = 0; othery < tileSize; y++, othery++)
                        {
                            for (int x = startX, otherx = 0; otherx < tileSize; x++, otherx++)
                            {
                                byte r = 0;
                                byte g = 0;
                                byte b = 0;
                                if (otherx >= xMax || othery >= yMax)
                                {
                                    //Not sure if needed but I'd like to ensure that any additional bytes
                                    //written to the image are 0.
                                }
                                else
                                {
                                    MazePointPos curPathPos;
                                    if (curpos < pathPosjes.Count)
                                    {
                                        curPathPos = pathPosjes[curpos];
                                        if (curPathPos.X == x && curPathPos.Y == y)
                                        {
                                            r = curPathPos.RelativePos;
                                            g = (byte)(255 - curPathPos.RelativePos);
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
                                }
                                int startPos = othery * tileSize * 3 + otherx * 3;

                                color_ptr[startPos + 0] = r;
                                color_ptr[startPos + 1] = g;
                                color_ptr[startPos + 2] = b;
                            }

                        }

                        var result = tif.WriteEncodedTile(tileNumber, color_ptr, tileSize * tileSize * 3);
                        //var result = tif.WriteTile(color_ptr, startX / tileSize, startY / tileSize, 0, 0);
                        //var result = tif.WriteRawTile(tileNumber, color_ptr, tileSize * tileSize * 3);
                        //Result should not be -1

                        lineSavingProgress((int)Math.Min((tileNumber + 1L) * tileSize / tilesInWidth, this.Height - 2), this.Height - 2);

                        tileNumber++;
                    }


                }

                tif.FlushData();
            }
        }


        private void SaveMazeAsImageDeluxeTiffWithColorMap(String fileName, List<MazePointPos> pathPosjes, Action<int, int> lineSavingProgress)
        {
            pathPosjes.Sort((first, second) =>
            {
                if (first.Y == second.Y)
                {
                    return first.X - second.X;
                }
                return first.Y - second.Y;
            });



            using (var tif = Tiff.Open(fileName, "w"))
            {
                if (tif == null)
                {
                    throw new InvalidOperationException("Tif file could not be opened. It is probably in use: " + fileName);
                }

                tif.SetField(TiffTag.IMAGEWIDTH, this.Width - 1);
                tif.SetField(TiffTag.IMAGELENGTH, this.Height - 1);
                tif.SetField(TiffTag.BITSPERSAMPLE, 8);
                tif.SetField(TiffTag.SAMPLESPERPIXEL, 1);
                tif.SetField(TiffTag.PHOTOMETRIC, Photometric.PALETTE);
                tif.SetField(TiffTag.COLORMAP, GetColorMaps());
                tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                tif.SetField(TiffTag.ROWSPERSTRIP, 1);
                tif.SetField(TiffTag.COMPRESSION, Compression.LZW);

                int curpos = 0;

                byte[] color_ptr = new byte[this.Width - 1];

                for (int y = 0; y < this.Height - 1; y++)
                {

                    for (int x = 0; x < this.Width - 1; x++)
                    {
                        byte ding = 254;

                        MazePointPos curPathPos;
                        if (curpos < pathPosjes.Count)
                        {
                            curPathPos = pathPosjes[curpos];
                            if (curPathPos.X == x && curPathPos.Y == y)
                            {
                                float reinterpreted = ((float)curPathPos.RelativePos) / 255.0f * 253.0f;

                                ding = (byte)((byte)reinterpreted);
                                curpos++;
                            }
                            else if (this.innerMap[x, y])
                            {
                                ding = 255;
                            }
                        }
                        else if (this.innerMap[x, y])
                        {
                            ding = 255;
                        }

                        color_ptr[x] = ding;
                    }
                    tif.WriteScanline(color_ptr, y);
                    lineSavingProgress(y, this.Height - 2);
                }

                tif.FlushData();
            }
        }


        private void SaveMazeAsImageDeluxeTiffWithDynamicallyGeneratedPath(String fileName, IEnumerable<MazePointPos> pathPosjes, Action<int, int> lineSavingProgress, Action<string> debugMessageCallback = null)
        {
            if (debugMessageCallback == null)
            {
                debugMessageCallback = (x) => { };
            }


            const int tifTileSize = HybridInnerMap.GridSize;

            //Should actually be Width -1 -1 but since we use the full Width it's only once -1
            //This will count the amount of tiles per line so if it's 15 Pixels we still want 2 tiles of 8
            int tilesInWidth = (((this.Width - 1) / tifTileSize) + 1);



            using (var tif = Tiff.Open(fileName, "w"))
            {
                if (tif == null)
                {
                    throw new InvalidOperationException("Tif file could not be opened. It is probably in use: " + fileName);
                }

                tif.SetField(TiffTag.IMAGEWIDTH, this.Width - 1);
                tif.SetField(TiffTag.IMAGELENGTH, this.Height - 1);
                tif.SetField(TiffTag.BITSPERSAMPLE, 8);
                tif.SetField(TiffTag.SAMPLESPERPIXEL, 3);
                tif.SetField(TiffTag.PHOTOMETRIC, Photometric.RGB);
                tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                //tif.SetField(TiffTag.ROWSPERSTRIP, 1);
                tif.SetField(TiffTag.COMPRESSION, Compression.LZW);

                tif.SetField(TiffTag.TILEWIDTH, tifTileSize);
                tif.SetField(TiffTag.TILELENGTH, tifTileSize);

                byte[] color_ptr = new byte[tifTileSize * tifTileSize * 3];

                int stepsPerLoop = tifTileSize * Maze.LineChunkCount;

                int tileNumber = 0;
                int partNumber = 0;
                for (int yChunkStart = 0; yChunkStart < this.Height - 1; yChunkStart += stepsPerLoop)
                {
                    var yChunkEnd = Math.Min(yChunkStart + stepsPerLoop, this.Height - 1);

                    var w = Stopwatch.StartNew();

                    var pathPointsHere = pathPosjes.Where(t => t.Y >= yChunkStart && t.Y < yChunkEnd).ToList();
                    pathPointsHere.Sort((first, second) =>
                    {
                        int firstXTile = first.X / tifTileSize;
                        int firstYTile = first.Y / tifTileSize;

                        int secondXTile = second.X / tifTileSize;
                        int secondYTile = second.Y / tifTileSize;

                        if (firstYTile != secondYTile)
                        {
                            return firstYTile - secondYTile;
                        }
                        if (firstXTile != secondXTile)
                        {
                            return firstXTile - secondXTile;
                        }

                        int firstXInTile = first.X % tifTileSize;
                        int firstYInTile = first.Y % tifTileSize;

                        int secondXInTile = second.X % tifTileSize;
                        int secondYInTile = second.Y % tifTileSize;

                        if (firstYInTile == secondYInTile)
                        {
                            return firstXInTile - secondXInTile;
                        }
                        return firstYInTile - secondYInTile;
                    });

                    debugMessageCallback(string.Format("{0}: YChunkStart: {1}, YChunkEnd: {2}, Count: {3}, Time to generate this part: {4} sec, Size: {5}mb", partNumber, yChunkStart, yChunkEnd, pathPointsHere.Count, Math.Round(w.Elapsed.TotalSeconds, 2), Math.Round(pathPointsHere.Count * 9.0 / 1024.0 / 1024.0, 3)));
                    partNumber++;

                    int curpos = 0;

                    for (int startY = yChunkStart; startY < yChunkEnd; startY += tifTileSize)
                    {

                        for (int startX = 0; startX < this.Width - 1; startX += tifTileSize)
                        {
                            int xMax = Math.Min(this.Width - 1 - startX, tifTileSize);
                            int yMax = Math.Min(this.Height - 1 - startY, tifTileSize);

                            for (int y = startY, othery = 0; othery < tifTileSize; y++, othery++)
                            {
                                for (int x = startX, otherx = 0; otherx < tifTileSize; x++, otherx++)
                                {
                                    byte r = 0;
                                    byte g = 0;
                                    byte b = 0;
                                    if (otherx >= xMax || othery >= yMax)
                                    {
                                        //Not sure if needed but I'd like to ensure that any additional bytes
                                        //written to the image are 0.
                                    }
                                    else
                                    {
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
                                    }
                                    int startPos = othery * tifTileSize * 3 + otherx * 3;

                                    color_ptr[startPos + 0] = r;
                                    color_ptr[startPos + 1] = g;
                                    color_ptr[startPos + 2] = b;
                                }

                            }

                            var result = tif.WriteEncodedTile(tileNumber, color_ptr, tifTileSize * tifTileSize * 3);
                            //var result = tif.WriteTile(color_ptr, startX / tileSize, startY / tileSize, 0, 0);
                            //var result = tif.WriteRawTile(tileNumber, color_ptr, tileSize * tileSize * 3);
                            //Result should not be -1

                            lineSavingProgress((int)Math.Min((tileNumber + 1L) * tifTileSize / tilesInWidth, this.Height - 2), this.Height - 2);

                            tileNumber++;
                        }


                    }

                    //Do some forced garbage collection since we're finished with this loop
                    pathPointsHere = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }


                tif.FlushData();
            }
        }


        /// <summary>
        /// This method performs a preanalysis on the path to make sure there's no super high memory usage for a certain area
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="pathPosjes"></param>
        /// <param name="lineSavingProgress"></param>
        /// <param name="debugMessageCallback"></param>
        private void SaveMazeAsImageDeluxeTiffWithDynamicallyGeneratedPathWithAnalysis(String fileName, IEnumerable<MazePointPos> pathPosjes, Action<int, int> lineSavingProgress, Action<string> debugMessageCallback = null)
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
            int rowsPerPathDeterminingCycle = FindTheMinimalRowsToWrite(debugMessageCallback, pathPointsPerRow, memoryFree);



            int tifTileSize = HybridInnerMap.GridSize;

            if (rowsPerPathDeterminingCycle < tifTileSize)
            {
                debugMessageCallback(string.Format("We can't work with the default tilesize of '{0}' so we have to scale it back to RowsPerCycle: '{1}'", tifTileSize, rowsPerPathDeterminingCycle));
                tifTileSize = rowsPerPathDeterminingCycle;
            }

            debugMessageCallback(string.Format("TiffTileSize: {0}", tifTileSize));

            debugMessageCallback("Starting generation of Maze Path and saving maze...");

            //Should actually be Width -1 -1 but since we use the full Width it's only once -1
            //This will count the amount of tiles per line so if it's 15 Pixels we still want 2 tiles of 8
            int tilesInWidth = (((this.Width - 1) / tifTileSize) + 1);



            using (var tif = Tiff.Open(fileName, "w"))
            {
                if (tif == null)
                {
                    throw new InvalidOperationException("Tif file could not be opened. It is probably in use: " + fileName);
                }

                tif.SetField(TiffTag.IMAGEWIDTH, this.Width - 1);
                tif.SetField(TiffTag.IMAGELENGTH, this.Height - 1);
                tif.SetField(TiffTag.BITSPERSAMPLE, 8);
                tif.SetField(TiffTag.SAMPLESPERPIXEL, 3);
                tif.SetField(TiffTag.PHOTOMETRIC, Photometric.RGB);
                tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                //tif.SetField(TiffTag.ROWSPERSTRIP, 1);
                tif.SetField(TiffTag.COMPRESSION, Compression.LZW);

                tif.SetField(TiffTag.TILEWIDTH, tifTileSize);
                tif.SetField(TiffTag.TILELENGTH, tifTileSize);

                byte[] color_ptr = new byte[tifTileSize * tifTileSize * 3];

                //int stepsPerLoop = rowsPerPathDeterminingCycle;

                int tileNumber = 0;
                int partNumber = 0;

                int yChunkStart = 0;
                while (yChunkStart < this.Height - 1)
                {
                    //We must use rowsperpathdeterminingcycle here instead of tiftilesize because else you might get into a scenario where the first 4 values and the second 4 values are beneath 1000. But if we would take value 2 to 6 which are also 4 values we would go above 1000.
                    //And yes I thought about this pretty well, it needs to be like this because you get forced into reading 500 lines of path from for example 1000 to 1500 where the other thing is 2000, hmmmm...
                    //Or not I really need to think about this a bit more. Because if the chunk size is 1000 then you can never end up reading something smaller then that which works because the rowsperpath is always bigger.
                    //So yeah, because rows per path is always a multiple or equal to tiftilesize you can never go out of sync becuase no matter what happens, e.g. tiftile = 500 and perpath = 2000. When you're at 2500 you just need to read 500. And you are never forced in reading anything that was
                    //not measured. Because you can't end up in having to read somewhere from 1250 to 1750 because of the multiple thingy. Ok I'm quite sure now it needs to be tifTileSize.
                    //
                    //Additional note, it always needs to be a multiple of tifTileSize because we write tiles at a time (we can't write half tiles). So that's why we don't want some stupidly small numbers here.
                    int stepsThisLoop = FindTheMaxPathRowsThatWouldFitInMemoryFromHere(debugMessageCallback, pathPointsPerRow, yChunkStart, tifTileSize, memoryFree);

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
                        int firstXTile = first.X / tifTileSize;
                        int firstYTile = first.Y / tifTileSize;

                        int secondXTile = second.X / tifTileSize;
                        int secondYTile = second.Y / tifTileSize;

                        if (firstYTile != secondYTile)
                        {
                            return firstYTile - secondYTile;
                        }
                        if (firstXTile != secondXTile)
                        {
                            return firstXTile - secondXTile;
                        }

                        int firstXInTile = first.X % tifTileSize;
                        int firstYInTile = first.Y % tifTileSize;

                        int secondXInTile = second.X % tifTileSize;
                        int secondYInTile = second.Y % tifTileSize;

                        if (firstYInTile == secondYInTile)
                        {
                            return firstXInTile - secondXInTile;
                        }
                        return firstYInTile - secondYInTile;
                    });
                    wSort.Stop();

                    int curpos = 0;

                    var wSaveAsImage = Stopwatch.StartNew();

                    for (int startY = yChunkStart; startY < yChunkEnd; startY += tifTileSize)
                    {

                        for (int startX = 0; startX < this.Width - 1; startX += tifTileSize)
                        {
                            int xMax = Math.Min(this.Width - 1 - startX, tifTileSize);
                            int yMax = Math.Min(this.Height - 1 - startY, tifTileSize);

                            for (int y = startY, othery = 0; othery < tifTileSize; y++, othery++)
                            {
                                for (int x = startX, otherx = 0; otherx < tifTileSize; x++, otherx++)
                                {
                                    byte r = 0;
                                    byte g = 0;
                                    byte b = 0;
                                    if (otherx >= xMax || othery >= yMax)
                                    {
                                        //Not sure if needed but I'd like to ensure that any additional bytes
                                        //written to the image are 0.
                                    }
                                    else
                                    {
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
                                    }
                                    int startPos = othery * tifTileSize * 3 + otherx * 3;

                                    color_ptr[startPos + 0] = r;
                                    color_ptr[startPos + 1] = g;
                                    color_ptr[startPos + 2] = b;
                                }

                            }

                            var result = tif.WriteEncodedTile(tileNumber, color_ptr, tifTileSize * tifTileSize * 3);
                            //var result = tif.WriteTile(color_ptr, startX / tileSize, startY / tileSize, 0, 0);
                            //var result = tif.WriteRawTile(tileNumber, color_ptr, tileSize * tileSize * 3);
                            //Result should not be -1

                            lineSavingProgress((int)Math.Min((tileNumber + 1L) * tifTileSize / tilesInWidth, this.Height - 2), this.Height - 2);

                            tileNumber++;
                        }


                    }
                    wSaveAsImage.Stop();

                    debugMessageCallback(string.Format("{0}: YChunkStart: {1}, YChunkEnd: {2}, Rows written: {3}, Count: {4}, Time to generate this part: {5} sec, Time to sort this part: {6} sec, Time to save this part in the image: {7} sec, Combined time: {8} sec, Size: {9}mb",
                        partNumber,
                        yChunkStart,
                        yChunkEnd,
                        stepsThisLoop,
                        pathPointsHere.Count,
                        Math.Round(wObtainPathPart.Elapsed.TotalSeconds, 2),
                        Math.Round(wSort.Elapsed.TotalSeconds, 2),
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


                tif.FlushData();
            }
        }



        public void Testje(int size, Action<int, int> lineSavingProgress, Action<string> debugMessageCallback = null)
        {
            if (debugMessageCallback == null)
            {
                debugMessageCallback = (x) => { };
            }
            string fileName = "testje123123.png";

            int tifTileSize = 4096;
            int tilesInWidth = (((size - 1) / tifTileSize) + 1);
            
            debugMessageCallback(string.Format("TiffTileSize: {0}", tifTileSize));

            var frandom = new FastRandom(1337);

            double lastTime = 0;

            using (var tif = Tiff.Open(fileName, "w"))
            {
                if (tif == null)
                {
                    throw new InvalidOperationException("Tif file could not be opened. It is probably in use: " + fileName);
                }

                tif.SetField(TiffTag.IMAGEWIDTH, size - 1);
                tif.SetField(TiffTag.IMAGELENGTH, size - 1);
                tif.SetField(TiffTag.BITSPERSAMPLE, 8);
                tif.SetField(TiffTag.SAMPLESPERPIXEL, 3);
                tif.SetField(TiffTag.PHOTOMETRIC, Photometric.RGB);
                tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                //tif.SetField(TiffTag.ROWSPERSTRIP, 1);
                tif.SetField(TiffTag.COMPRESSION, Compression.LZW);

                tif.SetField(TiffTag.TILEWIDTH, tifTileSize);
                tif.SetField(TiffTag.TILELENGTH, tifTileSize);

                byte[] color_ptr = new byte[tifTileSize * tifTileSize * 3];

                //int stepsPerLoop = rowsPerPathDeterminingCycle;

                int tileNumber = 0;
                int partNumber = 0;

                int yChunkStart = 0;
                while (yChunkStart < size - 1)
                {
                    //We must use rowsperpathdeterminingcycle here instead of tiftilesize because else you might get into a scenario where the first 4 values and the second 4 values are beneath 1000. But if we would take value 2 to 6 which are also 4 values we would go above 1000.
                    //And yes I thought about this pretty well, it needs to be like this because you get forced into reading 500 lines of path from for example 1000 to 1500 where the other thing is 2000, hmmmm...
                    //Or not I really need to think about this a bit more. Because if the chunk size is 1000 then you can never end up reading something smaller then that which works because the rowsperpath is always bigger.
                    //So yeah, because rows per path is always a multiple or equal to tiftilesize you can never go out of sync becuase no matter what happens, e.g. tiftile = 500 and perpath = 2000. When you're at 2500 you just need to read 500. And you are never forced in reading anything that was
                    //not measured. Because you can't end up in having to read somewhere from 1250 to 1750 because of the multiple thingy. Ok I'm quite sure now it needs to be tifTileSize.
                    //
                    //Additional note, it always needs to be a multiple of tifTileSize because we write tiles at a time (we can't write half tiles). So that's why we don't want some stupidly small numbers here.
                    //int stepsThisLoop = FindTheMaxPathRowsThatWouldFitInMemoryFromHere(debugMessageCallback, pathPointsPerRow, yChunkStart, tifTileSize, memoryFree);

                    var stepsThisLoop = 8192;

                    var yChunkEnd = Math.Min(yChunkStart + stepsThisLoop, size - 1);
                    //stepsThisLoop = yChunkEnd - yChunkStart;

                    //var wObtainPathPart = Stopwatch.StartNew();

                    ////We don't use a ToList here because we do actually know the expected list size beforehand. This way we make sure we don't have to do any internal Array Resizing.
                    //var expectedPathCount = pathPointsPerRow.Skip(yChunkStart).Take(yChunkEnd - yChunkStart).Sum();
                    //var pathPointsHere = new List<MazePointPos>(expectedPathCount);
                    //int currentPathPosPoint = 0;
                    //foreach (var pathPos in pathPosjes.Where(t => t.Y >= yChunkStart && t.Y < yChunkEnd))
                    //{
                    //    pathPointsHere.Add(pathPos);
                    //    currentPathPosPoint++;
                    //}
                    //wObtainPathPart.Stop();

                    //if (pathPointsHere.Count != expectedPathCount)
                    //{
                    //    debugMessageCallback(string.Format("Warning: Something strange is happening where the actual path point count '{0}' is not equal to the expected path point count '{1}' (Maze will still save correctly but it uses more memory then expected)", pathPointsHere.Count, expectedPathCount));
                    //}

                    //var wSort = Stopwatch.StartNew();
                    //pathPointsHere.Sort((first, second) =>
                    //{
                    //    int firstXTile = first.X / tifTileSize;
                    //    int firstYTile = first.Y / tifTileSize;

                    //    int secondXTile = second.X / tifTileSize;
                    //    int secondYTile = second.Y / tifTileSize;

                    //    if (firstYTile != secondYTile)
                    //    {
                    //        return firstYTile - secondYTile;
                    //    }
                    //    if (firstXTile != secondXTile)
                    //    {
                    //        return firstXTile - secondXTile;
                    //    }

                    //    int firstXInTile = first.X % tifTileSize;
                    //    int firstYInTile = first.Y % tifTileSize;

                    //    int secondXInTile = second.X % tifTileSize;
                    //    int secondYInTile = second.Y % tifTileSize;

                    //    if (firstYInTile == secondYInTile)
                    //    {
                    //        return firstXInTile - secondXInTile;
                    //    }
                    //    return firstYInTile - secondYInTile;
                    //});
                    //wSort.Stop();

                    //int curpos = 0;

                    var wSaveAsImage = Stopwatch.StartNew();

                    for (int startY = yChunkStart; startY < yChunkEnd; startY += tifTileSize)
                    {

                        for (int startX = 0; startX < size - 1; startX += tifTileSize)
                        {
                            int xMax = Math.Min(size - 1 - startX, tifTileSize);
                            int yMax = Math.Min(size - 1 - startY, tifTileSize);

                            for (int y = startY, othery = 0; othery < tifTileSize; y++, othery++)
                            {
                                for (int x = startX, otherx = 0; otherx < tifTileSize; x++, otherx++)
                                {
                                    byte r = 0;
                                    byte g = 0;
                                    byte b = 0;
                                    if (otherx >= xMax || othery >= yMax)
                                    {
                                        //Not sure if needed but I'd like to ensure that any additional bytes
                                        //written to the image are 0.
                                    }
                                    else
                                    {
                                        if (frandom.NextBool())
                                        {
                                            r = 255;
                                            g = 10;
                                            b = 10;
                                        }
                                        //some random stuff:

                                        //MazePointPos curPathPos;
                                        //if (curpos < pathPointsHere.Count)
                                        //{
                                        //    curPathPos = pathPointsHere[curpos];
                                        //    if (curPathPos.X == x && curPathPos.Y == y)
                                        //    {
                                        //        r = curPathPos.RelativePos;
                                        //        g = (byte)(255 - curPathPos.RelativePos);
                                        //        b = 0;
                                        //        curpos++;
                                        //    }
                                        //    else if (this.innerMap[x, y])
                                        //    {
                                        //        r = 255;
                                        //        g = 255;
                                        //        b = 255;
                                        //    }
                                        //}
                                        //else if (this.innerMap[x, y])
                                        //{
                                        //    r = 255;
                                        //    g = 255;
                                        //    b = 255;
                                        //}
                                    }
                                    int startPos = othery * tifTileSize * 3 + otherx * 3;

                                    color_ptr[startPos + 0] = r;
                                    color_ptr[startPos + 1] = g;
                                    color_ptr[startPos + 2] = b;
                                }

                            }

                            var wblah = Stopwatch.StartNew();
                            var result = tif.WriteEncodedTile(tileNumber, color_ptr, tifTileSize * tifTileSize * 3);
                            wblah.Stop();

                            lastTime += wblah.Elapsed.TotalSeconds;
                            //var result = tif.WriteTile(color_ptr, startX / tileSize, startY / tileSize, 0, 0);
                            //var result = tif.WriteRawTile(tileNumber, color_ptr, tileSize * tileSize * 3);
                            //Result should not be -1

                            lineSavingProgress((int)Math.Min((tileNumber + 1L) * tifTileSize / tilesInWidth, size - 2), size - 2);

                            tileNumber++;
                        }


                    }
                    wSaveAsImage.Stop();

                    debugMessageCallback(string.Format("{0}: YChunkStart: {1}, YChunkEnd: {2}, Rows written: {3}, Count: {4}, Time to generate this part: {5} sec, Time to sort this part: {6} sec, Time to save this part in the image: {7} sec, Combined time: {8} sec, Size: {9}mb",
                        partNumber,
                        yChunkStart,
                        yChunkEnd,
                        stepsThisLoop,
                        "",
                        "",
                        Math.Round(lastTime, 2),
                        Math.Round(wSaveAsImage.Elapsed.TotalSeconds, 2),
                        "",
                        ""));
                    partNumber++;
                    lastTime = 0;

                    yChunkStart += stepsThisLoop;

                    //Do some forced garbage collection since we're finished with this loop
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }


                tif.FlushData();
            }
        }


        private int FindTheMaxPathRowsThatWouldFitInMemoryFromHere(Action<string> debugMessageCallback, int[] pathPointsPerRow, int startRow, int stepsToIncreaseWithSize, ulong memoryFree)
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

                var expectedMemoryUsage = curSize * 9; //9 bytes per path pos
                if ((ulong)expectedMemoryUsage > memoryFree)
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

        private int FindTheMinimalRowsToWrite(Action<string> debugMessageCallback, int[] pathPointsPerRow, ulong memoryFree)
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

                var expectedMemoryUsage = maxSizeCur * 9; //9 bytes per path pos
                if ((ulong)expectedMemoryUsage > memoryFree)
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
    }
}
