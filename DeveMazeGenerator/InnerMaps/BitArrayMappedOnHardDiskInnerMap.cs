using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeveMazeGenerator.InnerMaps
{
    class BitArrayMappedOnHardDiskInnerMap : InnerMap
    {
        private IntHDArray inthdarray;

        private long totalLength;

        public BitArrayMappedOnHardDiskInnerMap(int width, int height)
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

        //public override void Print()
        //{
        //    StringBuilder build = new StringBuilder();
        //    for (int i = 0; i < length / 8; i++)
        //    {
        //        for (int y = 0; y < 8; y++)
        //        {
        //            Boolean b = this[i * 8 + y];
        //            if (b)
        //            {
        //                build.Append('1');
        //            }
        //            else
        //            {
        //                build.Append('0');
        //            }
        //        }
        //        build.Append(' ');
        //    }
        //    Console.WriteLine(build);
        //}

        public override InnerMapArray this[int thePosition]
        {
            get
            {
                return innerData[thePosition];
            }
        }

        public void SetRealPos(long pos, Boolean value)
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

        public Boolean GetRealPos(long pos)
        {
            int thePositionForBitShift = (int)(pos % 32);
            return (inthdarray[pos / 32] & (1 << thePositionForBitShift)) != 0;
        }
    }


    public class IntHDArray
    {
        public static Random randomFileNameRandom = new Random();
        public static String tempFolder = "temp\\";

        private FileStream fileStream;
        private String innerFileName;

        public IntHDArray(long size)
        {
            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }
            innerFileName = tempFolder + GetRandomFileName();
            fileStream = new FileStream(innerFileName, FileMode.Create);
            fileStream.SetLength(size);
        }

        private String GetRandomFileName()
        {
            String str = randomFileNameRandom.Next().ToString() + randomFileNameRandom.Next().ToString() + randomFileNameRandom.Next().ToString() + randomFileNameRandom.Next().ToString() + ".txt";
            return str;
        }

        public int this[long thePosition]
        {
            set
            {
                fileStream.Position = thePosition * 4;
                fileStream.Write(BitConverter.GetBytes(value), 0, 4);
            }
            get
            {
                fileStream.Position = thePosition * 4;
                byte[] readstuff = new byte[4];
                fileStream.Read(readstuff, 0, 4);
                return BitConverter.ToInt32(readstuff, 0);
            }
        }

        ~IntHDArray()
        {
            fileStream.Close();
            File.Delete(innerFileName);
        }

    }


}
