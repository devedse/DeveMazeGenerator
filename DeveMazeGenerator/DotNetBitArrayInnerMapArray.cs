using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator
{
    class DotNetBitArrayInnerMapArray : InnerMapArray
    {
        public BitArray innerData;

        public override int Length
        {
            get { return innerData.Length; }
        }

        public DotNetBitArrayInnerMapArray(int height)
        {
            innerData = new BitArray(height);
        }

        public override Boolean this[int y]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                innerData[y] = value;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return innerData[y];
            }
        }
    }
}
