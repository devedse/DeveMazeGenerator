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
    class HybridInnerMap : InnerMap, IDisposable
    {
        private HybridInnerMapPart currentMapPart = new HybridInnerMapPart(-1, -1, -1, -1, null); //-1 because no nullchecks needed etc :)

        public const int GridSize = 4096;
        private const int AmountOfMapPartsLoadedMax = 10;
        private int currentMapCycleFactor = 0;

        private HybridInnerMapPart[] mapParts;

        private CompleteHDArray completeHDArray;

        public HybridInnerMap(int width, int height)
            : base(width, height)
        {
            mapParts = new HybridInnerMapPart[AmountOfMapPartsLoadedMax];

            completeHDArray = new CompleteHDArray((width / 8L) * (height / 8L));

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
                    LoadNewMapPart(x / GridSize, y / GridSize);
                }

                int ything = y % GridSize;
                int xthing = x % GridSize;

                //Console.WriteLine("Get: " + x + ", " + y + ": " + currentMapPart.innerMapInPart[ything * gridSize + xthing]);
                //Thread.Sleep(100);

                return currentMapPart.innerMapInPart[ything * GridSize + xthing];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                //Console.WriteLine("Set: " + x + ", " + y + ": " + value);
                //Thread.Sleep(100);
                //Thread.Sleep(1);

                if (!(x > currentMapPart.StartX && x < currentMapPart.EndX && y > currentMapPart.StartY && y < currentMapPart.EndY))
                {
                    LoadNewMapPart(x / GridSize, y / GridSize);
                }

                int ything = y % GridSize;
                int xthing = x % GridSize;

                currentMapPart.innerMapInPart[ything * GridSize + xthing] = value;
            }
        }

        public void LoadNewMapPart(int x, int y)
        {
            //Console.WriteLine("Loading mappart: " + x + ", " + y);

            const long sizeinbytes = (GridSize * GridSize) / 8; //Divide by 8 because of 8





            Boolean foundMap = false;

            //Check if current cyclething contains the new map
            for (int i = 0; i < mapParts.Length; i++)
            {
                var mapToCheck = mapParts[i];

                if (mapToCheck != null && mapToCheck.StartX == x * GridSize && mapToCheck.StartY == y * GridSize && mapToCheck.EndX == (x + 1) * GridSize && mapToCheck.EndY == (y + 1) * GridSize)
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

                    long posToStoreAt = sizeinbytes * (long)(Width / GridSize) * (long)(oldone.StartY / GridSize) + (long)sizeinbytes * (long)(oldone.StartX / GridSize);
                    //Console.WriteLine("Storing at: " + posToStoreAt);
                    //Thread.Sleep(5000);
                    oldone.Store(posToStoreAt);
                }







                //Load a new one
                currentMapPart = new HybridInnerMapPart(x * GridSize, y * GridSize, (x + 1) * GridSize, (y + 1) * GridSize, completeHDArray);

                long posToLoadFrom = sizeinbytes * (long)(Width / GridSize) * (long)y + (long)(sizeinbytes * x);
                //Thread.Sleep(1000);
                //Console.WriteLine(posToLoadFrom);

                currentMapPart.Load(posToLoadFrom);






                //place it at the place of the old one
                mapParts[currentMapCycleFactor] = currentMapPart;

                //Turn the cyclething
                currentMapCycleFactor++;
                if (currentMapCycleFactor >= AmountOfMapPartsLoadedMax)
                {
                    currentMapCycleFactor = 0;
                }
            }

        }

        public void Dispose()
        {
            completeHDArray.Dispose();
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
}
