using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day6: DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData().Select(GetLocationInfo).ToList();
			CalculateCounts(data);
			return data.Where(li => !li.Infinite).Select(li => li.Count).Max().ToString();
		}

		public override string Problem2()
		{
			var data = GetData().Select(GetLocationInfo).ToList();
			return GetLocationsWithin(data, 10000).ToString();
		}

		private int GetLocationsWithin(List<LocationInfo> data, int distance)
		{
			var toReturn = 0;
			var minX = data.Select(d => d.Point.X).Min();
			var maxX = data.Select(d => d.Point.X).Max();
			var minY = data.Select(d => d.Point.Y).Min();
			var maxY = data.Select(d => d.Point.Y).Max();
			for (var x = minX - 1; x <= maxX + 1; ++x)
			{
				var infiniteX = x == minX - 1 || x == maxX + 1;
				for (var y = minY - 1; y <= maxY + 1; ++y)
				{
					var infiniteY = y == minY - 1 || y == maxY + 1;
					var sumDistance = data.Select(d => GetDistance(d, x, y)).Sum();
					if (sumDistance < distance)
					{
						toReturn++;
						if (infiniteX || infiniteY) throw new Exception();
					}
				}
			}
			return toReturn;
		}

		private void CalculateCounts(List<LocationInfo> data)
		{
			var minX = data.Select(d => d.Point.X).Min();
			var maxX = data.Select(d => d.Point.X).Max();
			var minY = data.Select(d => d.Point.Y).Min();
			var maxY = data.Select(d => d.Point.Y).Max();
			for (var x = minX - 1; x <= maxX + 1; ++x)
			{
				var infiniteX = x == minX - 1 || x == maxX + 1;
				for (var y = minY - 1; y <= maxY + 1; ++y)
				{
					var infiniteY = y == minY - 1 || y == maxY + 1;
					var closest = GetClosest(data, x, y);
					if (closest == null) continue;
					closest.Count += 1;
					if (infiniteX || infiniteY)
						closest.Infinite = true;
				}
			}
		}

		private LocationInfo GetClosest(List<LocationInfo> data, int x, int y)
		{
			LocationInfo toReturn = null;
			var curDistance = int.MaxValue;
			foreach (var locationInfo in data)
			{
				var distance = GetDistance(locationInfo, x, y);
				if (distance == curDistance) toReturn = null;
				if (distance >= curDistance) continue;
				curDistance = distance;
				toReturn = locationInfo;
			}
			return toReturn;
		}

		private int GetDistance(LocationInfo locationInfo, int x, int y)
		{
			return Math.Abs(locationInfo.Point.X - x) + Math.Abs(locationInfo.Point.Y - y);
		}

		private static LocationInfo GetLocationInfo(string data)
		{
			var cords = data.Split(',');
			return new LocationInfo
			{
				Count = 0,
				Infinite = false,
				Point = new Point(int.Parse(cords[0]), int.Parse(cords[1]))
			};
		}

		class LocationInfo
		{
			public bool Infinite { get; set; }
			public int Count { get; set; }
			public Point Point { get; set; }
		}
	}
}
