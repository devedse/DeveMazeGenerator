using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator.InnerMaps.InnerMapHelpers
{
    class CompleteHDArray
    {
        public static Random randomFileNameRandom = new Random();
        public static String tempFolder = "temp\\";

        private FileStream fileStream;
        private String innerFileName;

        public CompleteHDArray(long size)
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

        public void WriteIntArray(long pos, int[] array)
        {
            byte[] toWrite = new byte[array.Length * 4];
            Buffer.BlockCopy(array, 0, toWrite, 0, toWrite.Length);

            fileStream.Position = pos;
            fileStream.Write(toWrite, 0, toWrite.Length);
        }

        public int[] ReadIntArray(long pos, int count)
        {
            byte[] toRead = new byte[count];
            fileStream.Position = pos;
            fileStream.Read(toRead, 0, count);

            int[] intArray = new int[count / 4];
            Buffer.BlockCopy(toRead, 0, intArray, 0, count);
            return intArray;
        }

        ~CompleteHDArray()
        {
            fileStream.Close();
            File.Delete(innerFileName);
        }
    }
}
