using DeveMazeGenerator;
using DeveMazeGenerator.Generators;
using DeveMazeGenerator.InnerMaps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace DeveMazeGeneratorUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestHardDiskMap()
        {
            int width = 32 * 8;
            int height = 32 * 12;
            var v = new BitArrayMappedOnHardDiskInnerMap(width, height);

        }

        [TestMethod]
        public void TestInnerMaps()
        {
            Random r = new Random(5);

            int width = 32 * 8;
            int height = 32 * 12;

            List<InnerMap> maps = new List<InnerMap>();

            var subtypes = GetSubtypes(Assembly.GetAssembly(typeof(InnerMap)), typeof(InnerMap));

            Assert.AreNotEqual(0, subtypes.Count());

            foreach (var brrr in subtypes)
            {
                if (brrr != typeof(InnerMap))
                {
                    var ctors = brrr.GetConstructors();
                    var obj = ctors[0].Invoke(new object[] { width, height });
                    InnerMap curInnerMap = (InnerMap)obj;
                    maps.Add(curInnerMap);
                }
            }

            Assert.AreNotEqual(0, maps.Count());

            InnerMap refferenceMap = new BooleanInnerMap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Boolean randombool = r.Next(2) == 0;
                    refferenceMap[x, y] = randombool;
                    foreach (InnerMap curmap in maps)
                    {
                        curmap[x, y] = randombool;
                    }
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Boolean curbool = refferenceMap[x, y];
                    foreach (InnerMap curmap in maps)
                    {
                        Assert.AreEqual(curbool, curmap[x, y]);
                    }
                }
            }
        }

        [TestMethod]
        public void TestNogWat()
        {
            int bb = 5 + 5;
        }

        [TestMethod]
        public void GenerateMaze()
        {
            Maze m = new AlgorithmBacktrack().Generate(256, 512, InnerMapType.BooleanArray, null);
        }

        private static IEnumerable<Type> GetSubtypes(Assembly assembly, Type parent)
        {
            return assembly.GetTypes()
                           .Where(type => parent.IsAssignableFrom(type));
        }
    }
}