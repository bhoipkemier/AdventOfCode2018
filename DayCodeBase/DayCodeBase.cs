using System;
using System.IO;

namespace AdventOfCode2018.DayCodeBase
{
	public abstract class DayCodeBase
	{
		public string[] GetData(int fileCount = 0, string splitChars = "\n")
		{
			var filename = $"Data/{GetType().Name}_{fileCount}.txt";
			return File
				.ReadAllText(filename)
				.Split(splitChars.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
		}

		public virtual string Problem1() => string.Empty;

		public virtual string Problem2() => string.Empty;
	}
}
