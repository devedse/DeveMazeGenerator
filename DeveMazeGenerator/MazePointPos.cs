using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator
{
    public struct MazePointPos
    {
        public int X, Y;
        public byte RelativePos;


        public MazePointPos(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
            this.RelativePos = 0;
        }

        public MazePointPos(int X, int Y, byte RelativePos)
        {
            this.X = X;
            this.Y = Y;
            this.RelativePos = RelativePos;
        }

        public override string ToString()
        {
            return "MazePoint, X: " + X + ", Y: " + Y;
        }
    }
}
