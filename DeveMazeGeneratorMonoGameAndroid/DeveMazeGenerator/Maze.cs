using DeveMazeGenerator.Generators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeveMazeGenerator.InnerMaps;
using System.Runtime.CompilerServices;

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
        BitArrayMappedOnHardDisk,

        [Description("Hybrid map")]
        Hybrid
    }

    /// <summary>
    /// Info about mazes:
    /// 0 = False = Wall = Black
    /// 1 = True = Empty = White
    /// </summary>
    public partial class Maze
    {
        public int Width
        {
            get { return innerMap.Width; }
        }
        public int Height
        {
            get { return innerMap.Height; }
        }

        internal InnerMap innerMap;
        public InnerMap InnerMap
        {
            get { return innerMap; }
        }

        public Maze(int width, int height, InnerMapType innerMapType)
        {
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
                case InnerMapType.Hybrid:
                    innerMap = new HybridInnerMap(width, height);
                    break;
                default:
                    break;
            }
        }

        public Maze(InnerMap customInnerMap)
        {
            innerMap = customInnerMap;
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
            for (int y = 0; y < Height - 1; y++)
            {
                for (int x = 0; x < Width - 1; x++)
                {

                    //Horizontal
                    if (innerMap[x, y] == false)
                    {

                        Boolean done = false;
                        int xx = x;
                        while (!done)
                        {
                            if (xx >= Width - 1 || innerMap[xx, y] == true)
                            {
                                AddToWallList(walls, x, y, xx - 1, y);
                                done = true;
                            }
                            xx++;
                        }
                        x = xx - 1;
                    }

                }

            }



            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Height - 1; y++)
                {

                    //Vertical
                    if (innerMap[x, y] == false)
                    {

                        Boolean done = false;
                        int yy = y;
                        while (!done)
                        {
                            if (yy >= Height - 1 || innerMap[x, yy] == true)
                            {
                                AddToWallList(walls, x, y, x, yy - 1);
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
            for (int y = 0; y < m.Height - 1; y++)
            {
                for (int x = 0; x < m.Width - 1; x++)
                {
                    m.innerMap[x, y] = true;
                }
            }

            foreach (var wall in walls)
            {
                if (wall.ystart == wall.yend)
                {
                    //Horizontal
                    for (int x = wall.xstart; x <= wall.xend; x++)
                    {
                        m.innerMap[x, wall.ystart] = false;
                    }
                }
                else
                {
                    //Vertical
                    for (int y = wall.ystart; y <= wall.yend; y++)
                    {
                        m.innerMap[wall.xstart, y] = false;
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
