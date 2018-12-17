using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day17 : DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData().ToList();
			var clay = ParseData(data);
			var stopPoint = clay.Max(p => p.Y);
			return GetWaterCoverage(clay, stopPoint, true).ToString();
		}

		public override string Problem2()
		{
			var data = GetData().ToList();
			var clay = ParseData(data);
			var stopPoint = clay.Max(p => p.Y);
			return GetWaterCoverage(clay, stopPoint, false).ToString();
		}

		private int GetWaterCoverage(HashSet<Point> clay, int stopPoint, bool countFalling)
		{
			var water = new HashSet<Point>();
			var fallingWater = new HashSet<Point>();
			WaterFalls(new Point(500, 0), clay, water, fallingWater, stopPoint);
			return countFalling ? water.Concat(fallingWater).Distinct().Count() - clay.Min(c => c.Y) : water.Count - clay.Min(c => c.Y);
		}

		private void Debug(HashSet<Point> clay, HashSet<Point> water, HashSet<Point> fallingWater)
		{
			var toPrint = new StringBuilder();
			var minx = clay.Concat(water).Concat(fallingWater).Min(c => c.X);
			var maxx = clay.Concat(water).Concat(fallingWater).Max(c => c.X);
			var miny = clay.Concat(water).Concat(fallingWater).Min(c => c.Y);
			var maxy = clay.Concat(water).Concat(fallingWater).Max(c => c.Y);

			for (var y = miny; y <= maxy; ++y)
			{
				toPrint.AppendLine();
				for (var x = minx; x <= maxx; ++x)
				{
					var point = new Point(x, y);
					toPrint.Append(
						clay.Contains(point) ? '#' :
						water.Contains(point) ? '~' :
						fallingWater.Contains(point) ? '|' : '.');
				}
			}
			Console.WriteLine(toPrint);
		}

		private void WaterFalls(Point fallPoint, HashSet<Point> clay, HashSet<Point> water, HashSet<Point> fallingWater, int stopPoint)
		{
			if (fallPoint.Y > stopPoint) return;
			fallingWater.Add(fallPoint);
			var belowPoint = new Point(fallPoint.X, fallPoint.Y + 1);
			if (!clay.Contains(belowPoint) && !water.Contains(belowPoint))
			{
				WaterFalls(belowPoint, clay, water, fallingWater, stopPoint);
			}
			if (clay.Contains(belowPoint) || water.Contains(belowPoint))
			{
				if (IsBounded(fallPoint, clay, water))
				{
					fallingWater.Remove(fallPoint);
					FillWater(fallPoint, water, clay);
				}
				else
				{
					FallWaterHorizontally(fallPoint, fallingWater, clay, water, stopPoint);
				}
			}
		}

		private void FallWaterHorizontally(Point fallPoint, HashSet<Point> fallingWater, HashSet<Point> clay, HashSet<Point> water, int stopPoint)
		{

			var stoppedEarly = false;
			for (var x = fallPoint.X + 1; !clay.Contains(new Point(x, fallPoint.Y)); ++x)
			{
				fallingWater.Add(new Point(x, fallPoint.Y));
				if (clay.Contains(new Point(x, fallPoint.Y + 1)) || water.Contains(new Point(x, fallPoint.Y + 1))) continue;
				WaterFalls(new Point(x, fallPoint.Y + 1), clay, water, fallingWater, stopPoint);
				if(fallingWater.Contains(new Point(x, fallPoint.Y + 1)))
				{
					stoppedEarly = true;
					break;
				}
			}

			for (var x = fallPoint.X - 1; !clay.Contains(new Point(x, fallPoint.Y)); --x)
			{
				fallingWater.Add(new Point(x, fallPoint.Y));
				if (clay.Contains(new Point(x, fallPoint.Y + 1)) || water.Contains(new Point(x, fallPoint.Y + 1))) continue;
				WaterFalls(new Point(x, fallPoint.Y + 1), clay, water, fallingWater, stopPoint);
				if (fallingWater.Contains(new Point(x, fallPoint.Y + 1)))
				{
					stoppedEarly = true;
					break;
				}
			}

			if (!stoppedEarly)
			{
				FillWater(fallPoint, water, clay);
			}
		}

		private void FillWater(Point fallPoint, HashSet<Point> water, HashSet<Point> clay)
		{
			water.Add(fallPoint);
			for (var x = fallPoint.X + 1; !clay.Contains(new Point(x, fallPoint.Y)); ++x)
			{
				water.Add(new Point(x, fallPoint.Y));
			}

			for (var x = fallPoint.X - 1; !clay.Contains(new Point(x, fallPoint.Y)); --x)
			{
				water.Add(new Point(x, fallPoint.Y));
			}
		}

		private bool IsBounded(Point fallPoint, HashSet<Point> clay, HashSet<Point> water)
		{
			for (var x = fallPoint.X + 1; !clay.Contains(new Point(x, fallPoint.Y)); ++x)
			{
				if (!clay.Contains(new Point(x, fallPoint.Y + 1)) && !water.Contains(new Point(x, fallPoint.Y + 1)))
				{
					return false;
				}
			}

			for (var x = fallPoint.X - 1; !clay.Contains(new Point(x, fallPoint.Y)); --x)
			{
				if (!clay.Contains(new Point(x, fallPoint.Y + 1)) && !water.Contains(new Point(x, fallPoint.Y + 1)))
				{
					return false;
				}
			}

			return true;
		}

		//private List<Point> GetFallPoints(Point fallPoint, HashSet<Point> clay, HashSet<Point> fallingWater)
		//{

		//}

		private static HashSet<Point> ParseData(IEnumerable<string> data)
		{
			var toReturn = new HashSet<Point>();
			foreach (var line in data)
			{
				var splits = line.Split(',').Select(s => s.Trim(',', ' ')).ToArray();
				var isX = splits[0].StartsWith('x');
				var firstVal = int.Parse(splits[0].Substring(splits[0].IndexOf('=') + 1));
				var rangeVals = splits[1].Split("..").Select(s => int.Parse(s.Trim('x', 'y', '=', ' '))).ToArray();
				for (var i = rangeVals[0]; i <= rangeVals[1]; ++i)
				{
					toReturn.Add(new Point(
						isX ? firstVal : i,
						isX ? i : firstVal
					));
				}

			}

			return toReturn;
		}
	}
}