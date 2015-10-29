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
    public class AlgorithmDivision : Algorithm
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


            //needs to be optimized
            for (int x = 0; x < maze.Width; x++)
            {
                for (int y = 0; y < maze.Height; y++)
                {
                    if (x == 0 || x == maze.Width - 1 || x == maze.Width - 2 || y == 0 || y == maze.Height - 1 || y == maze.Height - 2)
                    {
                        map[x, y] = false;
                    }
                    else
                    {
                        map[x, y] = true;
                    }
                }
            }




            Stack<Rectangle> rectangles = new Stack<Rectangle>();
            Rectangle curRect = new Rectangle(0, 0, maze.Width - 1, maze.Height - 1);
            rectangles.Push(curRect);

            while (rectangles.Count != 0)
            {
                curRect = rectangles.Pop();

                if (curRect.Width > 3 && curRect.Height > 3)
                {

                    Boolean horizontalSplit = true;

                    if (curRect.Width > curRect.Height)
                    {
                        horizontalSplit = false;
                    }
                    else if (curRect.Width < curRect.Height)
                    {
                        horizontalSplit = true;
                    }
                    else
                    {
                        if (r.Next(2) == 0)
                        {
                            horizontalSplit = false;
                        }
                    }

                    if (horizontalSplit)
                    {
                        int splitnumber = 2 + r.Next((curRect.Height - 2) / 2) * 2;
                        int opening = 1 + r.Next((curRect.Width) / 2) * 2;

                        Rectangle rect1 = new Rectangle(curRect.X, curRect.Y, curRect.Width, splitnumber + 1);
                        Rectangle rect2 = new Rectangle(curRect.X, curRect.Y + splitnumber, curRect.Width, curRect.Height - splitnumber);

                        for (int i = curRect.X; i < curRect.X + curRect.Width; i++)
                        {
                            if (i - curRect.X != opening)
                            {
                                map[i, curRect.Y + splitnumber] = false;
                            }
                        }

                        //form.drawRectangle(curRect.X, curRect.Y + splitnumber, opening, 1, brushBlack);
                        //form.drawRectangle(curRect.X + opening + 1, curRect.Y + splitnumber, curRect.Width - opening - 1, 1, brushBlack);

                        rectangles.Push(rect1);
                        rectangles.Push(rect2);
                    }
                    else
                    {
                        int splitnumber = 2 + r.Next((curRect.Width - 2) / 2) * 2;
                        int opening = 1 + r.Next((curRect.Height) / 2) * 2;

                        Rectangle rect1 = new Rectangle(curRect.X, curRect.Y, splitnumber + 1, curRect.Height);
                        Rectangle rect2 = new Rectangle(curRect.X + splitnumber, curRect.Y, curRect.Width - splitnumber, curRect.Height);

                        for (int i = curRect.Y; i < curRect.Y + curRect.Height; i++)
                        {
                            if (i - curRect.Y != opening)
                            {
                                map[curRect.X + splitnumber, i] = false;
                            }
                        }

                        //form.drawRectangle(curRect.X + splitnumber, curRect.Y, 1, opening, brushBlack);
                        //form.drawRectangle(curRect.X + splitnumber, curRect.Y + opening + 1, 1, curRect.Height - opening - 1, brushBlack);

                        rectangles.Push(rect1);
                        rectangles.Push(rect2);
                    }
                }
            }
        }


    }
}
