using DeveMazeGenerator.Generators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DeveMazeGenerator.InnerMaps;

namespace DeveMazeGenerator.PathFinders
{
    public enum PathFinderAction
    {
        Step,
        Backtrack,
        Junction,
        RemovingJunction,
        RefoundJunction
    }

    public class PathFinderDepthFirstSmartAndSmartMemory
    {
        private static Random r = new Random();

        /// <summary>
        /// Finds the path between the start and the endpoint in a maze
        /// </summary>
        /// <param name="map">The maze.InnerMap</param>
        /// <param name="callBack">The callback that can be used to see what the pathfinder is doing (or null), the boolean true = a new path find thingy or false when it determined that path is not correct</param>
        /// <returns>The direction from begin to end</returns>
        public static QuatroStack GoFind(InnerMap map, Action<int, int, PathFinderAction> callBack)
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
        /// <returns>The direction from begin to end</returns>
        public static QuatroStack GoFind(MazePoint start, MazePoint end, InnerMap map, Action<int, int, PathFinderAction> callBack)
        {
            if (callBack == null)
            {
                callBack = (x, y, z) => { };
            }


            //Callback won't work nice with this since it will find its path from back to front
            //Swap them so we don't have to reverse at the end ;)
            MazePoint temp = start;
            start = end;
            end = temp;



            int width = map.Width;
            int height = map.Height;


            //List<MazePoint> stackje = new List<MazePoint>();
            //stackje.Add(start);
            QuatroStack quatro = new QuatroStack(); //0 == top, 1 == right, 2 == bot, 3 == left

            MazePoint cur = start;
            MazePoint prev = new MazePoint(-1, -1);


            Boolean backtracking = false;

            var possibleDirections = new MazePoint[4];
            int possibleDirectionsCount = 0;
            int maxTimesAtStart = -1;

            while (true)
            {

                //cur = stackje[stackje.Count - 1];
                var x = cur.X;
                var y = cur.Y;


                if (!backtracking)
                {
                    callBack(x, y, PathFinderAction.Step);
                }
                else
                {
                    callBack(x, y, PathFinderAction.Backtrack);
                }

                possibleDirectionsCount = 0;
                if ((prev.X != x - 1 || prev.Y != y) && isValid(x - 1, y, map, width, height))
                {
                    possibleDirections[possibleDirectionsCount].X = x - 1;
                    possibleDirections[possibleDirectionsCount].Y = y;
                    possibleDirectionsCount++;
                }
                if ((prev.X != x || prev.Y != y - 1) && isValid(x, y - 1, map, width, height))
                {
                    possibleDirections[possibleDirectionsCount].X = x;
                    possibleDirections[possibleDirectionsCount].Y = y - 1;
                    possibleDirectionsCount++;
                }
                if ((prev.X != x + 1 || prev.Y != y) && isValid(x + 1, y, map, width, height))
                {
                    possibleDirections[possibleDirectionsCount].X = x + 1;
                    possibleDirections[possibleDirectionsCount].Y = y;
                    possibleDirectionsCount++;
                }
                if ((prev.X != x || prev.Y != y + 1) && isValid(x, y + 1, map, width, height))
                {
                    possibleDirections[possibleDirectionsCount].X = x;
                    possibleDirections[possibleDirectionsCount].Y = y + 1;
                    possibleDirectionsCount++;
                }

                if (maxTimesAtStart == -1)
                {
                    //Only the first time when we are actually at start
                    maxTimesAtStart = possibleDirectionsCount;
                }
                else if (cur.X == start.X && cur.Y == start.Y)
                {
                    maxTimesAtStart--;
                    if (maxTimesAtStart == 0)
                    {
                        Console.WriteLine("No path found...");
                        break;
                    }
                }

                //If we have more then 2 directions we got a junction (only if we're not backtracking) (This is actually 3 directions but we only count 2 because we don't count previous direction)
                //If we are however at the start (which is actually the end because we swap them around) we don't create a direction because you're at the end and don't need directions
                //If we are however at the end (which is the start) we will create a direction if we have more then 2 directions (This is 2 because we don't have a previous one yet)
                if ((possibleDirectionsCount >= 2 && !backtracking && (x != start.X || y != start.Y)) || (possibleDirectionsCount >= 1 && x == end.X && y == end.Y))
                {
                    //Create junction
                    callBack(x, y, PathFinderAction.Junction);

                    int directionWeCameFrom = -1;
                    if (prev.X > cur.X)
                    {
                        directionWeCameFrom = 1; //Previous x was bigger so we came from the right
                    }
                    else if (prev.Y > cur.Y)
                    {
                        directionWeCameFrom = 2;
                    }
                    else if (prev.X < cur.X)
                    {
                        directionWeCameFrom = 3;
                    }
                    else if (prev.Y < cur.Y)
                    {
                        directionWeCameFrom = 0;
                    }
                    quatro.Push(directionWeCameFrom);
                }

                if (x == end.X && y == end.Y)
                {
                    //path found
                    return quatro;
                }

                if (possibleDirectionsCount > 0)
                {
                    if (backtracking && cur.X == start.X && cur.Y == start.Y)
                    {
                        //This is because we don't have a junction at the start point but we want to stop backtracking anyway
                        backtracking = false;
                    }


                    if (backtracking)
                    {
                        if (possibleDirectionsCount >= 2) //Make sure we don't start searching again in the direction we originally came from
                        {
                            callBack(x, y, PathFinderAction.RefoundJunction);

                            //Set the direction we backtracked from
                            var lastBackTrackDir = -1;
                            if (prev.X < cur.X)
                            {
                                lastBackTrackDir = 0;
                            }
                            else if (prev.Y < cur.Y)
                            {
                                lastBackTrackDir = 1;
                            }
                            else if (prev.X > cur.X)
                            {
                                lastBackTrackDir = 2;
                            }
                            else if (prev.Y > cur.Y)
                            {
                                lastBackTrackDir = 3;
                            }

                            var foundJunction = quatro.Peek();

                            int previousDirectionX = 0;
                            int previousDirectionY = 0;
                            switch (foundJunction)
                            {
                                case 0:
                                    previousDirectionY = -1;
                                    break;
                                case 1:
                                    previousDirectionX = 1;
                                    break;
                                case 2:
                                    previousDirectionY = 1;
                                    break;
                                case 3:
                                    previousDirectionX = -1;
                                    break;
                            }

                            Boolean foundSomething = false;
                            for (int i = 0; i < possibleDirectionsCount; i++)
                            {
                                var probDir = possibleDirections[i];
                                if (probDir.X != x + previousDirectionX || probDir.Y != y + previousDirectionY)
                                {
                                    var directionOfThisDir = -1;
                                    if (probDir.X < cur.X)
                                    {
                                        directionOfThisDir = 0;
                                    }
                                    else if (probDir.Y < cur.Y)
                                    {
                                        directionOfThisDir = 1;
                                    }
                                    else if (probDir.X > cur.X)
                                    {
                                        directionOfThisDir = 2;
                                    }
                                    else if (probDir.Y > cur.Y)
                                    {
                                        directionOfThisDir = 3;
                                    }

                                    if (directionOfThisDir > lastBackTrackDir)
                                    {
                                        prev = cur;
                                        cur = probDir;
                                        foundSomething = true;
                                        backtracking = false;
                                        break;
                                    }
                                }
                            }

                            if (!foundSomething)
                            {
                                callBack(x, y, PathFinderAction.RemovingJunction);
                                quatro.Pop();

                                prev = cur;
                                cur.X += previousDirectionX;
                                cur.Y += previousDirectionY;
                            }
                        }
                        else
                        {
                            prev = cur;
                            cur = possibleDirections[0];
                        }
                    }
                    else
                    {
                        prev = cur;
                        cur = possibleDirections[0];
                    }

                }
                else
                {
                    MazePoint curtemp = cur;
                    cur = prev;
                    prev = curtemp;
                    backtracking = true;
                }

            }

            return new QuatroStack();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Boolean isValid(int x, int y, InnerMap map, int width, int height)
        {
            if (x > 0 && x < width - 1 && y > 0 && y < height - 1)
            {
                return map[x, y];
            }
            else
            {
                return false;
            }
        }

        public static IEnumerable<MazePointPos> DeterminePathFromDirections(QuatroStack directions, InnerMap map)
        {
            return DeterminePathFromDirections(directions, new MazePoint(1, 1), new MazePoint(map.Width - 3, map.Height - 3), map);

        }

        public static IEnumerable<MazePointPos> DeterminePathFromDirections(QuatroStack directions, MazePoint start, MazePoint end, InnerMap map)
        {
            int currentDirectionPos = directions.Count - 1;

            var possibleDirections = new MazePointPos[4];
            int possibleDirectionsCount = 0;

            MazePointPos prev = new MazePointPos();
            MazePointPos cur = new MazePointPos(start.X, start.Y, 0);

            int width = map.Width;
            int height = map.Height;

            long current = 0;

            while (true)
            {
                byte formulathing = (byte)((double)current / (double)directions.Count * 255.0);
                cur.RelativePos = formulathing;

                yield return cur;

                if (cur.X == end.X && cur.Y == end.Y)
                {
                    //We found the path
                    break;
                }

                int x = cur.X;
                int y = cur.Y;

                possibleDirectionsCount = 0;
                if ((prev.X != x - 1 || prev.Y != y) && isValid(x - 1, y, map, width, height))
                {
                    possibleDirections[possibleDirectionsCount].X = x - 1;
                    possibleDirections[possibleDirectionsCount].Y = y;
                    possibleDirectionsCount++;
                }
                if ((prev.X != x || prev.Y != y - 1) && isValid(x, y - 1, map, width, height))
                {
                    possibleDirections[possibleDirectionsCount].X = x;
                    possibleDirections[possibleDirectionsCount].Y = y - 1;
                    possibleDirectionsCount++;
                }
                if ((prev.X != x + 1 || prev.Y != y) && isValid(x + 1, y, map, width, height))
                {
                    possibleDirections[possibleDirectionsCount].X = x + 1;
                    possibleDirections[possibleDirectionsCount].Y = y;
                    possibleDirectionsCount++;
                }
                if ((prev.X != x || prev.Y != y + 1) && isValid(x, y + 1, map, width, height))
                {
                    possibleDirections[possibleDirectionsCount].X = x;
                    possibleDirections[possibleDirectionsCount].Y = y + 1;
                    possibleDirectionsCount++;
                }

                if (possibleDirectionsCount == 1)
                {
                    prev = cur;
                    cur = possibleDirections[0];
                }
                else if (possibleDirectionsCount > 1)
                {
                    int directionToGo = directions.InnerList[currentDirectionPos];
                    currentDirectionPos--;

                    prev = cur;
                    switch (directionToGo)
                    {
                        case 0:
                            cur.Y -= 1;
                            break;
                        case 1:
                            cur.X += 1;
                            break;
                        case 2:
                            cur.Y += 1;
                            break;
                        case 3:
                            cur.X -= 1;
                            break;
                    }
                }
                current++;
            }
        }
    }
}
