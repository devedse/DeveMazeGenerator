using DeveMazeGenerator;
using DeveMazeGenerator.Generators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DeveMazeGeneratorConsole
{
    class Program
    {
        public Program()
        {
            Directory.CreateDirectory("mazes");

            Stopwatch w = new Stopwatch();
            w.Start();

            Algorithm alg = new AlgorithmBacktrack();
            ParallelOptions options = new ParallelOptions();
            //options.MaxDegreeOfParallelism = 8;
            Parallel.For(0, 10000, options, new Action<int>((i) =>
            {
                if (i % 50 == 0)
                {
                    Console.WriteLine(i / 10 + "%");
                }
                int size = 256;
                Maze maze = alg.Generate(size, size, i);
                var path = PathfinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(size - 3, size - 3), maze.InnerMap);
                //maze.SaveMazeAsBmpWithPath2("mazes\\" + i + ".bmp", path);
            }));

            w.Stop();
            Console.WriteLine("Done: " + w.Elapsed.TotalSeconds);
			Thread.Sleep(5000);
        }


        static void Main(string[] args)
        {
            Program p = new Program();
        }
    }
}
