using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DeveMazeGenerator.InnerMaps;
using System.Drawing;

namespace DeveMazeGenerator.Generators
{
    public class AlgorithmBacktrack2 : Algorithm
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
            long totSteps = (((long)maze.Width - 1L) / 2L) * (((long)maze.Height - 1L) / 2L);
            long currentStep = 1;

            int x = 1;
            int y = 1;

            //Stack<MazePoint> stackje = new Stack<MazePoint>();
            //stackje.Push(new MazePoint(x, y));
            map[x, y] = true;
            pixelChangedCallback.Invoke(x, y, currentStep, totSteps);

            MazePoint[] targets = new MazePoint[4];

            QuatroStack quatro = new QuatroStack(); //0 == top, 1 == right, 2 == bot, 3 == left

            Boolean backtracking = false;
            int prex = 0;
            int prey = 0;

            while (true)
            {
                //Console.WriteLine(quatro.Count + ", X: " + x + " Y: " + y);

                int targetCount = 0;
                if (isValid(x - 2, y, map, maze))
                {
                    targets[targetCount].X = x - 2;
                    targets[targetCount].Y = y;
                    targetCount++;
                }
                if (isValid(x + 2, y, map, maze))
                {
                    targets[targetCount].X = x + 2;
                    targets[targetCount].Y = y;
                    targetCount++;
                }
                if (isValid(x, y - 2, map, maze))
                {
                    targets[targetCount].X = x;
                    targets[targetCount].Y = y - 2;
                    targetCount++;
                }
                if (isValid(x, y + 2, map, maze))
                {
                    targets[targetCount].X = x;
                    targets[targetCount].Y = y + 2;
                    targetCount++;
                }

                //Thread.Sleep(1000);

                if (targetCount > 0)
                {

                    var target = targets[r.Next(targetCount)];

                    if (backtracking)
                    {
                        backtracking = false;

                        targetCount = 0;

                        if (map[x - 1, y]) //Wall open at the left
                        {
                            targets[targetCount].X = x - 2;
                            targets[targetCount].Y = y;
                            targetCount++;
                        }
                        if (map[x + 1, y]) //Wall open at the right
                        {
                            targets[targetCount].X = x + 2;
                            targets[targetCount].Y = y;
                            targetCount++;
                        }
                        if (map[x, y - 1]) //Wall open at the top
                        {
                            targets[targetCount].X = x;
                            targets[targetCount].Y = y - 2;
                            targetCount++;
                        }
                        if (map[x, y + 1]) //Wall open at the bottom
                        {
                            targets[targetCount].X = x;
                            targets[targetCount].Y = y + 2;
                            targetCount++;
                        }

                        if (targetCount <= 2) //If currently only 2 exist at this tile, create junction
                        {
                            for (int i = 0; i < targetCount; i++)
                            {
                                var curMazePoint = targets[i];
                                if (curMazePoint.X != prex || curMazePoint.Y != prey)
                                {
                                    if (curMazePoint.Y < y)
                                    {
                                        quatro.Push(0);
                                        //g.FillRectangle(Brushes.Green, x * 5, y * 5, 5, 5);
                                    }
                                    else if (curMazePoint.X > x)
                                    {
                                        quatro.Push(1);
                                        //g.FillRectangle(Brushes.Violet, x * 5, y * 5, 5, 5);
                                    }
                                    else if (curMazePoint.Y > y)
                                    {
                                        quatro.Push(2);
                                        //g.FillRectangle(Brushes.Blue, x * 5, y * 5, 5, 5);
                                    }
                                    else if (curMazePoint.X < x)
                                    {
                                        quatro.Push(3);
                                        //g.FillRectangle(Brushes.Brown, x * 5, y * 5, 5, 5);
                                    }

                                    break;
                                }
                            }
                        }
                    }




                    //stackje.Push(target);
                    map[target.X, target.Y] = true;

                    currentStep++;

                    if (target.X < x)
                    {
                        map[x - 1, y] = true;
                        pixelChangedCallback.Invoke(x - 1, y, currentStep, totSteps);
                    }
                    else if (target.X > x)
                    {
                        map[x + 1, y] = true;
                        pixelChangedCallback.Invoke(x + 1, y, currentStep, totSteps);
                    }
                    else if (target.Y < y)
                    {
                        map[x, y - 1] = true;
                        pixelChangedCallback.Invoke(x, y - 1, currentStep, totSteps);
                    }
                    else if (target.Y > y)
                    {
                        map[x, y + 1] = true;
                        pixelChangedCallback.Invoke(x, y + 1, currentStep, totSteps);
                    }

                    x = target.X;
                    y = target.Y;

                    prex = -1;
                    prey = -1;

                    pixelChangedCallback.Invoke(target.X, target.Y, currentStep, totSteps);
                }
                else
                {
                    backtracking = true;

                    if (map[x - 1, y]) //Wall open at the left
                    {
                        targets[targetCount].X = x - 2;
                        targets[targetCount].Y = y;
                        targetCount++;
                    }
                    if (map[x + 1, y]) //Wall open at the right
                    {
                        targets[targetCount].X = x + 2;
                        targets[targetCount].Y = y;
                        targetCount++;
                    }
                    if (map[x, y - 1]) //Wall open at the top
                    {
                        targets[targetCount].X = x;
                        targets[targetCount].Y = y - 2;
                        targetCount++;
                    }
                    if (map[x, y + 1]) //Wall open at the bottom
                    {
                        targets[targetCount].X = x;
                        targets[targetCount].Y = y + 2;
                        targetCount++;
                    }

                    if (targetCount > 2) //Junction
                    {
                        prex = x;
                        prey = y;

                        int whereToGo = quatro.Pop();
                        if (whereToGo == 0)
                        {
                            y -= 2;
                        }
                        else if (whereToGo == 1)
                        {
                            x += 2;
                        }
                        else if (whereToGo == 2)
                        {
                            y += 2;
                        }
                        else if (whereToGo == 3)
                        {
                            x -= 2;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < targetCount; i++)
                        {
                            var curMazePoint = targets[i];
                            if (curMazePoint.X != prex || curMazePoint.Y != prey)
                            {
                                prex = x;
                                prey = y;
                                x = curMazePoint.X;
                                y = curMazePoint.Y;
                                break;
                            }
                        }
                    }

                    pixelChangedCallback.Invoke(x, y, currentStep, totSteps);

                }

                if (x == 1 && y == 1)
                {
                    break;
                }
            }






        }


    }
}
