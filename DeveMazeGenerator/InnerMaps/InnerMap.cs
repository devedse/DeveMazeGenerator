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
        //virtual can be overidden
        public virtual void Print()
        {
            StringBuilder build = new StringBuilder();
            for (int x = 0; x < this.Length; x++)
            {
                InnerMapArray curInnerMapArray = this[x];
                for (int i = 0; i < curInnerMapArray.Length / 8; i++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        Boolean b = curInnerMapArray[i * 8 + y];
                        if (b)
                        {
                            build.Append('1');
                        }
                        else
                        {
                            build.Append('0');
                        }
                    }
                    build.Append(' ');
                }
                build.AppendLine();
            }
            Console.WriteLine(build);
        }

        public int Length
        {
            get { return innerData.Length; }
        }

        public InnerMapArray[] innerData;


        /// <summary>
        /// Info about mazes:
        /// 0 = False = Wall = Black
        /// 1 = True = Empty = White
        /// </summary>

        //abstract must be overidden
        public abstract InnerMapArray this[int x] { get; }
    }
}
