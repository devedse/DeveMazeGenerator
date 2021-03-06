﻿using DeveMazeGenerator.Generators;
using DeveMazeGenerator.InnerMaps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator
{
    public enum MazeSaveType
    {
        [Description("Uses 4 bits for each pixel color in the saved image")]
        ColorDepth4Bits,

        [Description("Uses 32 bits for each pixel color in the saved image (Uses more memory when saving too)")]
        ColorDepth32Bits
    }

    public partial class Maze
    {
        /// <summary>
        /// Saves the maze with 1 bit per pixel
        /// </summary>
        /// <param name="filename">The filename of the file</param>
        public void SaveAsBinaryFile(String filename)
        {
            using (var fileStream = new FileStream(filename, FileMode.Create))
            {
                for (int y = 0; y < this.Height; y++)
                {
                    BitArreintjeFastInnerMapArray lineOfData = new BitArreintjeFastInnerMapArray(this.Width);
                    for (int x = 0; x < this.Width; x++)
                    {
                        lineOfData[x] = this.innerMap[x, y];
                    }

                    byte[] bytesData = new byte[lineOfData.innerData.Length * sizeof(int)];
                    System.Buffer.BlockCopy(lineOfData.innerData, 0, bytesData, 0, bytesData.Length);

                    fileStream.Write(bytesData, 0, bytesData.Length);
                }
            }
        }

        /// <summary>
        /// Most memory efficient way of saving a maze
        /// Uses 1 bit per pixel
        /// </summary>
        /// <param name="fileName">The filename of the file</param>
        /// <param name="imageFormat">The format the image should be saved in (I suggest Bmp or Png)</param>
        public void SaveMazeAsImage(String fileName, ImageFormat imageFormat)
        {
            using (Bitmap objBmpImage = new Bitmap(innerMap.Width - 1, innerMap.Height - 1, PixelFormat.Format1bppIndexed))
            {

                Rectangle rect = new Rectangle(0, 0, objBmpImage.Width, objBmpImage.Height);
                System.Drawing.Imaging.BitmapData bmpData =
                    objBmpImage.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    objBmpImage.PixelFormat);

                IntPtr ptr = bmpData.Scan0;

                int bytes = Math.Abs(bmpData.Stride) * objBmpImage.Height;
                byte[] rgbValues = new byte[bytes];

                System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

                for (int y = 0; y < innerMap.Height - 1; y++)
                {
                    int counterdeluxe = bmpData.Stride * y;
                    int x = 0;
                    for (int i = 0; i < bmpData.Stride; i++)
                    {
                        if (x > objBmpImage.Width)
                        {
                            break;
                        }

                        BitArray bitar = new BitArray(8);

                        for (int j = 7; j >= 0; j--)
                        {
                            if (x < innerMap.Width)
                            {
                                bitar[j] = innerMap[x, y];
                                x++;
                            }
                        }
                        rgbValues[counterdeluxe] = (byte)GetIntFromBitArray(bitar);
                        counterdeluxe++;
                    }
                }

                System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

                objBmpImage.UnlockBits(bmpData);

                objBmpImage.Save(fileName, imageFormat);
            }
        }

        /// <summary>
        /// Saves the maze with a specified path
        /// Uses more memory then saving without a path (depending on the selected MazeSaveType)
        /// </summary>
        /// <param name="fileName">The filename of the file</param>
        /// <param name="imageFormat">The format the image should be saved in (I suggest Bmp or Png)</param>
        /// <param name="path">The path (can be generated by calling PathFinderDepthFirst.GoFind)</param>
        /// <param name="mazeSaveType">The bit depth the maze should be saved in.</param>
        public void SaveMazeAsImage(String fileName, ImageFormat imageFormat, List<MazePoint> path, MazeSaveType mazeSaveType)
        {
            switch (mazeSaveType)
            {
                case MazeSaveType.ColorDepth4Bits:
                    SaveMazeAsImagePath4Bit(fileName, imageFormat, path);
                    break;
                case MazeSaveType.ColorDepth32Bits:
                    SaveMazeAsImagePath32Bit(fileName, imageFormat, path);
                    break;
                default:
                    throw new Exception("No mazeSaveType selected");
            }
        }

        private void SaveMazeAsImagePath32Bit(String fileName, ImageFormat imageFormat, List<MazePoint> path)
        {
            /*
             * Are you 32 bit or 64 bit? Generally images use a single block of contiguous memory.
             * Getting 1 gig in 32 bit block of contiguous memory can be a challenge. – TNT 1 hour ago
             *
             *@user1149816: See for example this page. The basic trick is to call Bitmap.LockBits 
             *to get a BitmapData, which can then be accessed as a pointer to raw data (using either
             *an IntPtr or a byte*, the latter of which tends to be much faster). – Brian 44 mins ago
            */
            int cursize = 1;

            Bitmap objBmpImage = new Bitmap(cursize * (Width - 1), cursize * (Height - 1), PixelFormat.Format32bppArgb);
            Graphics objGraphics = Graphics.FromImage(objBmpImage);

            // Add the colors to the new bitmap.
            objGraphics = Graphics.FromImage(objBmpImage);

            // Set Background color
            objGraphics.Clear(Color.Black);
            objGraphics.SmoothingMode = SmoothingMode.None;
            //objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            //objGraphics.DrawString(sImageText, objFont, new SolidBrush(Color.FromArgb(102, 102, 102)), 0, 0);

            for (int y = 0; y < Height - 1; y++)
            {
                for (int x = 0; x < Width - 1; x++)
                {
                    if (innerMap[x, y] == true)
                    {
                        objGraphics.FillRegion(Brushes.White, new Region(new Rectangle(x * cursize, y * cursize, cursize, cursize)));
                    }
                }
            }

            for (int i = 0; i < path.Count; i++)
            {
                MazePoint n = path[i];

                int formulathing = (int)((double)i / (double)path.Count * 255.0);

                //rgb
                Color brushColor = Color.FromArgb(formulathing, 255 - formulathing, 0);

                objGraphics.FillRegion(new SolidBrush(brushColor), new Region(new Rectangle(n.X * cursize, n.Y * cursize, cursize, cursize)));
            }


            objGraphics.Flush();
            objGraphics.Dispose();

            objBmpImage.Save(fileName, imageFormat);

        }

        private void SaveMazeAsImagePath4Bit(String fileName, ImageFormat imageFormat, List<MazePoint> path)
        {

            using (Bitmap objBmpImage = new Bitmap(innerMap.Width - 1, innerMap.Height - 1, PixelFormat.Format4bppIndexed))
            {

                Rectangle rect = new Rectangle(0, 0, objBmpImage.Width, objBmpImage.Height);
                System.Drawing.Imaging.BitmapData bmpData =
                    objBmpImage.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    objBmpImage.PixelFormat);

                IntPtr ptr = bmpData.Scan0;

                int bytes = Math.Abs(bmpData.Stride) * objBmpImage.Height;
                byte[] rgbValues = new byte[bytes];

                System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

                //BitArreintjeFast[] pathding = new BitArreintjeFast[width];
                //for (int x = 0; x < width; x++)
                //{
                //    pathding[x] = new BitArreintjeFast(height);
                //    for (int y = 0; y < height; y++)
                //    {
                //        pathding[x][y] = false;
                //    }
                //}

                //foreach (MazePoint p in path)
                //{
                //    pathding[p.X][p.Y] = true;
                //}

                for (int y = 0; y < innerMap.Height - 1; y++)
                {
                    int counterdeluxe = bmpData.Stride * y;
                    int x = 0;
                    for (int i = 0; i < bmpData.Stride; i++)
                    {
                        if (x > objBmpImage.Width)
                        {
                            break;
                        }

                        BitArray bitar = new BitArray(8);

                        for (int j = 4; j >= 0; j = j - 4)
                        {
                            if (innerMap[x, y])
                            {
                                bitar[j + 3] = true;
                                bitar[j + 2] = true;
                                bitar[j + 1] = true;
                                bitar[j + 0] = true;
                            }
                            else
                            {
                                bitar[j + 3] = false;
                                bitar[j + 2] = false;
                                bitar[j + 1] = false;
                                bitar[j + 0] = false;
                            }
                            x++;
                        }
                        rgbValues[counterdeluxe] = (byte)GetIntFromBitArray(bitar);
                        counterdeluxe++;
                    }
                }

                for (int i = 0; i < path.Count; i++)
                {
                    long percent = 100L * (long)(i + 1) / (long)path.Count;
                    MazePoint point = path[i];
                    int xrest = point.X % 2;

                    int pos = bmpData.Stride * point.Y + point.X / 2;
                    //Console.WriteLine(pos);
                    BitArray bitar = GetBitArrayFromByte(rgbValues[pos]);

                    int xtra = 4;

                    if (xrest >= 1)
                    {
                        xtra = 0;
                    }

                    if (percent < 20)
                    {
                        bitar[xtra + 3] = true;
                        bitar[xtra + 2] = false;
                        bitar[xtra + 1] = true;
                        bitar[xtra + 0] = false;
                    }
                    else if (percent < 40)
                    {
                        bitar[xtra + 3] = false;
                        bitar[xtra + 2] = false;
                        bitar[xtra + 1] = true;
                        bitar[xtra + 0] = false;
                    }
                    else if (percent < 60)
                    {
                        bitar[xtra + 3] = false;
                        bitar[xtra + 2] = false;
                        bitar[xtra + 1] = true;
                        bitar[xtra + 0] = true;
                    }
                    else if (percent < 80)
                    {
                        bitar[xtra + 3] = false;
                        bitar[xtra + 2] = false;
                        bitar[xtra + 1] = false;
                        bitar[xtra + 0] = true;
                    }
                    else
                    {
                        bitar[xtra + 3] = true;
                        bitar[xtra + 2] = false;
                        bitar[xtra + 1] = false;
                        bitar[xtra + 0] = true;
                    }


                    rgbValues[pos] = (byte)GetIntFromBitArray(bitar);
                }

                System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

                objBmpImage.UnlockBits(bmpData);

                objBmpImage.Save(fileName, imageFormat);
            }

            //return (objBmpImage);
        }
    }
}
