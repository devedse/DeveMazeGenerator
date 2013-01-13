using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator.InnerMaps
{
    class DotNetBitArrayInnerMap : InnerMap
    {
        public DotNetBitArrayInnerMap(int width, int height)
            : base(width, height)
        {
            innerData = new InnerMapArray[width];
            for (int i = 0; i < width; i++)
            {
                innerData[i] = new DotNetBitArrayInnerMapArray(height);
            }
        }

        public override InnerMapArray this[int x]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return innerData[x];
            }
        }
    }
}
