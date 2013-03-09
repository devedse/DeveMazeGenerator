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
        public BooleanInnerMap(int width, int height)
            : base(width, height)
        {
            innerData = new InnerMapArray[width];
            for (int i = 0; i < width; i++)
            {
                innerData[i] = new BooleanInnerMapArray(height);
            }
        }

        public override Boolean this[int x, int y]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return innerData[x][y];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                innerData[x][y] = value;
            }
        }
    }
}
