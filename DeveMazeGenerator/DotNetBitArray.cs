using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator
{
    public class DotNetBitArray : InnerMap
    {
        public BitArray innerData;

        public override int Length
        {
            get { return innerData.Length; }
        }

        public DotNetBitArray(int size)
        {
            innerData = new BitArray(size);
        }

        public override bool this[int thePosition]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                innerData[thePosition] = value;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return innerData[thePosition];
            }
        }
    }
}
