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
		public Program ()
		{
			int size = 2048*8;
			GenerateMazeWithThisTypeAndShowTime(InnerMapType.BitArreintjeFast, size);
			GenerateMazeWithThisTypeAndShowTime(InnerMapType.BitArreintjeFast, size);
			Thread.Sleep(5000);
			GenerateMazeWithThisTypeAndShowTime(InnerMapType.BitArreintjeFast, size);

			Thread.Sleep (5000);
		}

		private void GenerateMazeWithThisTypeAndShowTime(InnerMapType type, int size)
		{
			Stopwatch w = new Stopwatch();
			w.Start();
			
			AlgorithmBacktrack back = new AlgorithmBacktrack();
			DebugMSG("Generating maze of type: " + type + " of size: " + size);
			Maze maze = back.Generate(size, size, type, null);
			//var path = PathFinderDepthFirst.GoFind(new MazePoint(1, 1), new MazePoint(size - 3, size - 3), maze.InnerMap);
			//maze.SaveMazeAsBmpWithPath4bpp("maze.bmp", path);
			DebugMSG("Ok done, time: " + w.Elapsed.TotalSeconds);
			DebugMSG("");
			
		}

		static void Main (string[] args)
		{
			Program p = new Program ();
		}

		public void DebugMSG (String str)
		{
			Console.WriteLine (str);
		}
	}
}
