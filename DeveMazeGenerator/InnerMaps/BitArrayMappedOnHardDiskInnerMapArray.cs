using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator.InnerMaps
{
    class BitArrayMappedOnHardDiskInnerMapArray : InnerMapArray
    {
        private int length;
        private int xCoord;
        private BitArrayMappedOnHardDiskInnerMap parent;

        public override int Length
        {
            get { return length; }
        }

        public BitArrayMappedOnHardDiskInnerMapArray(BitArrayMappedOnHardDiskInnerMap parent, int xCoord, int height)
        {
            this.length = height;
            this.xCoord = xCoord;
            this.parent = parent;
        }

        public override bool this[int y]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {

                parent.SetRealPos((long)xCoord * (long)length + y, value);

            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return parent.GetRealPos((long)xCoord * (long)length + y);
            }
        }
    }
}
