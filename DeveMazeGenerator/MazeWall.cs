using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator
{
    public class MazeWall
    {
        public int xstart;
        public int ystart;
        public int xend;
        public int yend;

        public MazeWall(int xstart, int ystart, int xend, int yend)
        {
            this.xstart = xstart;
            this.ystart = ystart;
            this.xend = xend;
            this.yend = yend;
        }
    }
}
