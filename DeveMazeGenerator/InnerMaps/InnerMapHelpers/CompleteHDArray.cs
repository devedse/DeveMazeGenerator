using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator.InnerMaps.InnerMapHelpers
{
    class CompleteHDArray : IDisposable
    {
        private FileStream fileStream;
        private String innerFileName;

        public CompleteHDArray(long size)
        {
            var curFolder = Directory.GetCurrentDirectory();
            var tempFolderHere = Path.Combine(curFolder, GlobalVars.TempFolderName);
            if (!Directory.Exists(tempFolderHere))
            {
                Directory.CreateDirectory(tempFolderHere);
            }
            innerFileName = Path.Combine(tempFolderHere, GetRandomFileName());
            fileStream = new FileStream(innerFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, 4096, FileOptions.DeleteOnClose | FileOptions.RandomAccess);
            fileStream.SetLength(size);
        }

        private String GetRandomFileName()
        {
            return Guid.NewGuid() + ".txt";
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

        public void Dispose()
        {
            fileStream.Dispose();
        }
    }
}
