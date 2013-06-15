using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator.InnerMaps
{
    public abstract class InnerMap
    {
        private int width;

        public int Width
        {
            get { return width; }
        }
        private int height;

        public int Height
        {
            get { return height; }
        }

        public InnerMap(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        //virtual can be overidden
        public virtual void Print()
        {
            StringBuilder build = new StringBuilder();
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    Boolean b = this[x, y];
                    if (b)
                    {
                        build.Append('1');
                    }
                    else
                    {
                        build.Append('0');
                    }
                }
                build.AppendLine();
            }
            Console.WriteLine(build);
        }

        /// <summary>
        /// Info about mazes:
        /// 0 = False = Wall = Black
        /// 1 = True = Empty = White
        /// </summary>

        //abstract must be overidden
        public abstract Boolean this[int x, int y] { get; set; }
    }
}
