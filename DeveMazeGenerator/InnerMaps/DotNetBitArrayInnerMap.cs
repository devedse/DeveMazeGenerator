using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator.InnerMaps
{
    class DotNetBitArrayInnerMap : InnerMap
    {
        private BitArray[] innerData;

        public DotNetBitArrayInnerMap(int width, int height)
            : base(width, height)
        {
            innerData = new BitArray[width];
            for (int i = 0; i < width; i++)
            {
                innerData[i] = new BitArray(height);
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
