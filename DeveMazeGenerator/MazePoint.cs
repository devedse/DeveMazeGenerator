using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator
{
    public struct MazePoint
    {
        public int X, Y;

        public MazePoint(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public override string ToString()
        {
            return "MazePoint, X: " + X + ", Y: " + Y;
        }
    }
}
