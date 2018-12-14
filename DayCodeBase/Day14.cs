using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day14 : DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData().Select(int.Parse);
			return string.Join(Environment.NewLine, data.Select(d => CalculateRecipes(d, 3, 7).ToString()));
		}
		public override string Problem2()
		{
			var data = GetData(1).Select(s => s.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray());
			return string.Join(Environment.NewLine, data.Select(d => CalculateRecipesBefore(d, 3, 7).ToString()));
		}

		private static string CalculateRecipesBefore(int[] sequence, int score1, int score2)
		{
			var recipes = new List<int>
			{
				score1,
				score2
			};
			var currentPos1 = 0;
			var currentPos2 = 1;
			int foundVal;
			for (foundVal = FoundSequence(recipes, sequence); foundVal < 0; foundVal = FoundSequence(recipes, sequence))
			{
				GetNewRecipe(recipes, ref currentPos1, ref currentPos2);
			}
			return foundVal.ToString();
		}

		private static int FoundSequence(List<int> recipes, int[] sequence)
		{
			if (recipes.Count >= sequence.Length && AreEqual(recipes, sequence, recipes.Count - sequence.Length)) return recipes.Count - sequence.Length;
			if (recipes.Count > sequence.Length && AreEqual(recipes, sequence, recipes.Count - sequence.Length - 1)) return recipes.Count - sequence.Length - 1;
			return -1;
		}

		private static bool AreEqual(List<int> recipes, int[] sequence, int index)
		{
			for (var seqIndex = 0; seqIndex < sequence.Length; ++seqIndex)
			{
				if (recipes[index + seqIndex] != sequence[seqIndex]) return false;
			}
			return true;
		}

		private static string CalculateRecipes(int i, int score1, int score2)
		{
			var recipes = new List<int>(i + 20)
			{
				score1,
				score2
			};
			var currentPos1 = 0;
			var currentPos2 = 1;
			while (recipes.Count < i + 10)
			{
				GetNewRecipe(recipes, ref currentPos1, ref currentPos2);
			}
			return $"{recipes[i]}{recipes[i + 1]}{recipes[i + 2]}{recipes[i + 3]}{recipes[i + 4]}{recipes[i + 5]}{recipes[i + 6]}{recipes[i + 7]}{recipes[i + 8]}{recipes[i + 9]}";
		}

		private static void GetNewRecipe(List<int> recipes, ref int currentPos1, ref int currentPos2)
		{
			var newSum = recipes[currentPos1] + recipes[currentPos2];
			if(newSum>=10) recipes.Add(newSum/10);
			recipes.Add(newSum % 10);
			currentPos1 = (currentPos1 + recipes[currentPos1] + 1) % recipes.Count;
			currentPos2 = (currentPos2 + recipes[currentPos2] + 1) % recipes.Count;
		}
	
	}
}
