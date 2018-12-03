using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day3: DayCodeBase
	{
		public override string Problem1()
		{
			var intersection = new HashSet<string>();
			var data = GetData().Select(GetRectangles).ToList();
			foreach (var rectangle1 in data)
			{
				foreach (var rectangle2 in data)
				{
					if (rectangle1.Item1 == rectangle2.Item1) continue;
					foreach (var intersect in GetIntersection(rectangle1.Item2, rectangle2.Item2))
					{
						intersection.Add(intersect);
					}
				}
			}
			return intersection.Count.ToString();
		}

		public override string Problem2()
		{
			var data = GetData().Select(GetRectangles).ToList();
			return data.First(r1 => !data.Any(r2 => r1.Item1 != r2.Item1 && GetIntersection(r1.Item2, r2.Item2).Any())).Item1.ToString();
		}

		private static IEnumerable<string> GetIntersection(Rectangle r1, Rectangle r2)
		{
			r1.Intersect(r2);
			if (r1.IsEmpty) yield break;
			for (var x = 0; x < r1.Width; ++x)
			{
				for (var y = 0; y < r1.Height; ++y)
					yield return $"{r1.X + x},{r1.Y + y}";
			}
		}

		private static Tuple<int, Rectangle> GetRectangles(string data)
		{
			var pattern = @"\#(\d+)\s+\@\s+(\d+),(\d+):\s+(\d+)x(\d+)";
			var matches = Regex.Matches(data, pattern);
			return new Tuple<int, Rectangle>(int.Parse(matches[0].Groups[1].Value), new Rectangle(int.Parse(matches[0].Groups[2].Value), int.Parse(matches[0].Groups[3].Value), int.Parse(matches[0].Groups[4].Value), int.Parse(matches[0].Groups[5].Value)));
		}
	}
}
