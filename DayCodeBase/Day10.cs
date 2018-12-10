using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day10 : DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData()
				.Select(d => new LightPositions(d))
				.ToList();
			var time = GetMinimumDeviationTime(data);
			return ShowMessage(data.Select(d => d.FastForward(time)).ToList());
		}
		public override string Problem2()
		{
			var data = GetData()
				.Select(d => new LightPositions(d))
				.ToList();
			var time = GetMinimumDeviationTime(data);
			return time.ToString();
		}

		private static int GetMinimumDeviationTime(List<LightPositions> data)
		{
			var spread = int.MaxValue;
			var time = 1;
			for (
				var newSpread = GetSpread(data, 1);
				newSpread < spread;
				spread = newSpread, newSpread = GetSpread(data, ++time)) ;
			return time - 1;
		}

		private static string ShowMessage(List<Point> points)
		{
			var toReturn = new StringBuilder();
			var maxX = points.Select(p => p.X).Max();
			var maxY = points.Select(p => p.Y).Max();
			toReturn.AppendLine();
			for (var y = points.Select(p => p.Y).Min(); y <= maxY; ++y)
			{
				for (var x = points.Select(p => p.X).Min(); x <= maxX; ++x)
				{
					toReturn.Append(points.Any(p => p.X == x && p.Y == y) ? '*' : ' ');
				}
				toReturn.AppendLine();
			}
			return toReturn.ToString();
		}

		private static int GetSpread(List<LightPositions> data, int time)
		{
			var ys = data.Select(d => d.FastForward(time).Y).ToList();
			return ys.Max() - ys.Min();
		}

		public class LightPositions
		{
			public Point Point { get; set; }
			public Point Velocity { get; set; }

			public Point FastForward(int seconds)
			{
				return new Point(Point.X + Velocity.X * seconds, Point.Y + Velocity.Y * seconds);
			}

			public LightPositions(string data)
			{
				var pattern = @".*\<(.*),\s(.*)\>.*<(.*),\s(.*)\>";
				var match = Regex.Matches(data, pattern)[0];
				Point = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
				Velocity = new Point(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));

			}
		}
	}
}
