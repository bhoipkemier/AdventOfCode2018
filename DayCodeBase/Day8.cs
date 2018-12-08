using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day8 : DayCodeBase
	{
		public override string Problem1()
		{
			var numbers = GetNumbers(GetData().First());
			var root = GetRoot(numbers);
			return AddMetaData(root).ToString();
		}

		public override string Problem2()
		{
			var numbers = GetNumbers(GetData().First());
			var root = GetRoot(numbers);
			return CalculateProb2Sum(root).ToString();
		}

		private static int CalculateProb2Sum(Node root)
		{
			if (!root.Children.Any()) return root.MetaData.Sum();
			var toReturn = 0;
			foreach (var metadata in root.MetaData)
			{
				if (metadata > 0 && metadata <= root.Children.Count)
				{
					toReturn += CalculateProb2Sum(root.Children[metadata - 1]);
				}
			}

			return toReturn;
		}

		private static int AddMetaData(Node root)
		{
			return root.MetaData.Sum() + root.Children.Select(AddMetaData).Sum();
		}

		private static List<int> GetNumbers(string data)
		{
			return data.Split(' ').Select(int.Parse).ToList();
		}

		private static Node GetRoot(IList<int> numbers)
		{
			var toReturn = new Node();
			var numChildren = numbers.First();
			numbers.RemoveAt(0);
			var numMetadata = numbers.First();
			numbers.RemoveAt(0);
			for (var child = 0; child < numChildren; ++child)
			{
				toReturn.Children.Add(GetRoot(numbers));
			}

			for (var metadata = 0; metadata < numMetadata; ++metadata)
			{
				toReturn.MetaData.Add(numbers.First());
				numbers.RemoveAt(0);
			}

			return toReturn;
		}

		private class Node
		{
			public List<Node> Children { get; }
			public List<int> MetaData { get; }

			public Node()
			{
				Children = new List<Node>();
				MetaData = new List<int>();
			}
		}
	}
}
