using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator.InnerMaps
{
    class BitArreintjeFastInnerMapArray
    {
        //Internal since it's used by some other classes (because this is so awesome)
        internal int[] innerData;

        public BitArreintjeFastInnerMapArray(int height)
        {
            innerData = new int[height / 32 + 1];
        }

        public bool this[int y]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (value)
                {
                    int a = 1 << y;
                    innerData[y / 32] |= a;
                }
                else
                {
                    int a = ~(1 << y);
                    innerData[y / 32] &= a;
                }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return (innerData[y / 32] & (1 << y)) != 0;
            }
        }
    }
}
