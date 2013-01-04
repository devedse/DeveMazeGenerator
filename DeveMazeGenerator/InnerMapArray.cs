using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator
{
    public abstract class InnerMapArray
    {
        //abstract must be overidden
        public abstract bool this[int y] { get; set; }
        public abstract int Length { get; }
    }
}
