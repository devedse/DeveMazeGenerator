using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGenerator
{
    public static class GlobalVars
    {
        /// <summary>
        /// Check this :)
        /// https://msdn.microsoft.com/en-us/library/hh285054(v=vs.110).aspx
        /// </summary>
        public const int MaxArraySize = 2146435071;

        /// <summary>
        /// The temp folder name to use for storing maps on hard disk (used in for example the HybridInnerMap and BitArrayMappedOnHardDiskInnerMap)
        /// </summary>
        public const String TempFolderName = "temp";
    }
}
