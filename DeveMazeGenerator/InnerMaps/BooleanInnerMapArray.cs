using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator.InnerMaps
{
    class BooleanInnerMapArray : InnerMapArray
    {
        public Boolean[] innerData;

        public override int Length
        {
            get { return innerData.Length; }
        }

        public BooleanInnerMapArray(int height)
        {
            innerData = new Boolean[height];
        }

        public override Boolean this[int y]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                innerData[y] = value;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return innerData[y];
            }
        }
    }
}
