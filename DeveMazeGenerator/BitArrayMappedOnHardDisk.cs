//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DeveMazeGenerator
//{
//    public class BitArrayMappedOnHardDisk : InnerMap
//    {
//        private IntHDArray inthdarray;
//        private int length;

//        public override int Length
//        {
//            get { return length; }
//        }

//        public BitArrayMappedOnHardDisk(int size)
//        {
//            this.length = size;
//            inthdarray = new IntHDArray(size / 8 + 1);
//            //data = new int[size / 32 + 1];
//        }

//        public override void Print()
//        {
//            StringBuilder build = new StringBuilder();
//            for (int i = 0; i < length / 8; i++)
//            {
//                for (int y = 0; y < 8; y++)
//                {
//                    Boolean b = this[i * 8 + y];
//                    if (b)
//                    {
//                        build.Append('1');
//                    }
//                    else
//                    {
//                        build.Append('0');
//                    }
//                }
//                build.Append(' ');
//            }
//            Console.WriteLine(build);
//        }

//        public override bool this[int thePosition]
//        {
//            set
//            {
//                if (value)
//                {
//                    int a = 1 << thePosition;
//                    inthdarray[thePosition / 32] |= a;
//                }
//                else
//                {
//                    int a = ~(1 << thePosition);
//                    inthdarray[thePosition / 32] &= a;
//                }
//            }
//            get
//            {
//                return (inthdarray[thePosition / 32] & (1 << thePosition)) != 0;
//            }
//        }

//    }

//    public class IntHDArray
//    {
//        public static Random randomFileNameRandom = new Random();
//        public static String tempFolder = "temp\\";

//        private FileStream fileStream;
//        private String innerFileName;

//        public IntHDArray(int size)
//        {
//            if (!Directory.Exists(tempFolder))
//            {
//                Directory.CreateDirectory(tempFolder);
//            }
//            innerFileName = tempFolder + GetRandomFileName();
//            fileStream = new FileStream(innerFileName, FileMode.Create);
//        }

//        private String GetRandomFileName()
//        {
//            String str = randomFileNameRandom.Next().ToString() + randomFileNameRandom.Next().ToString() + randomFileNameRandom.Next().ToString() + randomFileNameRandom.Next().ToString() + ".txt";
//            return str;
//        }

//        public int this[int thePosition]
//        {
//            set
//            {
//                fileStream.Position = thePosition * 4;
//                fileStream.Write(BitConverter.GetBytes(value), 0, 4);
//            }
//            get
//            {
//                fileStream.Position = thePosition * 4;
//                byte[] readstuff = new byte[4];
//                fileStream.Read(readstuff, 0, 4);
//                return BitConverter.ToInt32(readstuff, 0);
//            }
//        }

//        ~IntHDArray()
//        {
//            fileStream.Close();
//            File.Delete(innerFileName);
//        }

//    }
//}
