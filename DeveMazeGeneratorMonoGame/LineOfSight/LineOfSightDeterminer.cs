using DeveMazeGenerator;
using DeveMazeGenerator.Generators;
using DeveMazeGenerator.InnerMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGeneratorMonoGame.LineOfSight
{
    public class LineOfSightDeterminer
    {
        private InnerMap innerMap;
        private List<MazePoint> path;
        private int current = 0;

        public LineOfSightDeterminer(InnerMap innerMap, List<MazePoint> path)
        {
            this.innerMap = innerMap;
            this.path = path;
        }

        public LineOfSightObject GetNextLosObject()
        {
            while (current < path.Count)
            {
                List<MazePoint> adjacentPoints = new List<MazePoint>();
                var curMazePoint = path[current];

                int x = curMazePoint.X;
                int y = curMazePoint.Y;

                if (x - 2 > 0 && innerMap[x - 1, y] && innerMap[x - 2, y] && !path.Any(t => t.X == x - 2 && t.Y == y))
                {
                    adjacentPoints.Add(new MazePoint(x - 2, y));
                }
                else if (x + 2 < innerMap.Height && innerMap[x + 1, y] && innerMap[x + 2, y] && !path.Any(t => t.X == x + 2 && t.Y == y))
                {
                    adjacentPoints.Add(new MazePoint(x + 2, y));
                }
                else if (y - 2 > 0 && innerMap[x, y - 1] && innerMap[x, y - 2] && !path.Any(t => t.X == x && t.Y == y - 2))
                {
                    adjacentPoints.Add(new MazePoint(x, y - 2));
                }
                else if (y + 2 < innerMap.Height && innerMap[x, y + 1] && innerMap[x, y + 2] && !path.Any(t => t.X == x && t.Y == y + 2))
                {
                    adjacentPoints.Add(new MazePoint(x, y + 2));
                }

                if (adjacentPoints.Any())
                {
                    LineOfSightObject losobject = new LineOfSightObject() { CameraPoint = adjacentPoints.First() };
                    return losobject;
                }

                current += 2;
            }
            return null;
        }

        public List<MazePoint> GetAdjacentPoints(MazePoint curMazePoint)
        {
            //int xstart = Math.Max(curMazePoint.X - 2, 0);
            //int ystart = Math.Max(curMazePoint.Y - 2, 0);
            //int xend = Math.Min(curMazePoint.X + 2, innerMap.Width);
            //int yend = Math.Min(curMazePoint.Y + 2, innerMap.Height);


            int xstart = 0;
            int ystart = 0;
            int xend = innerMap.Width;
            int yend = innerMap.Height;

            List<MazePoint> probableMazePoints = new List<MazePoint>();

            for (int x = xstart; x < xend; x++)
            {
                for (int y = ystart; y < yend; y++)
                {
                    var probablePoint = new MazePoint(x, y);

                    //Not valid because its the same point
                    //if (AreEqual(curMazePoint, probablePoint))
                    //    break;

                    //Not valid because its on the path
                    if (path.Any(t => (t.X == probablePoint.X && t.Y == probablePoint.Y)))
                        continue;

                    if (HasLosSmart(probablePoint, curMazePoint))
                    {
                        probableMazePoints.Add(probablePoint);
                    }

                    //probableMazePoints.Add(probablePoint);
                }
            }

            return probableMazePoints;
        }

        private Boolean HasLosSmart(MazePoint start, MazePoint end)
        {
            if (start.X < end.X)
            {
                return HasLos(start, end);
            }
            else
            {
                return HasLos(end, start);
            }
        }

        private Boolean HasLos(MazePoint start, MazePoint end)
        {
            //return Line(start.X, start.Y, end.X, end.Y);

            //double curx = start.X + 0.5;
            //double cury = start.Y + 0.5;

            //double width = end.X - start.X;
            //double height = end.Y - start.Y;

            //double valuething = height / width;

            //while (curx < end.X + 0.5)
            //{
            //    if (innerMap[(int)curx, (int)cury] == false)
            //    {
            //        return false;
            //    }

            //    double remainingSubstraction = valuething;

            //    while (remainingSubstraction > 0)
            //    {
            //        var valueToSubstract = Math.Min(1.0, remainingSubstraction);

            //        cury -= valueToSubstract;
            //        remainingSubstraction -= valueToSubstract;
            //        if (innerMap[(int)curx, (int)cury] == false)
            //        {
            //            return false;
            //        }
            //    }
            //    curx += 1;
            //}

            double width = end.X - start.X;
            double height = end.Y - start.Y;
            //double schuin = Math.Sqrt(width * width + height * height);

            double hoek = Math.Atan(height / width);

            //double hoek = Math.Sin(height / schuin);

            List<MazePoint> reeksje = new List<MazePoint>();

            for (int x = 0; x <= width; x++)
            {

                double startHeight = x * Math.Tan(hoek);
                double endHeight = (x + 1) * Math.Tan(hoek);



                if (startHeight < endHeight)
                {
                    if (endHeight > height)
                        endHeight = height;

                    for (int y = (int)startHeight; y <= (int)endHeight; y++)
                    {
                        reeksje.Add(new MazePoint(x + start.X, y + start.Y));
                        //if (innerMap[x + start.X, y + start.Y] == false)
                        //{
                        //    return false;
                        //}
                    }
                }
                else
                {
                    if (endHeight < height)
                        endHeight = height;

                    for (int y = (int)endHeight; y <= (int)startHeight; y++)
                    {
                        reeksje.Add(new MazePoint(x + start.X, y + start.Y));
                        //if (innerMap[x + start.X, y + start.Y] == false)
                        //{
                        //    return false;
                        //}
                    }
                }
            }

            if (reeksje.Any(x => innerMap[x.X, x.Y] == false))
            {
                return false;
            }

            return true;
        }


        private bool AreEqual(MazePoint first, MazePoint second)
        {
            return first.X == second.X && first.Y == second.Y;
        }



        private static void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }

        ///// <summary>
        ///// The plot function delegate
        ///// </summary>
        ///// <param name="x">The x co-ord being plotted</param>
        ///// <param name="y">The y co-ord being plotted</param>
        ///// <returns>True to continue, false to stop the algorithm</returns>
        //public delegate bool PlotFunction(int x, int y);

        /// <summary>
        /// Plot the line from (x0, y0) to (x1, y10
        /// </summary>
        /// <param name="x0">The start x</param>
        /// <param name="y0">The start y</param>
        /// <param name="x1">The end x</param>
        /// <param name="y1">The end y</param>
        /// <param name="plot">The plotting function (if this returns false, the algorithm stops early)</param>
        public bool Line(int x0, int y0, int x1, int y1)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep) { Swap<int>(ref x0, ref y0); Swap<int>(ref x1, ref y1); }
            if (x0 > x1) { Swap<int>(ref x0, ref x1); Swap<int>(ref y0, ref y1); }
            int dX = (x1 - x0), dY = Math.Abs(y1 - y0), err = (dX / 2), ystep = (y0 < y1 ? 1 : -1), y = y0;

            for (int x = x0; x <= x1; ++x)
            {
                if (!(steep ? innerMap[y, x] : innerMap[x, y])) return false;
                err = err - dY;
                if (err < 0) { y += ystep; err += dX; }
            }
            return true;
        }


    }
}
