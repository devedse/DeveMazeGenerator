using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DeveMazeGenerator.InnerMaps;

namespace DeveMazeGenerator.Generators.Tests
{
    public class AlgorithmBacktrackFastWithoutActionAndMazeAndFastRandomFastStackArray3 : Algorithm
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
            var map = GoGenerate(new FastRandom(), width, height);

            InnerMap innerMap = new BooleanInnerMap(width, height, map);
            var maze = new Maze(innerMap);
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
            var map = GoGenerate(new FastRandom(seed), width, height);

            InnerMap innerMap = new BooleanInnerMap(width, height, map);
            var maze = new Maze(innerMap);
            return maze;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool isValid(int x, int y, Boolean[][] map, int width, int height)
        {
            //Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (x > 0 && x < width - 1 && y > 0 && y < height - 1)
            {
                return !map[x][y];
            }
            return false;
        }


        private Boolean[,] GoGenerate(FastRandom r, int width, int height)
        {
            int x = 1;
            int y = 1;


            var map = new Boolean[width][];

            for (int i = 0; i < width; i++)
            {
                map[i] = new Boolean[height];
            }

            //The length of this array is just enough to fit the longest path possible
            int[] stackjex = new int[width * height / 4];
            int[] stackjey = new int[width * height / 4];

            int pointertje = 0;

            stackjex[pointertje] = x;
            stackjey[pointertje] = y;
            pointertje++;

            map[x][y] = true;
            //pixelChangedCallback.Invoke(x, y, currentStep, totSteps);

            MazePoint[] targets = new MazePoint[4];

            //form.drawPixel(x, y, brushThisUses);
            while (pointertje > 0)
            {
                x = stackjex[pointertje - 1];
                y = stackjey[pointertje - 1];

                int targetCount = 0;
                if (isValid(x - 2, y, map, width, height))
                {
                    targets[targetCount].X = x - 2;
                    targets[targetCount].Y = y;
                    targetCount++;
                }
                if (isValid(x + 2, y, map, width, height))
                {
                    targets[targetCount].X = x + 2;
                    targets[targetCount].Y = y;
                    targetCount++;
                }
                if (isValid(x, y - 2, map, width, height))
                {
                    targets[targetCount].X = x;
                    targets[targetCount].Y = y - 2;
                    targetCount++;
                }
                if (isValid(x, y + 2, map, width, height))
                {
                    targets[targetCount].X = x;
                    targets[targetCount].Y = y + 2;
                    targetCount++;
                }

                //Thread.Sleep(1000);

                if (targetCount > 0)
                {
                    var target = targets[r.Next(targetCount)];
                    stackjex[pointertje] = target.X;
                    stackjey[pointertje] = target.Y;
                    pointertje++;

                    map[target.X][target.Y] = true;

                    if (target.X < x)
                    {
                        map[x - 1][y] = true;
                        //pixelChangedCallback.Invoke(x - 1, y, currentStep, totSteps);
                        //form.drawPixel(x - 1, y, brushThisUses);
                    }
                    else if (target.X > x)
                    {
                        map[x + 1][y] = true;
                        //pixelChangedCallback.Invoke(x + 1, y, currentStep, totSteps);
                        //form.drawPixel(x + 1, y, brushThisUses);
                    }
                    else if (target.Y < y)
                    {
                        map[x][y - 1] = true;
                        //pixelChangedCallback.Invoke(x, y - 1, currentStep, totSteps);
                        //form.drawPixel(x, y - 1, brushThisUses);
                    }
                    else if (target.Y > y)
                    {
                        map[x][y + 1] = true;
                        //pixelChangedCallback.Invoke(x, y + 1, currentStep, totSteps);
                        //form.drawPixel(x, y + 1, brushThisUses);
                    }
                    //pixelChangedCallback.Invoke(target.X, target.Y, currentStep, totSteps);
                    //form.drawPixel(target.X, target.Y, brushThisUses);
                }
                else
                {
                    pointertje--;
                }


            }

            //return map;
            return null;
        }


    }
}
