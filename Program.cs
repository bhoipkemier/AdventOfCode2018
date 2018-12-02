using AdventOfCode2018.DayCodeBase;
using System;
using System.Linq;

namespace AdventOfCode2018
{
	internal class Program
	{
		private static readonly DayCodeBase.DayCodeBase[] CodeBases = {
			new Day1(),
			new Day2(), 
		};

		private static void Main(string[] args)
		{
			Console.WriteLine(CodeBases.Last().Problem1());
			Console.WriteLine("===================================================");
			Console.WriteLine(CodeBases.Last().Problem2());
			Console.ReadLine();
		}
	}
}
