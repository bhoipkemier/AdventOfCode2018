using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day12 : DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData().ToList();
			var state = new Tuple<string, long>(data.First().Substring(15), 0);
			var rules = GetRules(data.Skip(2));
			var final = AdvanceGenerations(state, rules, 20);
			return CalculateValue(final).ToString();
		}
		public override string Problem2()
		{
			var data = GetData().ToList();
			var state = new Tuple<string, long>(data.First().Substring(15), 0);
			var rules = GetRules(data.Skip(2));
			var final = AdvanceGenerations(state, rules, 50000000000);
			return CalculateValue(final).ToString();
		}

		private static long CalculateValue(Tuple<string, long> state) => state.Item1.Select((c, i) => c == '#' ? (long)i + state.Item2 : (long)0).Sum();


		private static Tuple<string, long> AdvanceGenerations(Tuple<string, long> state, HashSet<string> rules, long genCount)
		{
			for (long i = 0; i < genCount; ++i)
			{
				var prev = state;
				state = AdvanceGeneration(state, rules);
				
				if (state.Item1 == prev.Item1)
				{
					var delta = state.Item2 - prev.Item2;
					return new Tuple<string, long>(state.Item1, prev.Item2 + (delta * (genCount - i)));
				}
			}

			return state;
		}

		private static Tuple<string, long> AdvanceGeneration(Tuple<string, long> state, HashSet<string> rules)
		{
			var toReturn = new StringBuilder();
			var paddedLine = $"....{state.Item1}....";
			for (var i = 0; i < paddedLine.Length - 4; ++i)
			{
				var toConsider = paddedLine.Substring(i, 5);
				toReturn.Append(rules.Contains(toConsider) ? '#' : '.');
			}
			var newGen = toReturn.ToString();
			var firstPlant = newGen.IndexOf('#');
			newGen = newGen.Substring(firstPlant);
			return new Tuple<string, long>(newGen.Substring(0,newGen.LastIndexOf('#') + 1), state.Item2 - 2 + firstPlant);
		}

		private static HashSet<string> GetRules(IEnumerable<string> data) => new HashSet<string>(data.Where(d => d.EndsWith('#')).Select(d => d.Substring(0,5)));

	}
}
