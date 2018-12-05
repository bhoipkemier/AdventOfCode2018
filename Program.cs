using System;
using System.Linq;

namespace AdventOfCode2018
{
	internal class Program
	{
		private static readonly DayCodeBase.DayCodeBase[] CodeBases = typeof(Program)
			.Assembly
			.GetTypes()
			.Where(c => c.Name.StartsWith("Day") && !c.Name.StartsWith("DayCode"))
			.OrderBy(c => int.Parse(c.Name.Substring(3)))
			.Select(c => c.GetConstructor(new Type[] { }).Invoke(new object[] { }))
			.Cast<DayCodeBase.DayCodeBase>()
			.ToArray();

		private static void Main(string[] args)
		{
			while (true)
			{
				Console.Write("Enter the day: ");
				var day = Console.ReadLine();
				if (day == string.Empty)
				{
					RunDay(CodeBases.Length - 1);
				}else if (int.TryParse(day, out var selDay))
				{
					RunDay(selDay - 1);
				}
				else
				{
					return;
				}
			}
		}

		private static void RunDay(int day)
		{
			if (day < 0 || CodeBases.Length <= day) day = CodeBases.Length - 1;
			var selectedDay = CodeBases[day];
			Console.WriteLine("===================================================");
			Console.WriteLine($"Day {day + 1}");
			Console.WriteLine($"Problem 1: {selectedDay.Problem1()}");
			Console.WriteLine($"Problem 2: {selectedDay.Problem2()}");
			Console.WriteLine("===================================================");
		}
	}
}
