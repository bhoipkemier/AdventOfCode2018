using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day1: DayCodeBase
	{
		public override string Problem1()
		{
			return GetData()
				.Select(int.Parse)
				.Sum()
				.ToString();
		}

		public override string Problem2()
		{
			var currentFrequency = 0;
			var seenFrequencies = new HashSet<int>(new[] { currentFrequency });
			var numbers = GetData().Select(int.Parse).ToList();
			while (true)
			{
				foreach (var number in numbers)
				{
					currentFrequency += number;
					if (seenFrequencies.Contains(currentFrequency))
						return currentFrequency.ToString();
					seenFrequencies.Add(currentFrequency);
				}
			}
		}
	}
}
