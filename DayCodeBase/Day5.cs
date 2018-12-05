using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day5: DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData().First();
			return Implode(data).Count.ToString();
		}

		public override string Problem2()
		{
			var data = GetData().First();
			return data.ToLower()
				.Distinct()
				.Select(c =>
				{
					var removed = data
						.Replace(new string(c, 1).ToLower(), string.Empty)
						.Replace(new string(c, 1).ToUpper(), string.Empty);
					return Implode(removed).Count;
				})
				.Min()
				.ToString();
		}

		private static List<char> Implode(string dataStr)
		{
			var data = dataStr.ToCharArray().ToList();
			for (var i = 1; i < data.Count; i = Math.Max(++i, 1))
			{
				if (data[i] != data[i - 1] &&
				    string.Equals(data[i].ToString(), data[i - 1].ToString(), StringComparison.CurrentCultureIgnoreCase))
				{
					data.RemoveAt(i);
					data.RemoveAt(i-1);
					i -= 2;
				}
			}

			return data;
		}
	}
}
