using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator
{
    public class QuatroList
    {
        int[] innerCrap;

        public QuatroList()
        {
            innerCrap = new int[128];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void IncreaseSize(int v)
        {

            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            v++;
            //v = next power of 2 now;

            Array.Resize<int>(ref innerCrap, v);
        }



        public int this[int y]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                //Value = 00000010 for example

                //check if y is between 0 and 3?
                if (value < 0 || value > 3)
                    throw new ArgumentException("input has to be between 0 and 3");

                int derealone = y % 16; //The n-th bit group
                int unmasked = value << (derealone * 2); //moving value to the left, 00100000 for example
                int pos = y / 16; //Position in array

                if (pos >= innerCrap.Length)
                {
                    IncreaseSize(pos);
                }

                int mask = (3 << derealone * 2); //Mask where the bits should be set 00110000
                int negativemask = -1 ^ mask; //Invert the mask 11001111

                //Clear current bits at the position, the mask is used for this, (for example 10101010 with the mask will become 10001010):
                int pre = negativemask & innerCrap[pos];

                int after = pre | unmasked; //Add the value to the cleared thing 10001010 + 00100000 becomes 10101010

                innerCrap[pos] = after;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                //Get at pos 2 for example

                int derealone = y % 16; //the n-th bit group

                int mask = (3 << derealone * 2); //Mask where the bits should be, 00110000 for example
                int pos = y / 16; //Pos in array

                int unmasked = innerCrap[pos] & mask; //Get only the 2 bits that were found by using the mask
                int bitshiftedback = (int)((uint)unmasked >> (derealone * 2)); //Shift the bits to the right so that 00110000 becomes 00000011, then we can read it as "3" for example

                return bitshiftedback;
            }
        }

        public static int BitStringToInt(String bits)
        {
            bits = bits.Replace(" ", "");
            return Convert.ToInt32(bits, 2);
        }

        public static String IntToBitString(int x)
        {
            string brrr = Convert.ToString(x, 2);
            StringBuilder build = new StringBuilder();
            build.Append(brrr);
            while (build.Length < 32)
            {
                build.Insert(0, "0");
            }
            build.Insert(8, " ");
            build.Insert(17, " ");
            build.Insert(26, " ");
            return build.ToString();
        }
    }
}
