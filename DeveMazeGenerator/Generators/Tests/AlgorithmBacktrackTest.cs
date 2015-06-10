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
    public class AlgorithmBacktrackTest
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

        private void GoGenerate(InnerMap map, Maze maze, Random r, Action<int, int, long, long> pixelChangedCallback)
        {
            long totSteps = (((long)maze.Width - 1L) / 2L) * (((long)maze.Height - 1L) / 2L);
            long currentStep = 0;

            int x = 1;
            int y = 1;

            Stack<MazePoint> stackje = new Stack<MazePoint>();
            stackje.Push(new MazePoint(x, y));
            map[x, y] = true;
            pixelChangedCallback.Invoke(x, y, currentStep, totSteps);

            MazePoint[] targets = new MazePoint[4];

            //form.drawPixel(x, y, brushThisUses);
            while (stackje.Count != 0)
            {

                MazePoint cur = stackje.Peek();
                x = cur.X;
                y = cur.Y;

                int targetCount = 0;
                if (x - 2 > 0 && !map[x - 2, y])
                {
                    targets[targetCount].X = x - 2;
                    targets[targetCount].Y = y;
                    targetCount++;
                }
                if (x + 2 < maze.Width - 1 && !map[x + 2, y])
                {
                    targets[targetCount].X = x + 2;
                    targets[targetCount].Y = y;
                    targetCount++;
                }
                if (y - 2 > 0 && !map[x, y - 2])
                {
                    targets[targetCount].X = x;
                    targets[targetCount].Y = y - 2;
                    targetCount++;
                }
                if (y + 2 < maze.Height - 1 && !map[x, y + 2])
                {
                    targets[targetCount].X = x;
                    targets[targetCount].Y = y + 2;
                    targetCount++;
                }

                //Thread.Sleep(1000);

                if (targetCount > 0)
                {
                    var target = targets[r.Next(targetCount)];
                    stackje.Push(target);
                    map[target.X, target.Y] = true;

                    currentStep++;

                    if (target.X < x)
                    {
                        map[x - 1, y] = true;
                        pixelChangedCallback.Invoke(x - 1, y, currentStep, totSteps);
                        //form.drawPixel(x - 1, y, brushThisUses);
                    }
                    else if (target.X > x)
                    {
                        map[x + 1, y] = true;
                        pixelChangedCallback.Invoke(x + 1, y, currentStep, totSteps);
                        //form.drawPixel(x + 1, y, brushThisUses);
                    }
                    else if (target.Y < y)
                    {
                        map[x, y - 1] = true;
                        pixelChangedCallback.Invoke(x, y - 1, currentStep, totSteps);
                        //form.drawPixel(x, y - 1, brushThisUses);
                    }
                    else if (target.Y > y)
                    {
                        map[x, y + 1] = true;
                        pixelChangedCallback.Invoke(x, y + 1, currentStep, totSteps);
                        //form.drawPixel(x, y + 1, brushThisUses);
                    }
                    pixelChangedCallback.Invoke(target.X, target.Y, currentStep, totSteps);
                    //form.drawPixel(target.X, target.Y, brushThisUses);
                }
                else
                {
                    stackje.Pop();
                }


            }
        }


    }
}
