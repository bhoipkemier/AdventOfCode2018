using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.DayCodeBase
{
	public abstract class DayCodeBase
	{
		public string[] GetData(int fileCount = 0, string splitChars = "\n")
		{
			var filename = $"Data/{GetType().Name}_{fileCount}.txt";
			return File
				.ReadAllText(filename)
				.Split(splitChars.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
				.Select(s => s.Replace("\r", string.Empty))
				.ToArray();
		}

		public virtual string Problem1() => string.Empty;

		public virtual string Problem2() => string.Empty;
	}
}
