using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator.InnerMaps.InnerMapHelpers
{
    class IntHDArray
    {
        private static String tempFolder = "temp\\";

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
            return Guid.NewGuid() + ".txt";
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
