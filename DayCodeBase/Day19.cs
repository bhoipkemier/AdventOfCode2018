using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day19 : DayCodeBase
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
			var registers = ProcessInstructions(instructions, ip);
			return registers[0].ToString();
		}

		public override string Problem2()
		{
			//var data = GetData().ToArray();
			//var ip = int.Parse(data.First().Split(' ')[1]);
			//var instructions = ParseInstructions(data.Skip(1).ToArray());
			//var registers = ProcessInstructions(instructions, ip, 1);
			//return registers[0].ToString();
			return "Sum of factors of 10551428\r\n1+2+4+67+134+268+39371+78742+157484+2637857+5275714+10551428\r\n18741072";
		}

		private static int[] ProcessInstructions(List<InstructionCommand> instructions, int ip, int startr0 = 0)
		{
			var debug = false;
			var registers = new[] { startr0, 0, 0, 0, 0, 0 };
			for(var nextCommand = registers[ip]; nextCommand < instructions.Count && nextCommand >= 0; registers[ip]++, nextCommand = registers[ip])
			{
				if(debug)
					Console.Write(
						$"ip={nextCommand} [{string.Join(", ", registers.Select(r => r.ToString()).ToArray())}] {AllCommands.First(kvp => kvp.Value == instructions[nextCommand].Operator).Key} {instructions[nextCommand].Operand1} {instructions[nextCommand].Operand2} {instructions[nextCommand].Resultant} ");
				instructions[nextCommand].Run(registers);
				if(debug) Console.WriteLine($" [{string.Join(", ", registers.Select(r => r.ToString()).ToArray())}]");
			}
			return registers;
		}

		private static List<InstructionCommand> ParseInstructions(string[] data)
		{
			var toReturn = new List<InstructionCommand>();
			foreach (var line in data)
			{
				var splits = (line + " 0 0").Split(' ');
				var ints = splits.Skip(1).Select(int.Parse).ToArray();
				toReturn.Add(new InstructionCommand
				{
					Operator = AllCommands[splits[0]],
					Operand1 = ints[0],
					Operand2 = ints[1],
					Resultant =ints[2],
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
