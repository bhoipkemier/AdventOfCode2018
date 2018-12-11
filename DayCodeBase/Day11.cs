using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day11 : DayCodeBase
	{
		
		public override string Problem1()
		{
			var serialNumbers = GetData()
				.Select(int.Parse)
				.Select(s => FindLargestPowerCoord(300, s, 3))
				.Select(p => $"{p.Point.X},{p.Point.Y}")
				.ToArray();

			return string.Join(Environment.NewLine, serialNumbers);
		}

		public override string Problem2()
		{
			var serialNumbers = GetData()
				.Select(int.Parse)
				.Select(s => FindLargestPowerCoord(300, s))
				.Select(p => $"{p.Point.X},{p.Point.Y},{p.Size}")
				.ToArray();

			return string.Join(Environment.NewLine, serialNumbers);
		}

		private static PowerCoord FindLargestPowerCoord(int gridSize, int serialNumber)
		{
			var toReturn = new PowerCoord(Point.Empty, int.MinValue, 0);
			var grid = GetGrid(gridSize, serialNumber);
			for (var subGridSize = 1; subGridSize <= gridSize; ++subGridSize)
			{
				Debug.WriteLine(subGridSize.ToString());
				var powerCoord = GetMaxGridLocation(grid, subGridSize);
				if (powerCoord.Power > toReturn.Power) toReturn = powerCoord;
			}
			return toReturn;
		}

		private static PowerCoord FindLargestPowerCoord(int gridSize, int serialNumber, int subGridSize)
		{
			var grid = GetGrid(gridSize, serialNumber);
			return GetMaxGridLocation(grid, subGridSize);
		}

		private static PowerCoord GetMaxGridLocation(int[,] grid, int subGridSize)
		{
			var toReturn = new PowerCoord(Point.Empty, int.MinValue, subGridSize);
			for (var x = 0; x < grid.GetLength(0) - subGridSize; ++x)
			{
				for (var y = 0; y < grid.GetLength(0) - subGridSize; ++y)
				{
					var power = 0;
					for (var subx = 0; subx < subGridSize; ++subx)
					{
						for (var suby = 0; suby < subGridSize; ++suby)
						{
							power += grid[x + subx, y + suby];
						}
					}

					if (power > toReturn.Power)
					{
						toReturn.Power = power;
						toReturn.Point = new Point(x + 1, y + 1);
					}
				}
			}

			return toReturn;
		}

		class PowerCoord 

		{
			public Point Point;
			public int Power;
			public int Size;

			public PowerCoord(Point point, int power, int size)
			{
				Point = point;
				Power = power;
				Size = size;
			}
		}

		private static int[,] GetGrid(int gridSize, int serialNumber)
		{
			var toReturn = new int[gridSize, gridSize];
			for (var x = 0; x < gridSize; ++x)
			{
				for (var y = 0; y < gridSize; ++y)
				{
					var rackId = x + 11;
					var powerLevel = ((rackId * (y + 1) + serialNumber) * rackId / 100) % 10 - 5;
					toReturn[x, y] = powerLevel;
				}
			}
			return toReturn;
		}
	}
}
