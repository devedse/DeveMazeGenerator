using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DeveMazeGenerator.InnerMaps;

namespace DeveMazeGenerator.Generators
{
    public class AlgorithmKruskal : Algorithm
    {
        /// <summary>
        /// Generate a Maze
        /// </summary>
        /// <param name="width">Width of the maze</param>
        /// <param name="height">Height of the maze</param>
        /// <param name="innerMapType">The type which is used to store a map</param>
        /// <param name="pixelChangedCallback">When a pixel is changed you can define a callback here to for example draw the maze while its being generated, add null if you don't want this. Last 2 longs are for the current step and the total steps (can be used to calculate how far the maze is done being generated)</param>
        /// <returns>A maze</returns>
        public Maze Generate(int width, int height, InnerMapType innerMapType, Action<int, int, long, long> pixelChangedCallback)
        {
            if (pixelChangedCallback == null)
            {
                pixelChangedCallback = (x, y, z, u) => { };
            }

            Maze maze = new Maze(width, height, innerMapType);
            GoGenerate(maze.InnerMap, maze, new Random(), pixelChangedCallback);
            return maze;

        }

        /// <summary>
        /// Generate a Maze
        /// </summary>
        /// <param name="width">Width of the maze</param>
        /// <param name="height">Height of the maze</param>
        /// <param name="innerMapType">The type which is used to store a map</param>
        /// <param name="seed">The seed that is used to generate a maze</param>
        /// <param name="pixelChangedCallback">When a pixel is changed you can define a callback here to for example draw the maze while its being generated, add null if you don't want this. Last 2 longs are for the current step and the total steps (can be used to calculate how far the maze is done being generated)</param>
        /// <returns>A maze</returns>
        public Maze Generate(int width, int height, InnerMapType innerMapType, int seed, Action<int, int, long, long> pixelChangedCallback)
        {
            if (pixelChangedCallback == null)
            {
                pixelChangedCallback = (x, y, z, u) => { };
            }

            Maze maze = new Maze(width, height, innerMapType);
            GoGenerate(maze.InnerMap, maze, new Random(seed), pixelChangedCallback);
            return maze;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool isValid(int x, int y, InnerMap map, Maze maze)
        {
            //Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (x > 0 && x < maze.Width - 1 && y > 0 && y < maze.Height - 1)
            {
                return !map[x, y];
            }
            return false;
        }


        private void GoGenerate(InnerMap map, Maze maze, Random r, Action<int, int, long, long> pixelChangedCallback)
        {
            long totSteps = (((long)maze.Width - 1L) / 2L) * (((long)maze.Height - 1L) / 2L) * 2;
            long currentStep = 1;


            KruskalCell[][] theMap;


            //Prepare
            theMap = new KruskalCell[map.Width][];
            for (int x = 0; x < map.Width; x++)
            {
                theMap[x] = new KruskalCell[map.Height];
                for (int y = 0; y < map.Height; y++)
                {
                    KruskalCell c = new KruskalCell(x, y);
                    theMap[x][y] = c;

                    if ((x + 1) % 2 == 0 && (y + 1) % 2 == 0 && x != map.Width - 1 && y != map.Height - 1)
                    {
                        currentStep++;
                        pixelChangedCallback(x, y, currentStep, totSteps);
                        c.kruskalTileType = KruskalTileType.Passable;
                        c.cellset.Add(c);
                    }
                    else
                    {
                        c.kruskalTileType = KruskalTileType.Solid;
                    }
                }
            }







            //Find walls and add neighbouring cells
            List<KruskalCell> walls = new List<KruskalCell>();
            for (int y = 1; y < map.Height - 2; y++)
            {
                Boolean horizontalwall = false;
                int startje = 1;
                if (y % 2 == 1)
                {
                    horizontalwall = true;
                    startje = 2;
                }
                for (int x = startje; x < map.Width - 2; x = x + 2)
                {
                    KruskalCell ccc = theMap[x][y];
                    ccc.kruskalTileType = KruskalTileType.Solid;
                    walls.Add(ccc);
                    ccc.cellset.Clear();
                    if (horizontalwall)
                    {
                        //form.pixelDraw(x, y, Brushes.Blue);
                        ccc.cellset.Add(theMap[x - 1][y]);
                        ccc.cellset.Add(theMap[x + 1][y]);
                    }
                    else
                    {
                        //form.pixelDraw(x, y, Brushes.Yellow);
                        ccc.cellset.Add(theMap[x][y - 1]);
                        ccc.cellset.Add(theMap[x][y + 1]);
                    }
                }
            }






            walls = walls.RandomPermutation();
            int cur = 0;
            foreach (KruskalCell wall in walls)
            {
                cur++;

                KruskalCell cell1 = wall.cellset[0];
                KruskalCell cell2 = wall.cellset[1];
                if (!cell1.cellset.Equals(cell2.cellset))
                {
                    //Thread.Sleep(200);
                    wall.kruskalTileType = KruskalTileType.Passable;
                    //form.drawPixel(wall.x, wall.y, brushThisUses);
                    currentStep++;
                    pixelChangedCallback(wall.x, wall.y, currentStep, totSteps);
                    List<KruskalCell> l1 = cell1.cellset;
                    List<KruskalCell> l2 = cell2.cellset;

                    if (l1.Count > l2.Count)
                    {
                        l1.AddRange(l2);
                        foreach (KruskalCell c in l2)
                        {
                            c.cellset = l1;
                        }
                    }
                    else
                    {
                        l2.AddRange(l1);
                        foreach (KruskalCell c in l1)
                        {
                            c.cellset = l2;
                        }
                    }
                }
            }





            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    int ding = (int)theMap[x][y].kruskalTileType;
                    if (ding == 0)
                    {
                        map[x, y] = false;
                    }
                    else
                    {
                        map[x, y] = true;
                    }

                }
            }
        }
    }

    public class KruskalCell
    {
        public int x;
        public int y;
        public KruskalTileType kruskalTileType;
        public List<KruskalCell> cellset = new List<KruskalCell>();


        public KruskalCell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public String xeny()
        {
            String zzz = x + "-" + y;
            return zzz;
        }
    }

    public enum KruskalTileType
    {
        Solid = 0, Passable = 1
    }
}
