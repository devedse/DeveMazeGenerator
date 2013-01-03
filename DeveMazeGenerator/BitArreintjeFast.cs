using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator
{
    public class BitArreintjeFast : InnerMap
    {
        public int[] innerData;
        private int length;

        public override int Length
        {
            get { return length; }
        }

        public BitArreintjeFast(int size)
        {
            this.length = size;
            innerData = new int[size / 32 + 1];
        }


        public override bool this[int thePosition]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (value)
                {
                    int a = 1 << thePosition;
                    innerData[thePosition / 32] |= a;
                }
                else
                {
                    int a = ~(1 << thePosition);
                    innerData[thePosition / 32] &= a;
                }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return (innerData[thePosition / 32] & (1 << thePosition)) != 0;
            }
        }

    }
}
