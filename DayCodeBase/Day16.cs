using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day16 : DayCodeBase
	{
		private static readonly Action<int, int, int, int[]>[] AllCommands = new Action<int, int, int, int[]>[] { Addr, Addi, Mulr, Muli, Banr, Bani, Borr, Bori, Setr, Seti, Gtir, Gtri, Gtrr, Eqir, Eqri, Eqrr };

		public override string Problem1()
		{
			var data = GetData().ToList();
			var testCases = ParseTestCaseses(data);
			return GetCasesOver(testCases, 3).ToString();
		}
		public override string Problem2()
		{
			var testCases = ParseTestCaseses(GetData().ToList());
			var opCodeLookup = GetOpCodeLookup(testCases);
			var instructions = ParseInstructions(GetData(1));
			var registers = ProcessInstructions(opCodeLookup, instructions);
			return registers[0].ToString();
		}

		private static int[] ProcessInstructions(Action<int, int, int, int[]>[] opCodeLookup, List<InstructionCommand> instructions)
		{
			var registers = new[] {0, 0, 0, 0};
			foreach (var command in instructions)
			{
				opCodeLookup[command.Operator](command.Operand1, command.Operand2, command.Resultant, registers);
			}
			return registers;
		}

		private static List<InstructionCommand> ParseInstructions(string[] data)
		{
			var toReturn = new List<InstructionCommand>();
			foreach (var line in data)
			{
				var splits = line.Split(' ').Select(int.Parse).ToArray();
				toReturn.Add(new InstructionCommand
				{
					Operator = splits[0],
					Operand1 = splits[1],
					Operand2 = splits[2],
					Resultant = splits[3],
				});
			}

			return toReturn;
		}

		private static Action<int, int, int, int[]>[] GetOpCodeLookup(List<TestCases> testCases)
		{
			var eliminationList = Enumerable.Range(1, 16).Select(i => new HashSet<Action<int, int, int, int[]>>(AllCommands)).ToArray();
			foreach (var testCase in testCases)
			{
				eliminationList[testCase.Command.Operator] = new HashSet<Action<int, int, int, int[]>>(
					eliminationList[testCase.Command.Operator].Intersect(MatchingInst(testCase))
					);
			}

			EliminateMore(eliminationList);
			return eliminationList.Select(i => i.Single()).ToArray();
		}

		private static void EliminateMore(HashSet<Action<int, int, int, int[]>>[] eliminationList)
		{
			while (eliminationList.Any(e => e.Count > 1))
			{
				var canElimnate = eliminationList.Where(e => e.Count == 1).SelectMany(e => e).ToList();
				foreach (var hashSet in eliminationList)
				{
					if (hashSet.Count <= 1) continue;
					foreach (var toEliminate in canElimnate)
					{
						hashSet.Remove(toEliminate);
					}
				}
			}
		}

		private static void Addr(int a, int b, int c, int[] registers)
		{
			registers[c] = registers[a] + registers[b];
		}

		private static void Addi(int a, int b, int c, int[] registers)
		{
			registers[c] = registers[a] + b;
		}

		private static void Mulr(int a, int b, int c, int[] registers)
		{
			registers[c] = registers[a] * registers[b];
		}

		private static void Muli(int a, int b, int c, int[] registers)
		{
			registers[c] = registers[a] * b;
		}

		private static void Banr(int a, int b, int c, int[] registers)
		{
			registers[c] = registers[a] & registers[b];
		}

		private static void Bani(int a, int b, int c, int[] registers)
		{
			registers[c] = registers[a] & b;
		}

		private static void Borr(int a, int b, int c, int[] registers)
		{
			registers[c] = registers[a] | registers[b];
		}

		private static void Bori(int a, int b, int c, int[] registers)
		{
			registers[c] = registers[a] | b;
		}

		private static void Setr(int a, int b, int c, int[] registers)
		{
			registers[c] = registers[a];
		}

		private static void Seti(int a, int b, int c, int[] registers)
		{
			registers[c] = a;
		}

		private static void Gtir(int a, int b, int c, int[] registers)
		{
			registers[c] = a > registers[b] ? 1 : 0;
		}

		private static void Gtri(int a, int b, int c, int[] registers)
		{
			registers[c] = registers[a] > b ? 1 : 0;
		}

		private static void Gtrr(int a, int b, int c, int[] registers)
		{
			registers[c] = registers[a] > registers[b] ? 1 : 0;
		}

		private static void Eqir(int a, int b, int c, int[] registers)
		{
			registers[c] = a == registers[b] ? 1 : 0;
		}

		private static void Eqri(int a, int b, int c, int[] registers)
		{
			registers[c] = registers[a] == b ? 1 : 0;
		}

		private static void Eqrr(int a, int b, int c, int[] registers)
		{
			registers[c] = registers[a] == registers[b] ? 1 : 0;
		}

		private static int GetCasesOver(List<TestCases> testCases, int numMatches)
		{
			return testCases.Select(MatchingInst).Count(matches => matches.Length >= numMatches);
		}

		private static Action<int, int, int, int[]>[] MatchingInst(TestCases testCase)
		{
			return AllCommands.Where(c => CommandMatches(c, testCase)).ToArray();
		}

		private static bool CommandMatches(Action<int, int, int, int[]> instruction, TestCases testCase)
		{
			var reg = new List<int>(testCase.Before).ToArray();
			instruction(testCase.Command.Operand1, testCase.Command.Operand2, testCase.Command.Resultant, reg);
			return reg.SequenceEqual(testCase.After);
		}

		private static List<TestCases> ParseTestCaseses(List<string> data)
		{
			var toReturn = new List<TestCases>();
			const string baPattern = @".*\[(\d+),\s(\d+),\s(\d+),\s(\d+)\]";
			const string instPattern = @"(\d+)\s(\d+)\s(\d+)\s(\d+)";
			int[] GetArray(Match match) => Enumerable.Range(1, 4).Select(x => int.Parse(match.Groups[x].Value)).ToArray();
			for (var i = 0; i < data.Count; i += 4)
			{
				var beforeMatch = Regex.Matches(data[i], baPattern)[0];
				var instructionMatch = Regex.Matches(data[i + 1], instPattern)[0];
				var afterMatch = Regex.Matches(data[i + 2], baPattern)[0];
				var command = GetArray(instructionMatch);
				toReturn.Add(new TestCases
				{
					Before = GetArray(beforeMatch),
					After = GetArray(afterMatch),
					Command = new InstructionCommand
					{
						Operator = command[0],
						Operand1 = command[1],
						Operand2 = command[2],
						Resultant = command[3]
					}
				});
			}
			return toReturn;
		}

		public class InstructionCommand
		{
			public int Operator { get; set; }
			public int Operand1 { get; set; }
			public int Operand2 { get; set; }
			public int Resultant { get; set; }
		}

		public class TestCases
		{
			public int[] Before { get; set; }
			public int[] After { get; set; }
			public InstructionCommand Command { get; set; }
		}
	}
}
