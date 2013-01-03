using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator.Generators
{
    public interface Algorithm
    {
        Maze Generate(int width, int height, InnerMapType innerMapType);
        Maze Generate(int width, int height, InnerMapType innerMapType, int seed);
    }

    public struct MazePoint
    {
        public int X, Y;

        public MazePoint(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
