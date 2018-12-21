using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day21 : DayCodeBase
	{
		private static readonly Dictionary<string, Action<int, int, int, int[]>> AllCommands = new Dictionary<string, Action<int, int, int, int[]>>{
			{"addr", Addr },
			{"addi", Addi },
			{"mulr", Mulr},
			{"muli", Muli},
			{"banr", Banr},
			{"bani", Bani},
			{"borr", Borr},
			{"bori", Bori},
			{"setr", Setr},
			{"seti", Seti},
			{"gtir", Gtir},
			{"gtri", Gtri},
			{"gtrr", Gtrr},
			{"eqir", Eqir},
			{"eqri", Eqri},
			{"eqrr", Eqrr}
		};

		public override string Problem1()
		{
			var data = GetData().ToArray();
			var ip = int.Parse(data.First().Split(' ')[1]);
			var instructions = ParseInstructions(data.Skip(1).ToArray());
			var registers = ProcessInstructions(instructions, ip, true);
			return registers[3].ToString();
		}
		public override string Problem2()
		{
			var data = GetData().ToArray();
			var ip = int.Parse(data.First().Split(' ')[1]);
			var instructions = ParseInstructions(data.Skip(1).ToArray());
			var registers = ProcessInstructions(instructions, ip, false);
			return registers[6].ToString();
		}

		private static int[] ProcessInstructions(List<Day19.InstructionCommand> instructions, int ip, bool problem1)
		{
			var reg3 = new HashSet<int>();
			var debug = false;
			var registers = new[] { 0, 0, 0, 0, 0, 0, 0 };
			for (var nextCommand = registers[ip]; nextCommand < instructions.Count && nextCommand >= 0; registers[ip]++, nextCommand = registers[ip])
			{
				if (problem1 && 28 == nextCommand) return registers;
				if (!problem1 && 28 == nextCommand)
				{

					if (reg3.Contains(registers[3])) return registers;
					reg3.Add(registers[3]);
					registers[6] = registers[3];
				}
				if (debug)
					Console.Write(
						$"ip={nextCommand} [{string.Join(", ", registers.Select(r => r.ToString()).ToArray())}] {AllCommands.First(kvp => kvp.Value == instructions[nextCommand].Operator).Key} {instructions[nextCommand].Operand1} {instructions[nextCommand].Operand2} {instructions[nextCommand].Resultant} ");
				instructions[nextCommand].Run(registers);
				if (debug) Console.WriteLine($" [{string.Join(", ", registers.Select(r => r.ToString()).ToArray())}]");
			}
			return registers;
		}

		private static List<Day19.InstructionCommand> ParseInstructions(string[] data)
		{
			var toReturn = new List<Day19.InstructionCommand>();
			foreach (var line in data)
			{
				var splits = (line + " 0 0").Split(' ');
				var ints = splits.Skip(1).Select(int.Parse).ToArray();
				toReturn.Add(new Day19.InstructionCommand
				{
					Operator = AllCommands[splits[0]],
					Operand1 = ints[0],
					Operand2 = ints[1],
					Resultant = ints[2],
				});
			}

			return toReturn;
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

		private static void Ip(int a, int b, int c, int[] registers)
		{
			registers[6] = a;
		}

		public class InstructionCommand
		{
			public Action<int, int, int, int[]> Operator { get; set; }
			public int Operand1 { get; set; }
			public int Operand2 { get; set; }
			public int Resultant { get; set; }

			public void Run(int[] registers)
			{
				Operator(Operand1, Operand2, Resultant, registers);
			}
		}
	}
}