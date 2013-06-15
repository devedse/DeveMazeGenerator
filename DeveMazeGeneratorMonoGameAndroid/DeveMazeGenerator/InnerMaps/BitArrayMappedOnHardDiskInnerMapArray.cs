using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator.InnerMaps
{
    class BitArrayMappedOnHardDiskInnerMapArray
    {
        private int length;
        private int xCoord;
        private BitArrayMappedOnHardDiskInnerMap parent;

        public BitArrayMappedOnHardDiskInnerMapArray(BitArrayMappedOnHardDiskInnerMap parent, int xCoord, int height)
        {
            this.length = height;
            this.xCoord = xCoord;
            this.parent = parent;
        }

        public bool this[int y]
        {
            set
            {
                parent.SetRealPos((long)xCoord * (long)length + y, value);
            }
            get
            {
                return parent.GetRealPos((long)xCoord * (long)length + y);
            }
        }
    }
}
