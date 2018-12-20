using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day20 : DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData();
			var boards = data.Select(ParseBoard).ToList();
			return string.Join(Environment.NewLine, boards.Select(b => b.GetDistances().Values.Max().ToString()));
		}

		public override string Problem2()
		{
			var data = GetData();
			var boards = data.Select(ParseBoard).ToList();
			return string.Join(Environment.NewLine, boards.Select(b => b.GetDistances().Values.Count(s => s>=1000).ToString()));
		}

		private Board ParseBoard(string path)
		{
			var toReturn = new Board();
			toReturn.Rooms.Add(new Point(0, 0));
			var offset = 1;
			FollowPath(toReturn, path, ref offset, new List<Point>(new[] {new Point(0, 0)}));
			return toReturn;
		}

		private List<Point> FollowPath(Board board, string path, ref int pathOffset, List<Point> startLocations)
		{
			startLocations = startLocations.Distinct().ToList();
			var curLocations = new List<Point>(startLocations);
			var toReturn = new List<Point>();
			for (var step = path[pathOffset]; true; step = path[++pathOffset])
			{
				if ("NSEW".Contains(step))
				{
					curLocations = curLocations.Select(p => board.Move(step, p)).ToList();
				}
				else if (step == '|')
				{
					toReturn.AddRange(curLocations);
					curLocations = new List<Point>(startLocations);
				}
				else if (")$".Contains(step))
				{
					toReturn.AddRange(curLocations);
					break;
				}
				else if (step == '(')
				{
					++pathOffset;
					curLocations = FollowPath(board, path, ref pathOffset, curLocations);
				}
			}
			return toReturn;
		}

		private class Board
		{
			private HashSet<Point> Doors { get; }
			public HashSet<Point> Rooms { get; }

			public Board()
			{
				Doors = new HashSet<Point>();
				Rooms = new HashSet<Point>();
			}
			public void Print()
			{
				var board = new StringBuilder();
				for (var y = Rooms.Select(r => r.Y).Min() - 1; y <= Rooms.Select(r => r.Y).Max() + 1; ++y)
				{
					for (var x = Rooms.Select(r => r.X).Min() - 1; x <= Rooms.Select(r => r.X).Max() + 1; ++x)
					{
						var curPoint = new Point(x,y);
						board.Append(Doors.Contains(curPoint) || Rooms.Contains(curPoint) ? ' ' : '#');
					}
					board.AppendLine();
				}
				Console.WriteLine(board.ToString());
			}

			public Point Move(char step, Point point)
			{
				var offsetX = step == 'W' ? -1 :
					step == 'E' ? 1 : 0;
				var offsetY = step == 'S' ? 1 :
					step == 'N' ? -1 : 0;
				Doors.Add(new Point(point.X + offsetX, point.Y + offsetY));
				Rooms.Add(new Point(point.X + offsetX * 2, point.Y + offsetY * 2));
				return new Point(point.X + offsetX * 2, point.Y + offsetY * 2);
			}

			public Dictionary<Point, int> GetDistances()
			{
				var toReturn = new Dictionary<Point, int>();
				var toVisit = new HashSet<Point>(Rooms.Concat(Doors));
				var visited = new HashSet<Point>();
				var curLocations = new[] {new Point(0, 0)}.ToList();
				var steps = 0;
				for (; curLocations.Any(); ++steps)
				{
					var newLocations = new List<Point>();
					foreach (var curLocation in curLocations)
					{
						var newPoints = new[]
						{
							new Point(curLocation.X - 1, curLocation.Y), new Point(curLocation.X + 1, curLocation.Y),
							new Point(curLocation.X, curLocation.Y - 1), new Point(curLocation.X, curLocation.Y + 1)
						};
						foreach (var newPoint in newPoints)
						{
							if (!toVisit.Contains(newPoint) || visited.Contains(newPoint)) continue;
							newLocations.Add(newPoint);
							visited.Add(newPoint);
							if(Rooms.Contains(newPoint)) toReturn.Add(newPoint, steps/2 + 1);
						}
					}

					curLocations = newLocations;
				}

				return toReturn;
			}
		}
	}
}
