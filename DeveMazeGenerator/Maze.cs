using DeveMazeGenerator.Generators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeveMazeGenerator.InnerMaps;

namespace DeveMazeGenerator
{
    public enum InnerMapType
    {
        [Description("Uses n/8 bytes of memory, pretty fast, no multithreading")]
        BitArreintjeFast,

        [Description("Uses n bytes of memory, really fast, multithreading")]
        BooleanArray,

        [Description("Uses n/8 bytes of memory, average speed, no multithreading")]
        DotNetBitArray,

        [Description("Uses 0 bytes of memory, super slow, god knows what happens when multithreading")]
        BitArrayMappedOnHardDisk
    }

    /// <summary>
    /// Info about mazes:
    /// 0 = False = Wall = Black
    /// 1 = True = Empty = White
    /// </summary>
    public class Maze
    {
        private int width;
        public int Width
        {
            get { return width; }
        }
        private int height;
        public int Height
        {
            get { return height; }
        }

        internal InnerMap innerMap;
        public InnerMap InnerMap
        {
            get { return innerMap; }
        }

        public Maze(int width, int height, InnerMapType innerMapType)
        {
            this.width = width;
            this.height = height;

            switch (innerMapType)
            {
                case InnerMapType.BitArreintjeFast:
                    innerMap = new BitArreintjeFastInnerMap(width, height);
                    break;
                case InnerMapType.BooleanArray:
                    innerMap = new BooleanInnerMap(width, height);
                    break;
                case InnerMapType.DotNetBitArray:
                    innerMap = new DotNetBitArrayInnerMap(width, height);
                    break;
                case InnerMapType.BitArrayMappedOnHardDisk:
                    innerMap = new BitArrayMappedOnHardDiskInnerMap(width, height);
                    break;
                default:
                    break;
            }

        }

        public void SaveMazeAsBmp(String fileName)
        {
            using (Bitmap objBmpImage = new Bitmap(innerMap[0].Length - 1, innerMap.Length - 1, PixelFormat.Format1bppIndexed))
            {

                Rectangle rect = new Rectangle(0, 0, objBmpImage.Width, objBmpImage.Height);
                System.Drawing.Imaging.BitmapData bmpData =
                    objBmpImage.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    objBmpImage.PixelFormat);

                IntPtr ptr = bmpData.Scan0;

                int bytes = Math.Abs(bmpData.Stride) * objBmpImage.Height;
                byte[] rgbValues = new byte[bytes];

                System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

                for (int y = 0; y < innerMap[0].Length - 1; y++)
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
                            bitar[j] = innerMap[x][y];
                            x++;
                        }
                        rgbValues[counterdeluxe] = (byte)GetIntFromBitArray(bitar);
                        counterdeluxe++;
                    }
                }

                System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

                objBmpImage.UnlockBits(bmpData);

                objBmpImage.Save(fileName, ImageFormat.Bmp);
            }
        }

        public void SaveMazeAsBmpWithPath32bpp(String fileName, List<MazePoint> path)
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

            Bitmap objBmpImage = new Bitmap(cursize * (width - 1), cursize * (height - 1), PixelFormat.Format32bppArgb);
            Graphics objGraphics = Graphics.FromImage(objBmpImage);

            // Add the colors to the new bitmap.
            objGraphics = Graphics.FromImage(objBmpImage);

            // Set Background color
            objGraphics.Clear(Color.Black);
            objGraphics.SmoothingMode = SmoothingMode.None;
            //objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            //objGraphics.DrawString(sImageText, objFont, new SolidBrush(Color.FromArgb(102, 102, 102)), 0, 0);

            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    if (innerMap[x][y] == true)
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

            objBmpImage.Save(fileName, ImageFormat.Png);

        }

        public void SaveMazeAsBmpWithPath4bpp(String fileName, List<MazePoint> path)
        {

            using (Bitmap objBmpImage = new Bitmap(innerMap[0].Length - 1, innerMap.Length - 1, PixelFormat.Format4bppIndexed))
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

                for (int y = 0; y < innerMap[0].Length - 1; y++)
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
                            if (innerMap[x][y])
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
                    int percent = 100 * (i + 1) / path.Count;
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

                objBmpImage.Save(fileName, ImageFormat.Bmp);
            }

            //return (objBmpImage);
        }

        public List<MazeWall> GenerateListOfMazeWalls()
        {
            //for (int y = 0; y < height; y++)
            //{
            //    for (int x = 0; x < width; x++)
            //    {
            //        Console.Write(innerMap[x][y] ? "0" : "1");
            //    }
            //    Console.WriteLine();
            //}

            List<MazeWall> walls = new List<MazeWall>();
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {

                    //Horizontal
                    if (innerMap[x][y] == false)
                    {

                        Boolean done = false;
                        int xx = x;
                        while (!done)
                        {
                            if (xx >= width - 1 || innerMap[xx][y] == true)
                            {
                                AddToWallList(walls, x, y, xx, y);
                                done = true;
                            }
                            xx++;
                        }
                        x = xx - 1;
                    }

                }

            }



            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {

                    //Vertical
                    if (innerMap[x][y] == false)
                    {

                        Boolean done = false;
                        int yy = y;
                        while (!done)
                        {
                            if (yy >= height - 1 || innerMap[x][yy] == true)
                            {
                                AddToWallList(walls, x, y, x, yy);
                                done = true;
                            }
                            yy++;
                        }
                        y = yy - 1;
                    }

                }

            }


            return walls;
        }

        public static Maze LoadMazeFromWalls(List<MazeWall> walls, int width, int height)
        {
            Maze m = new Maze(width, height, InnerMapType.BitArreintjeFast);

            //-1 for stupid black pixel thing :o
            for (int y = 0; y < m.height - 1; y++)
            {
                for (int x = 0; x < m.width - 1; x++)
                {
                    m.innerMap[x][y] = true;
                }
            }

            foreach (var wall in walls)
            {
                if (wall.ystart == wall.yend)
                {
                    //Horizontal
                    for (int x = wall.xstart; x < wall.xend; x++)
                    {
                        m.innerMap[x][wall.ystart] = false;
                    }
                }
                else
                {
                    //Vertical
                    for (int y = wall.ystart; y < wall.yend; y++)
                    {
                        m.innerMap[wall.xstart][y] = false;
                    }
                }
            }

            return m;
        }

        private void AddToWallList(List<MazeWall> walls, int xstart, int ystart, int xend, int yend)
        {
            //if (xstart == xend && ystart == yend)
            //{
            //    //Length is 1, don't add
            //    return;
            //}
            if (xend - xstart <= 1 && yend - ystart <= 1)
            {
                return;
            }

            MazeWall wall = new MazeWall(xstart, ystart, xend, yend);
            walls.Add(wall);

            //Console.WriteLine("New wall found: " + xstart + ":" + ystart + "  " + xend + ":" + yend);
        }

        private int GetIntFromBitArray(BitArray bitArray)
        {
            int[] array = new int[1];
            bitArray.CopyTo(array, 0);
            return array[0];
        }

        private BitArray GetBitArrayFromByte(byte byteje)
        {
            BitArray b = new BitArray(new byte[1] { byteje });
            return b;
        }
    }
}
