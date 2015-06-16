using DeveMazeGenerator.InnerMaps.InnerMapHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeveMazeGenerator.InnerMaps
{
    class BitArrayMappedOnHardDiskInnerMap : InnerMap, IDisposable
    {
        private IntHDArray inthdarray;

        private long totalLength;
        private BitArrayMappedOnHardDiskInnerMapArray[] innerData;

        public BitArrayMappedOnHardDiskInnerMap(int width, int height)
            : base(width, height)
        {
            totalLength = (long)width * (long)height;
            inthdarray = new IntHDArray(totalLength / 8 + 1);

            innerData = new BitArrayMappedOnHardDiskInnerMapArray[width];
            for (int i = 0; i < width; i++)
            {
                innerData[i] = new BitArrayMappedOnHardDiskInnerMapArray(this, i, height);
            }
            //data = new int[size / 32 + 1];
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

        internal void SetRealPos(long pos, Boolean value)
        {
            if (pos > totalLength)
            {
                throw new IndexOutOfRangeException("Toooo long");
            }

            int thePositionForBitShift = (int)(pos % 32);

            if (value)
            {

                int a = 1 << thePositionForBitShift;
                inthdarray[pos / 32] |= a;
            }
            else
            {
                int a = ~(1 << thePositionForBitShift);
                inthdarray[pos / 32] &= a;
            }

            //inthdarray[pos] = value;
        }

        internal Boolean GetRealPos(long pos)
        {
            int thePositionForBitShift = (int)(pos % 32);
            return (inthdarray[pos / 32] & (1 << thePositionForBitShift)) != 0;
        }

        public void Dispose()
        {
            inthdarray.Dispose();
        }
    }

}
