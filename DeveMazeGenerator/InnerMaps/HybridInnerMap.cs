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
    class HybridInnerMap : InnerMap
    {
        public HybridInnerMapPart currentMapPart = new HybridInnerMapPart(-1, -1, -1, -1, null); //-1 because no nullchecks needed etc :)

        public int gridSize = 256;
        public int amountOfMapPartsLoadedMax = 10;
        public int currentMapCycleFactor = 0;

        public HybridInnerMapPart[] mapParts;

        public CompleteHDArray completeHDArray;

        public HybridInnerMap(int width, int height)
            : base(width, height)
        {
            mapParts = new HybridInnerMapPart[amountOfMapPartsLoadedMax];

            completeHDArray = new CompleteHDArray((width / 8) * (height / 8));

            //innerData = new InnerMapArray[width];
            //for (int i = 0; i < width; i++)
            //{
            //    //innerData[i] = new BooleanInnerMapArray(height);
            //}
        }

        public override Boolean this[int x, int y]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (!(x > currentMapPart.StartX && x < currentMapPart.EndX && y > currentMapPart.StartY && y < currentMapPart.EndY))
                {
                    LoadNewMapPart(x / gridSize, y / gridSize);
                }

                int ything = y % gridSize;
                int xthing = x % gridSize;

                //Console.WriteLine("Get: " + x + ", " + y + ": " + currentMapPart.innerMapInPart[ything * gridSize + xthing]);
                //Thread.Sleep(100);

                return currentMapPart.innerMapInPart[ything * gridSize + xthing];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                //Console.WriteLine("Set: " + x + ", " + y + ": " + value);
                //Thread.Sleep(100);
                //Thread.Sleep(1);

                if (!(x > currentMapPart.StartX && x < currentMapPart.EndX && y > currentMapPart.StartY && y < currentMapPart.EndY))
                {
                    LoadNewMapPart(x / gridSize, y / gridSize);
                }

                int ything = y % gridSize;
                int xthing = x % gridSize;

                currentMapPart.innerMapInPart[ything * gridSize + xthing] = value;
            }
        }

        public void LoadNewMapPart(int x, int y)
        {
            //Console.WriteLine("Loading mappart: " + x + ", " + y);

            long sizeinbytes = (gridSize * gridSize) / 8; //Divide by 8 because of 8





            Boolean foundMap = false;

            //Check if current cyclething contains the new map
            for (int i = 0; i < mapParts.Length; i++)
            {
                var mapToCheck = mapParts[i];

                if (mapToCheck != null && mapToCheck.StartX == x * gridSize && mapToCheck.StartY == y * gridSize && mapToCheck.EndX == (x + 1) * gridSize && mapToCheck.EndY == (y + 1) * gridSize)
                {
                    //Console.WriteLine("From memory");
                    //Thread.Sleep(2000);
                    currentMapPart = mapToCheck;
                    //currentMapCycleFactor = i;
                    foundMap = true;
                    break;
                }
            }

            if (!foundMap)
            {
                //Console.WriteLine("From HD");
                //Thread.Sleep(2000);

                //Check if there's room for more
                if (mapParts[currentMapCycleFactor] != null)
                {
                    //If not save the oldest one
                    var oldone = mapParts[currentMapCycleFactor];

                    long posToStoreAt = sizeinbytes * (long)(Width / gridSize) * (long)(oldone.StartY / gridSize) + (long)sizeinbytes * (long)(oldone.StartX / gridSize);
                    //Console.WriteLine("Storing at: " + posToStoreAt);
                    //Thread.Sleep(5000);
                    oldone.Store(posToStoreAt);
                }







                //Load a new one
                currentMapPart = new HybridInnerMapPart(x * gridSize, y * gridSize, (x + 1) * gridSize, (y + 1) * gridSize, completeHDArray);

                long posToLoadFrom = sizeinbytes * (long)(Width / gridSize) * (long)y + (long)(sizeinbytes * x);
                //Thread.Sleep(1000);
                //Console.WriteLine(posToLoadFrom);

                currentMapPart.Load(posToLoadFrom);






                //place it at the place of the old one
                mapParts[currentMapCycleFactor] = currentMapPart;

                //Turn the cyclething
                currentMapCycleFactor++;
                if (currentMapCycleFactor >= amountOfMapPartsLoadedMax)
                {
                    currentMapCycleFactor = 0;
                }
            }

        }
    }

    class HybridInnerMapPart
    {
        private int startX;
        public int StartX
        {
            get { return startX; }
            set { startX = value; }
        }
        private int startY;
        public int StartY
        {
            get { return startY; }
            set { startY = value; }
        }
        private int endX;
        public int EndX
        {
            get { return endX; }
            set { endX = value; }
        }
        private int endY;
        public int EndY
        {
            get { return endY; }
            set { endY = value; }
        }

        public BitArreintjeFastInnerMapArray innerMapInPart;

        private int width;
        private int height;

        private CompleteHDArray hdArray;

        public HybridInnerMapPart(int startX, int startY, int endX, int endY, CompleteHDArray hdArray)
        {
            this.hdArray = hdArray;

            this.startX = startX;
            this.startY = startY;
            this.endX = endX;
            this.endY = endY;

            width = endX - startX;
            height = endY - startY;

            innerMapInPart = new BitArreintjeFastInnerMapArray(width * height);
        }

        public void Store(long pos)
        {
            hdArray.WriteIntArray(pos, innerMapInPart.innerData);
        }

        public void Load(long pos)
        {
            innerMapInPart.innerData = hdArray.ReadIntArray(pos, (width * height) / 8);
        }
    }

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
