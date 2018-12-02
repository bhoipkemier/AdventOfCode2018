using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day2: DayCodeBase
	{
		public override string Problem1()
		{
			var ids = GetData().Select(s => s.ToCharArray()).ToList();
			return (ids.Count(id => AppearsExactly(id, 2)) * ids.Count(id => AppearsExactly(id, 3))).ToString();
		}

		public override string Problem2()
		{
			var ids = GetData();
			foreach (var id in ids)
			{
				foreach (var id2 in ids)
				{
					var commonId = GetCommonId(id, id2);
					if (commonId != null) return commonId;
				}
			}
			return string.Empty;
		}

		private static string GetCommonId(string id, string id2)
		{
			var toReturn = id
				.Where((t, i) => t == id2[i])
				.Aggregate("", (current, t) => current + t);

			return toReturn.Length == id.Length - 1 ? toReturn : null;
		}

		private static bool AppearsExactly(IEnumerable<char> id, int times)
		{
			return id
				.GroupBy(c => c)
				.Any(g => g.Count() == times);
		}
	}
}
