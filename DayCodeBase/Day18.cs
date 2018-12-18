using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day18: DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData().ToList();
			var board = GetBoard(data);
			return RunGenerations(board, 10).ToString();
		}

		public override string Problem2()
		{
			var data = GetData().ToList();
			var board = GetBoard(data);
			return RunGenerations(board, 1000000000).ToString();
		}

		private static long RunGenerations(char[,] board, long genCount)
		{
			var detector = new StableDetector(10);
			for (var i = 0; i < genCount; ++i)
			{
				board = RunGeneration(board);
				if(detector.Add(GetCount(board, '|') * GetCount(board, '#'))) return detector.Prediction(genCount);
			}

			return GetCount(board, '|') * GetCount(board, '#');
		}

		public class StableDetector
		{
			private List<long> values = new List<long>();
			private int numRepetitions = 0;

			public StableDetector(int numReps)
			{
				numRepetitions = numReps;
			}

			public bool Add(long value)
			{
				try
				{
					var prevOccurance = values.Count == 0 ? -1 : values.LastIndexOf(value, values.Count - 1);
					values.Add(value);
					var cycleLength = values.Count - 1 - prevOccurance;
					for (var i = 0; i < numRepetitions; ++i)
					{
						if (values[values.Count - 1 - (i * cycleLength)] != value) return false;
					}
				}
				catch (Exception)
				{
					return false;
				}
				return true;
			}

			public long Prediction(long generation)
			{
				var value = values.Last();
				var prevOccurance = values.LastIndexOf(value, values.Count - 2);
				var cycleLength = values.Count - 1 - prevOccurance;
				int remaining = (int)(generation - values.Count);
				var offset = remaining % cycleLength;
				return values[values.Count - (cycleLength - offset) - 1];
			}
		}

		private static long GetCount(char[,] board, char cType)
		{
			return Enumerable.Range(0, board.GetLength(0))
				.Select(y => Enumerable.Range(0, board.GetLength(1))
					.Count(x => board[y, x] == cType))
				.Sum();
		}

		private static char[,] RunGeneration(char[,] board)
		{
			var toReturn = new char[board.GetLength(0), board.GetLength(1)];
			Array.Copy(board,0,toReturn,0,board.Length);
			for (var y = 0; y < board.GetLength(0); ++y)
			{
				for (var x = 0; x < board.GetLength(1); ++x)
				{
					var adjacent = GetAdjacent(board, x, y);
					toReturn[y, x] = 
						board[y, x] == '.' && adjacent.Count(c => c == '|') >= 3 ? '|' :
						board[y, x] == '.' ? '.' :
						board[y, x] == '|' && adjacent.Count(c => c == '#') >= 3 ? '#' :
						board[y, x] == '|' ? '|' :
						board[y, x] == '#' && (adjacent.Count(c => c == '#') >= 1 && adjacent.Count(c => c == '|') >= 1) ? '#' :
						'.';
				}
			}
			return toReturn;
		}

		private static IEnumerable<char> GetAdjacent(char[,] board, long x, long y)
		{
			var toReturn = new List<char>();
			if (y > 0 && x > 0) toReturn.Add(board[y - 1, x - 1]);
			if (y > 0) toReturn.Add(board[y - 1, x]);
			if (y > 0 && x < board.GetLength(1)-1) toReturn.Add(board[y - 1, x + 1]);
			if (x > 0) toReturn.Add(board[y, x - 1]);
			if (x < board.GetLength(1) - 1) toReturn.Add(board[y, x + 1]);
			if (y < board.GetLength(0) - 1 && x > 0) toReturn.Add(board[y + 1, x - 1]);
			if (y < board.GetLength(0) - 1) toReturn.Add(board[y + 1, x]);
			if (y < board.GetLength(0) - 1 && x < board.GetLength(1) - 1) toReturn.Add(board[y + 1, x + 1]);
			return toReturn;
		}

		private static char[,] GetBoard(List<string> data)
		{
			var toReturn = new char[data.Count(), data.First().Length];
			for (var y = 0; y < data.Count(); ++y)
			{
				for (var x = 0; x < data.Count(); ++x)
				{
					toReturn[y, x] = data[y][x];
				}
			}
			return toReturn;
		}
	}
}
