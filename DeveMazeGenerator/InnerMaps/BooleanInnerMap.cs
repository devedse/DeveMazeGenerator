using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator.InnerMaps
{
    class BooleanInnerMap : InnerMap
    {
        private Boolean[,] innerData;

        public BooleanInnerMap(int width, int height)
            : base(width, height)
        {
            innerData = new Boolean[width, height];
        }

        internal BooleanInnerMap(int width, int height, Boolean[,] data)
            : base(width, height)
        {
            innerData = data;
        }

        public override Boolean this[int x, int y]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return innerData[x, y];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                innerData[x, y] = value;
            }
        }
    }
}
