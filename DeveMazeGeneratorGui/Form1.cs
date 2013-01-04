using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeveMazeGenerator;
using DeveMazeGenerator.Generators;
using System.IO;

namespace DeveMazeGeneratorGui
{
    public partial class Form1 : Form
    {
        private Boolean forceesStoppenEnzo = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Algorithm curalg = new AlgorithmBacktrack();
            Stopwatch w = new Stopwatch();
            w.Start();
            int size = (int)Math.Pow(2.0, 10.0);
            Console.WriteLine("Generating maze of size: " + size);
            Console.WriteLine("Saved size it should be: " + Math.Pow((double)size, 2.0) / 1024.0 / 1024.0 / 8.0 + " mb");
            Maze maze = curalg.Generate(size, size, InnerMapType.BitArreintjeFast, 1337);
            w.Stop();
            Console.WriteLine("Generating time: " + w.Elapsed.TotalSeconds);
            Console.WriteLine("Finding path...");
            w.Reset();
            w.Start();
            var path = PathFinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(size - 3, size - 3), maze.InnerMap);
            w.Stop();
            Console.WriteLine("Time it took to find path: " + w.Elapsed.TotalSeconds);

            Console.WriteLine("Saving...");
            w.Reset();
            w.Start();

            maze.SaveMazeAsBmpWithPath4bpp("zzzzzzzzzzz2.bmp", path);
            Console.WriteLine("Done saving, saving time: " + w.Elapsed.TotalSeconds);
            Console.WriteLine("Location: " + System.IO.Directory.GetCurrentDirectory());

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stopwatch w = new Stopwatch();
            w.Start();
            for (int i = 0; i < 100000000; i++)
            {
                int aaa = test(i, i);
            }
            w.Stop();
            Console.WriteLine("Time: " + w.Elapsed.TotalSeconds);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public int test(int x, int y)
        {
            //Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            return x + y;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
                {
                    Directory.CreateDirectory("bigmazes");

                    Stopwatch w = new Stopwatch();
                    w.Start();

                    Algorithm alg = new AlgorithmBacktrack();
                    ParallelOptions options = new ParallelOptions();
                    //options.MaxDegreeOfParallelism = 8;
                    Parallel.For(0, 1000, options, new Action<int>((i) =>
                    {
                        if (i % 50 == 0)
                        {
                            DebugMSG(i / 10 + "%");
                        }
                        int size = 1024;
                        Maze maze = alg.Generate(size, size, InnerMapType.BitArreintjeFast, i);
                        var path = PathFinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(size - 3, size - 3), maze.InnerMap);
                        maze.SaveMazeAsBmpWithPath4bpp("bigmazes\\" + i + ".bmp", path);
                    }));

                    w.Stop();
                    DebugMSG("Done: " + w.Elapsed.TotalSeconds);
                });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Directory.CreateDirectory("mazes");

                Stopwatch w = new Stopwatch();
                w.Start();

                Algorithm alg = new AlgorithmBacktrack();
                ParallelOptions options = new ParallelOptions();
                //options.MaxDegreeOfParallelism = 8;
                for (int i = 0; i < 1000; i++)
                {
                    if (i % 50 == 0)
                    {
                        DebugMSG(i / 10 + "%");
                    }
                    int size = 256;
                    Maze maze = alg.Generate(size, size, InnerMapType.BitArreintjeFast, i);
                    var path = PathFinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(size - 3, size - 3), maze.InnerMap);
                    maze.SaveMazeAsBmpWithPath4bpp("mazes\\" + i + ".bmp", path);
                };

                w.Stop();
                DebugMSG("Done: " + w.Elapsed.TotalSeconds);
            });
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Task t = new Task(new Action(() =>
            {
                Algorithm curalg = new AlgorithmBacktrack();
                Stopwatch w = new Stopwatch();
                w.Start();
                int size = (int)Math.Pow(2.0, 17.0);
                DebugMSG("Generating maze of size: " + size);
                DebugMSG("Saved size it should be: " + Math.Pow((double)size, 2.0) / 1024.0 / 1024.0 / 8.0 + " mb");
                Maze maze = curalg.Generate(size, size, InnerMapType.BitArreintjeFast, 1337);
                w.Stop();
                DebugMSG("Generating time: " + w.Elapsed.TotalSeconds);
                //Console.WriteLine("Finding path...");
                //w.Reset();
                //w.Start();
                //var path = PathfinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(size - 3, size - 3), maze.InnerMap);
                //w.Stop();
                //Console.WriteLine("Time it took to find path: " + w.Elapsed.TotalSeconds);

                GC.Collect();

                DebugMSG("Saving...");
                w.Reset();
                w.Start();

                maze.SaveMazeAsBmp("bigmazeetc.bmp");
                DebugMSG("Done saving, saving time: " + w.Elapsed.TotalSeconds);
                DebugMSG("Location: " + System.IO.Directory.GetCurrentDirectory());
                maze = null;
                GC.Collect();
            }));

            t.Start();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Stopwatch w = new Stopwatch();

            ParallelOptions options = new ParallelOptions();
            //options.MaxDegreeOfParallelism = 8;
            Algorithm alg;

            w.Start();
            alg = new AlgorithmBacktrack();
            Parallel.For(0, 1000, options, new Action<int>((i) =>
            {
                //if (i % 50 == 0)
                //{
                //    Console.WriteLine(i / 10 + "%");
                //}
                int size = 256;
                Maze maze = alg.Generate(size, size, InnerMapType.BitArreintjeFast, i);
                //var path = PathFinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(size - 3, size - 3), maze.InnerMap);
                //maze.SaveMazeAsBmpWithPath2("mazes\\" + i + ".bmp", path);
            }));
            w.Stop();
            Console.WriteLine("Time aggressive: " + w.Elapsed.TotalSeconds);
            w.Reset();

            w.Start();
            alg = new AlgorithmBacktrackNoInlining();
            Parallel.For(0, 1000, options, new Action<int>((i) =>
            {
                //if (i % 50 == 0)
                //{
                //    Console.WriteLine(i / 10 + "%");
                //}
                int size = 256;
                Maze maze = alg.Generate(size, size, InnerMapType.BitArreintjeFast, i);
                //var path = PathFinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(size - 3, size - 3), maze.InnerMap);
                //maze.SaveMazeAsBmpWithPath2("mazes\\" + i + ".bmp", path);
            }));
            w.Stop();
            Console.WriteLine("Time noinlining: " + w.Elapsed.TotalSeconds);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Stopwatch w = new Stopwatch();

            ParallelOptions options = new ParallelOptions();
            //options.MaxDegreeOfParallelism = 8;
            Algorithm alg;

            for (int y = 0; y < 100; y++)
            {
                Console.WriteLine("----------------------");
                w.Start();
                alg = new AlgorithmBacktrack();
                for (int i = 0; i < 1000; i++)
                {
                    //if (i % 50 == 0)
                    //{
                    //    Console.WriteLine(i / 10 + "%");
                    //}
                    int size = 256;
                    Maze maze = alg.Generate(size, size, InnerMapType.BitArreintjeFast, i);
                    //var path = PathFinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(size - 3, size - 3), maze.InnerMap);
                    //maze.SaveMazeAsBmpWithPath2("mazes\\" + i + ".bmp", path);
                }
                w.Stop();
                DebugMSG("Time aggressive: " + w.Elapsed.TotalSeconds);
                w.Reset();

                w.Start();
                alg = new AlgorithmBacktrackNoInlining();
                for (int i = 0; i < 1000; i++)
                {
                    //if (i % 50 == 0)
                    //{
                    //    Console.WriteLine(i / 10 + "%");
                    //}
                    int size = 256;
                    Maze maze = alg.Generate(size, size, InnerMapType.BitArreintjeFast, i);
                    //var path = PathFinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(size - 3, size - 3), maze.InnerMap);
                    //maze.SaveMazeAsBmpWithPath2("mazes\\" + i + ".bmp", path);
                }
                w.Stop();

                DebugMSG("Time noinlining: " + w.Elapsed.TotalSeconds);
                w.Reset();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Stopwatch w = new Stopwatch();
            int loops = 10000;

            for (int y = 0; y < 1; y++)
            {
                DebugMSG("----------------------");
                AlgorithmBacktrack alg = new AlgorithmBacktrack();
                w.Start();
                for (int i = 0; i < loops; i++)
                {
                    //if (i % 50 == 0)
                    //{
                    //    Console.WriteLine(i / 10 + "%");
                    //}
                    int size = 256;
                    Maze maze = alg.Generate(size, size, InnerMapType.BitArreintjeFast, i);
                    //var path = PathFinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(size - 3, size - 3), maze.InnerMap);
                    //maze.SaveMazeAsBmpWithPath2("mazes\\" + i + ".bmp", path);
                }
                w.Stop();
                DebugMSG("Time AlgorithmBacktrack: " + w.Elapsed.TotalSeconds);
                w.Reset();

                AlgorithmBacktrackWithCallback alg2 = new AlgorithmBacktrackWithCallback();
                w.Start();
                for (int i = 0; i < loops; i++)
                {
                    //if (i % 50 == 0)
                    //{
                    //    Console.WriteLine(i / 10 + "%");
                    //}
                    int size = 256;
                    Maze maze = alg2.SpecialGenerate(size, size, InnerMapType.BitArreintjeFast, i, (xx, yy) => { ChangeEenPixel(xx, yy); });
                    //var path = PathFinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(size - 3, size - 3), maze.InnerMap);
                    //maze.SaveMazeAsBmpWithPath2("mazes\\" + i + ".bmp", path);
                }
                w.Stop();

                DebugMSG("Time AlgorithmBacktrackWithCallback: " + w.Elapsed.TotalSeconds);
                w.Reset();

                AlgorithmBacktrackWithCallback alg3 = new AlgorithmBacktrackWithCallback();
                w.Start();
                for (int i = 0; i < loops; i++)
                {
                    //if (i % 50 == 0)
                    //{
                    //    Console.WriteLine(i / 10 + "%");
                    //}
                    int size = 256;
                    Maze maze = alg3.SpecialGenerate(size, size, InnerMapType.BitArreintjeFast, i, null);
                    //var path = PathFinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(size - 3, size - 3), maze.InnerMap);
                    //maze.SaveMazeAsBmpWithPath2("mazes\\" + i + ".bmp", path);
                }
                w.Stop();

                DebugMSG("Time AlgorithmBacktrackWithCallback met nullcallback: " + w.Elapsed.TotalSeconds);
                w.Reset();
            }
        }

        public void ChangeEenPixel(int x, int y)
        {
            int b = x + y;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Stopwatch w = new Stopwatch();

            for (int y = 0; y < 100; y++)
            {
                Console.WriteLine("----------------------");
                AlgorithmBacktrack alg = new AlgorithmBacktrack();
                w.Start();
                for (int i = 0; i < 10000; i++)
                {
                    //if (i % 50 == 0)
                    //{
                    //    Console.WriteLine(i / 10 + "%");
                    //}
                    int size = 256;
                    Maze maze = alg.Generate(size, size, InnerMapType.BitArreintjeFast, i);
                    //var path = PathFinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(size - 3, size - 3), maze.InnerMap);
                    //maze.SaveMazeAsBmpWithPath2("mazes\\" + i + ".bmp", path);
                }
                w.Stop();
                Console.WriteLine("Time AlgorithmBacktrack1: " + w.Elapsed.TotalSeconds);
                w.Reset();

                AlgorithmBacktrack alg2 = new AlgorithmBacktrack();
                w.Start();
                for (int i = 0; i < 10000; i++)
                {
                    //if (i % 50 == 0)
                    //{
                    //    Console.WriteLine(i / 10 + "%");
                    //}
                    int size = 256;
                    Maze maze = alg2.Generate(size, size, InnerMapType.BitArreintjeFast, i);
                    //var path = PathFinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(size - 3, size - 3), maze.InnerMap);
                    //maze.SaveMazeAsBmpWithPath2("mazes\\" + i + ".bmp", path);
                }
                w.Stop();

                Console.WriteLine("Time AlgorithmBacktrack2: " + w.Elapsed.TotalSeconds);
                w.Reset();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Stopwatch w = new Stopwatch();
            AlgorithmBacktrack algje = new AlgorithmBacktrack();
            w.Start();
            algje.Generate(20000, 20000, InnerMapType.BitArreintjeFast);
            w.Stop();
            DebugMSG("Seconds it took: " + w.Elapsed.TotalSeconds);

        }


        public void DebugMSG(String str)
        {
            listBox1.Invoke(new Action(() =>
            {
                str = DateTime.Now.ToLongTimeString() + ":    " + str;
                listBox1.Items.Insert(0, str);
            }));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AlgorithmBacktrack back = new AlgorithmBacktrack();

            int width = 16;
            int height = 16;
            Maze maze = back.Generate(width, height, InnerMapType.BitArreintjeFast);
            var path = PathFinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(width - 3, height - 3), maze.InnerMap);
            maze.SaveMazeAsBmpWithPath4bpp("mazePath.bmp", path);
            maze.SaveMazeAsBmp("maze1.bmp");

            //foreach (var v in maze.InnerMap)
            //{
            //    v.Print();
            //}

            var walls = maze.GenerateListOfMazeWalls();

            foreach (var wall in walls)
            {
                Console.WriteLine("New wall found: " + wall.xstart + ":" + wall.ystart + "  " + wall.xend + ":" + wall.yend);
            }

            Maze loadedfromwall = Maze.LoadMazeFromWalls(walls, width, height);
            loadedfromwall.SaveMazeAsBmp("maze2.bmp");

            DebugMSG("Ok done");
        }



        private void GenerateMazeWithThisTypeAndShowTime(InnerMapType type)
        {
            Stopwatch w = new Stopwatch();
            w.Start();

            AlgorithmBacktrack back = new AlgorithmBacktrack();
            int size = (int)Math.Pow(2, 11);
            DebugMSG("Generating maze of type: " + type + " of size: " + size);
            Maze maze = back.Generate(size, size, type);
            var path = PathFinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(size - 3, size - 3), maze.InnerMap);
            maze.SaveMazeAsBmpWithPath4bpp("maze.bmp", path);
            DebugMSG("Ok done, time: " + w.Elapsed.TotalSeconds);
            DebugMSG("");

        }

        private void button12_Click(object sender, EventArgs e)
        {
            Task t = Task.Run(new Action(() =>
            {
                try
                {
                    DebugMSG("---------------");
                    GenerateMazeWithThisTypeAndShowTime(InnerMapType.BitArrayMappedOnHardDisk);
                    GenerateMazeWithThisTypeAndShowTime(InnerMapType.BitArreintjeFast);
                    GenerateMazeWithThisTypeAndShowTime(InnerMapType.BooleanArray);
                    GenerateMazeWithThisTypeAndShowTime(InnerMapType.DotNetBitArray);
                }
                catch (Exception eee)
                {
                    DebugMSG(eee.ToString());
                }
            }));
        }

        //Vind langste path
        private void button13_Click(object sender, EventArgs e)
        {
            forceesStoppenEnzo = false;
            int hoeveelHijErDoetPerKeer = 10000000;
            int size = 1024 * 8;

            Task.Run(() =>
            {
                object lockertje = new object();
                int curLongest = 0;
                Maze curLongestMaze = null;
                List<MazePoint> pathshortest = null;

                Directory.CreateDirectory("longestpath");

                foreach (String str in Directory.GetFiles("longestpath"))
                {
                    String filename = Path.GetFileNameWithoutExtension(str);
                    int length = int.Parse(filename);
                    if (length > curLongest)
                    {
                        curLongest = length;
                    }
                }

                DebugMSG("Current longest maze: " + curLongest);

                Random r = new Random();
                int starthere = r.Next(int.MaxValue - hoeveelHijErDoetPerKeer);

                Algorithm alg = new AlgorithmBacktrack();

                Parallel.For(starthere, starthere + hoeveelHijErDoetPerKeer, new Action<int>((i) =>
                {
                    if (forceesStoppenEnzo)
                    {

                    }
                    else
                    {
                        Maze m = alg.Generate(size, size, InnerMapType.BitArreintjeFast, i);
                        var path = PathFinderDepthFirst.GoFind(m.InnerMap);

                        DebugMSG("Weer een maze done, path length: " + path.Count);

                        if (path.Count > curLongest)
                        {
                            lock (lockertje)
                            {
                                if (path.Count > curLongest)
                                {
                                    DebugMSG("New longest maze found, length: " + path.Count);
                                    curLongest = path.Count;
                                    curLongestMaze = m;
                                    pathshortest = path;
                                    //m.SaveMazeAsBmpWithPath2("longestpath\\" + path.Count.ToString() + ".bmp", path);
                                }
                            }

                        }
                    }
                }));



                DebugMSG("Done met dit setje :)");





            });
        }

        //Vind korste path
        private void button14_Click(object sender, EventArgs e)
        {
            forceesStoppenEnzo = false;
            int hoeveelHijErDoetPerKeer = 10000000;
            int size = 1024;

            Task.Run(() =>
            {
                object lockertje = new object();
                int curLongest = int.MaxValue;
                Maze curLongestMaze = null;
                List<MazePoint> pathshortest = null;

                Directory.CreateDirectory("shortestpath");

                foreach (String str in Directory.GetFiles("shortestpath"))
                {
                    String filename = Path.GetFileNameWithoutExtension(str);
                    int length = int.Parse(filename);
                    if (length < curLongest)
                    {
                        curLongest = length;
                    }
                }

                DebugMSG("Current shortest maze: " + curLongest);

                Random r = new Random();
                int starthere = r.Next(int.MaxValue - hoeveelHijErDoetPerKeer);

                Algorithm alg = new AlgorithmBacktrack();

                Parallel.For(starthere, starthere + hoeveelHijErDoetPerKeer, new Action<int>((i) =>
                {
                    if (forceesStoppenEnzo)
                    {

                    }
                    else
                    {
                        Maze m = alg.Generate(size, size, InnerMapType.BitArreintjeFast, i);
                        var path = PathFinderDepthFirst.GoFind(m.InnerMap);

                        if (path.Count < curLongest)
                        {
                            lock (lockertje)
                            {
                                if (path.Count < curLongest)
                                {
                                    DebugMSG("New shortest maze found, length: " + path.Count);
                                    curLongest = path.Count;
                                    curLongestMaze = m;
                                    pathshortest = path;
                                    m.SaveMazeAsBmpWithPath4bpp("shortestpath\\" + path.Count.ToString() + ".bmp", path);
                                }
                            }
                        }
                    }
                }));



                DebugMSG("Done met dit setje :)");





            });
        }

        private void button15_Click(object sender, EventArgs e)
        {
            GC.Collect();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            forceesStoppenEnzo = true;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                AlgorithmBacktrack back = new AlgorithmBacktrack();

                int width = 1024 * 1;
                int height = 1024 * 1;

                DebugMSG("Generating maze of size: " + width + " * " + height);
                Maze maze = back.Generate(width, height, InnerMapType.BitArrayMappedOnHardDisk);

                DebugMSG("Generating done :)");


                maze.SaveMazeAsBmp("maze1.bmp");
                DebugMSG("Maze saved");


                //foreach (var v in maze.InnerMap)
                //{
                //    v.Print();
                //}

                DebugMSG("Finding walls...");
                var walls = maze.GenerateListOfMazeWalls();
                DebugMSG("Walls found");

                DebugMSG("Creating new Maze from walls...");

                Maze loadedfromwall = Maze.LoadMazeFromWalls(walls, width, height);

                DebugMSG("Created :), saving this maze...");
                loadedfromwall.SaveMazeAsBmp("maze2.bmp");

                DebugMSG("Done saving, creating path for this maze...");

                var path = PathFinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(width - 3, height - 3), loadedfromwall.InnerMap);

                DebugMSG("Pathfinding done, saving this maze + path...");

                loadedfromwall.SaveMazeAsBmpWithPath4bpp("mazePath.bmp", path);

                DebugMSG("Saving done :)");

                DebugMSG("Generating walls from this thing again...");

                var walls2 = loadedfromwall.GenerateListOfMazeWalls();

                DebugMSG("Done with walls2");

                DebugMSG("Creating and saving new maze for this...");
                Maze mmmmmm = Maze.LoadMazeFromWalls(walls2, width, height);
                mmmmmm.SaveMazeAsBmp("maze3.bmp");

                DebugMSG("Ok done :), comparing maze 1 and 3...");

                Boolean thesame = true;

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (maze.InnerMap[x][y] != mmmmmm.InnerMap[x][y])
                        {
                            thesame = false;
                        }
                    }
                }

                DebugMSG("Done checking, mazes are the same: " + thesame);

                DebugMSG("Ok done :D");
            });
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                AlgorithmBacktrack back = new AlgorithmBacktrack();

                int width = 1024 * 8;
                int height = 1024 * 8;

                DebugMSG("Generating maze of size: " + width + " * " + height);
                Maze maze = back.Generate(width, height, InnerMapType.BitArreintjeFast);

                DebugMSG("Generating done");

                DebugMSG("Saving as bmp");

                maze.SaveMazeAsBmp("mazeSavedDirectly.bmp");

                DebugMSG("Saving done :)");

                DebugMSG("Generating walls...");

                var walls = maze.GenerateListOfMazeWalls();

                DebugMSG("Generating walls done :)");

                DebugMSG("Saving walls to file...");

                using (FileStream stream = new FileStream("mazeSavedAsWalls.txt", FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        foreach (MazeWall w in walls)
                        {
                            writer.Write(w.xstart);
                            writer.Write(w.xend);
                            writer.Write(w.ystart);
                            writer.Write(w.yend);
                        }
                    }
                }

                DebugMSG("Everything done \\o/");
            });
        }
    }


}
