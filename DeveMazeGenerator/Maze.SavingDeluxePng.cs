using Hjg.Pngcs;
using System;
using System.Collections.Generic;
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
    }
}
