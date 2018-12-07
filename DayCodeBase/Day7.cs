using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day7 : DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData().Select(GetRules).ToList();
			return Compute(data);
		}
		public override string Problem2()
		{
			var data = GetData().Select(GetRules).ToList();
			return Compute(data, 5);
		}


		private string Compute(List<Tuple<char, char>> data)
		{
			var toReturn = string.Empty;
			var allItems = new HashSet<char>(data.Select(d => d.Item1).Concat(data.Select(d => d.Item2)).Distinct());
			var itemsFound = new HashSet<char>();
			while (data.Any())
			{
				var cantDo = new HashSet<char>(data.Select(d => d.Item2).Distinct());
				var canDo = allItems.Where(i => !itemsFound.Contains(i))
					.Where(i => !cantDo.Contains(i))
					.ToList();
				var first = canDo.OrderBy(c => c).First();
				itemsFound.Add(first);
				toReturn += first;
				data = data.Where(d => d.Item1 != first).ToList();
			}

			foreach (var c in allItems.Where(i => !itemsFound.Contains(i)).OrderBy(i => i))
			{
				toReturn += c;
			}

			return toReturn;
		}

		private string Compute(List<Tuple<char, char>> data, int numWorkers)
		{
			var second = 0;
			var allItems = new HashSet<char>(data.Select(d => d.Item1).Concat(data.Select(d => d.Item2)).Distinct());
			Dictionary<char, int> time =
				allItems.ToDictionary(c => c, c => 60 + "ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c.ToString()) + 1);
			var inProgress = new List<char>();
			var toDo = new HashSet<char>(allItems);
			for (; inProgress.Any() || toDo.Any(); ++second)
			{
				var cantDo = new HashSet<char>(data.Select(d => d.Item2).Distinct());
				var canDo = toDo.Where(i => !inProgress.Contains(i))
					.Where(i => !cantDo.Contains(i))
					.OrderBy(c => c)
					.ToList();

				while (inProgress.Count < numWorkers && canDo.Any())
				{
					inProgress.Add(canDo.First());
					toDo.Remove(canDo.First());
					canDo.RemoveAt(0);
				}

				foreach (var c in inProgress)
				{
					time[c] = time[c] - 1;
				}

				var toRemove = inProgress.Where(c => time[c] == 0).ToList();
				data = data.Where(d => !toRemove.Contains(d.Item1)).ToList();
				inProgress = inProgress.Where(c => time[c] > 0).ToList();
			}

			return second.ToString();
		}

		


		private static Tuple<char, char> GetRules(string data)
		{
			var c1 = data.Substring(5, 1)[0];
			var c2 = data.Substring(36, 1)[0];
			return new Tuple<char, char>(c1, c2);
		}
	}
}
