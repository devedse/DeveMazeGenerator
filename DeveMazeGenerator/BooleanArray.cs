using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator
{
    public class BooleanArray : InnerMap
    {
        public Boolean[] innerData;

        public override int Length
        {
            get { return innerData.Length; }
        }

        public BooleanArray(int size)
        {
            innerData = new Boolean[size];
        }

        public override bool this[int thePosition]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                innerData[thePosition] = value;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return innerData[thePosition];
            }
        }
    }
}
