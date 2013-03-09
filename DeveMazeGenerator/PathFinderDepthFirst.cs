using DeveMazeGenerator.Generators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DeveMazeGenerator.InnerMaps;

namespace DeveMazeGenerator
{
    public class PathFinderDepthFirst
    {
        private static Random r = new Random();

        /// <summary>
        /// Finds the path between the start and the endpoint in a maze
        /// </summary>
        /// <param name="map">The maze.InnerMap</param>
        /// <param name="callBack">The callback that can be used to see what the pathfinder is doing (or null), the boolean true = a new path find thingy or false when it determined that path is not correct</param>
        /// <returns>The shortest path in a list of points</returns>
        public static List<MazePoint> GoFind(InnerMap map, Action<int, int, Boolean> callBack)
        {
            return GoFind(new MazePoint(1, 1), new MazePoint(map.Width - 3, map.Height - 3), map, callBack);
        }

        /// <summary>
        /// Finds the path between the start and the endpoint in a maze
        /// </summary>
        /// <param name="start">The start point</param>
        /// <param name="end">The end point</param>
        /// <param name="map">The maze.InnerMap</param>
        /// <param name="callBack">The callback that can be used to see what the pathfinder is doing (or null), the boolean true = a new path find thingy or false when it determined that path is not correct</param>
        /// <returns>The shortest path in a list of points</returns>
        public static List<MazePoint> GoFind(MazePoint start, MazePoint end, InnerMap map, Action<int, int, Boolean> callBack)
        {
            if (callBack == null)
            {
                callBack = (x, y, z) => { };
            }


            //Callback won't work nice with this since it will find its path from back to front
            //Swap them so we don't have to reverse at the end ;)
            //MazePoint temp = start;
            //start = end;
            //end = temp;



            int width = map.Width;
            int height = map.Height;

            InnerMap visitedMap = new BitArreintjeFastInnerMap(width, height);

            List<MazePoint> pointlist = new List<MazePoint>();





            //@todo Controleer dit
            InnerMap visited = new BitArreintjeFastInnerMap(width, height);
            for (int x = 0; x < width; x++)
            {
                //visited[x] = new BitArreintjeFast(height);
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 || y == 0 || x == width || y == height)
                    {
                        visited[x, y] = true;
                    }
                    //else
                    //{
                    //    visited[x][y] = false;
                    //}
                }
            }


            //Hier begint het gedoe
            Stack<MazePoint> stackje = new Stack<MazePoint>();
            stackje.Push(start);
            visited[start.X, start.Y] = true;
            callBack.Invoke(start.X, start.Y, true);
            //form.pixelDraw(x, y, Brushes.White);
            while (stackje.Count != 0)
            {
                MazePoint cur = stackje.Peek();
                int x = cur.X;
                int y = cur.Y;

                if (end.X == x && end.Y == y)
                {
                    callBack.Invoke(x, y, true);
                    break;
                }

                MazePoint target = new MazePoint(-1, -1);
                if (isValid(x + 1, y, map, visited, width, height))
                {
                    target = new MazePoint(x + 1, y);
                }
                else if (isValid(x, y + 1, map, visited, width, height))
                {
                    target = new MazePoint(x, y + 1);
                }
                else if (isValid(x - 1, y, map, visited, width, height))
                {
                    target = new MazePoint(x - 1, y);
                }
                else if (isValid(x, y - 1, map, visited, width, height))
                {
                    target = new MazePoint(x, y - 1);
                }
                //Thread.Sleep(1000);

                if (target.X != -1)
                {
                    callBack.Invoke(x, y, true);
                    //var target = targets[r.Next(targets.Count)];
                    stackje.Push(target);
                    visited[target.X, target.Y] = true;
                    //form.pixelDraw(target.X, target.Y, Brushes.Blue);
                    //Thread.Sleep(200);

                    //if (target.X < x)
                    //{
                    //    visited[x - 1][y] = true;
                    //    //form.pixelDraw(x - 1, y, Brushes.White);
                    //}
                    //else if (target.X > x)
                    //{
                    //    visited[x + 1][y] = true;
                    //    //form.pixelDraw(x + 1, y, Brushes.White);
                    //}
                    //else if (target.Y < y)
                    //{
                    //    visited[x][y - 1] = true;
                    //    //form.pixelDraw(x, y - 1, Brushes.White);
                    //}
                    //else if (target.Y > y)
                    //{
                    //    visited[x][y + 1] = true;
                    //    //form.pixelDraw(x, y + 1, Brushes.White);
                    //}
                }
                else
                {
                    callBack.Invoke(x, y, false);
                    stackje.Pop();
                }
            }

            pointlist.AddRange(stackje);

            pointlist.Reverse();

            return pointlist;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Boolean isValid(int x, int y, InnerMap map, InnerMap visitedmap, int width, int height)
        {
            if (x > 0 && x < width - 1 && y > 0 && y < height - 1)
            {
                if (visitedmap[x, y])
                {
                    return false;
                }
                else
                {
                    return map[x, y];
                }
            }
            else
            {
                return false;
            }
        }
    }
}
