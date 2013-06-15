using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator.InnerMaps
{
    public class BitArreintjeFastInnerMap : InnerMap
    {
        private BitArreintjeFastInnerMapArray[] innerData;

        public BitArreintjeFastInnerMap(int width, int height)
            : base(width, height)
        {
            innerData = new BitArreintjeFastInnerMapArray[width];
            for (int i = 0; i < width; i++)
            {
                innerData[i] = new BitArreintjeFastInnerMapArray(height);
            }
        }

        public override Boolean this[int x, int y]
        {
            get
            {
                return innerData[x][y];
            }
            set
            {
                innerData[x][y] = value;
            }
        }

    }
}
