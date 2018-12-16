using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day15 : DayCodeBase
	{

		public override string Problem1()
		{
			var data = GetData();
			var games = ParseData(data);
			foreach (var game in games)
			{
				Fight(game);
			}
			return string.Join(Environment.NewLine, games.Select(GetScore));
		}

		public override string Problem2()
		{
			var toReturn = new StringBuilder();
			var data = GetData();
			var games = ParseData(data);
			for (var i = 0; i < games.Count; ++i)
			{
				var game = ParseData(data)[i];
				var elfCount = game.Elves.Count;
				var attackPoints = 3;
				for (var first = true; first || game.Elves.Count != elfCount; first = false)
				{
					game = ParseData(data)[i];
					attackPoints += 1;
					foreach (var elf in game.Elves)
					{
						elf.AttackPoints = attackPoints;
					}
					Fight(game);
				}

				toReturn.AppendLine($"{GetScore(game)}");
			}
			return toReturn.ToString();
		}

		private static void Fight(Game game)
		{
			for (;
				game.Elves.Any(e => e.HealthPoints > 0) &&
				game.Goblins.Any(g => g.HealthPoints > 0); DoTurn(game));
		}

		private static void DoTurn(Game game)
		{
			var turnOrder = game.AllUnits.OrderBy(u => u.Position.Y).ThenBy(u => u.Position.X).ToList();
			foreach (var currentUnit in turnOrder)
			{
				if (!currentUnit.Alive) continue;
				if (!game.GetEnemies(currentUnit).Any()) return;
				Move(currentUnit, game);
				Attack(currentUnit, game);
			}
			game.Round += 1;
			//Debug(game);
		}

		private static void Move(Unit currentUnit, Game game)
		{
			if(GetAdjacentPositions(currentUnit.Position).Intersect(game.GetEnemies(currentUnit).Select(e => e.Position)).Any()) return;
			var destinations = GetDestinations(currentUnit, game);
			var paths = GetPathsToDestinations(currentUnit, game, destinations);
			if(!paths.Any()) return;
			var minPaths = paths.GroupBy(p => p.Distance, p => p)
				.OrderBy(g => g.Key)
				.First()
				.ToList()
				.OrderBy(p => p.Value.Y)
				.ThenBy(p => p.Value.X);
			var positionToMove = minPaths.First().SecondStep.Value;
			var token = game.Board[currentUnit.Position.Y, currentUnit.Position.X];
			game.Board[currentUnit.Position.Y, currentUnit.Position.X] = '.';
			game.Board[positionToMove.Y, positionToMove.X] = token;
			currentUnit.Position = positionToMove;
		}

		private static List<PathNode<Point>> GetPathsToDestinations(Unit currentUnit, Game game, List<Point> destinations)
		{
			var visited = new HashSet<Point>();
			var paths = new Queue<PathNode<Point>>();
			var completedPaths = new List<PathNode<Point>>();
			visited.Add(currentUnit.Position);
			paths.Enqueue(new PathNode<Point>{ Value = currentUnit.Position });
			while (paths.Any())
			{
				Travel(paths, game, destinations, completedPaths, visited);
			}
			return completedPaths;
		}

		private static void Travel(Queue<PathNode<Point>> paths, Game game, List<Point> destinations, List<PathNode<Point>> completedPaths, HashSet<Point> visited)
		{
			var currentPath = paths.Dequeue();
			var nextPositions = GetAdjacentPositions(currentPath.Value)
				.Where(p => !visited.Contains(p) &&
				            game.Board[p.Y, p.X] == '.' );
			foreach (var nextPosition in nextPositions)
			{
				visited.Add(nextPosition);
				var newPath = currentPath.Travel(nextPosition);
				if (destinations.Contains(newPath.Value))
				{
					completedPaths.Add(newPath);
				}
				else
				{
					paths.Enqueue(newPath);
				}
			}
		}

		private static List<Point> GetDestinations(Unit currentUnit, Game game) => game.GetEnemies(currentUnit).SelectMany(e => GetAdjacentPositions(e.Position))
			.Where(p => game.Board[p.Y, p.X] == '.')
			.ToList();

		private static void Attack(Unit currentUnit, Game game)
		{
			var toAttack = GetEnemyAdjacent(currentUnit, game);
			if(toAttack == null) return;
			toAttack.HealthPoints = Math.Max(0, toAttack.HealthPoints - currentUnit.AttackPoints);
			if (!toAttack.Alive)
			{
				game.GetEnemies(currentUnit).Remove(toAttack);
				game.Board[toAttack.Position.Y, toAttack.Position.X] = '.';
			}
		}

		private static Point[] GetAdjacentPositions(Point point) => new[] {new Point(point.X, point.Y - 1), new Point(point.X-1, point.Y), new Point(point.X + 1, point.Y), new Point(point.X, point.Y + 1) };

		private static Unit GetEnemyAdjacent(Unit currentUnit, Game game)
		{
			var closeEnemies = GetAdjacentPositions(currentUnit.Position)
				.Select(adjacentPosition => game.GetEnemies(currentUnit).FirstOrDefault(e => e.Position == adjacentPosition))
				.Where(e => e != null)
				.ToList();
			if (!closeEnemies.Any()) return null;
			var minHp = closeEnemies.Select(e => e.HealthPoints).Min();
			var minHpEnemies = closeEnemies.Where(e => e.HealthPoints == minHp).ToList();
			return GetAdjacentPositions(currentUnit.Position)
				.Select(ap => minHpEnemies.FirstOrDefault(e => e.Position == ap))
				.First(e => e != null);
		}

		private static void Debug(Game game)
		{
			Console.WriteLine($"Round: {game.Round}");
			for (var y = 0; y < game.Board.GetLength(0); ++y)
			{
				Console.Write(Environment.NewLine);
				for (var x = 0; x < game.Board.GetLength(1); ++x)
				{
					Console.Write(game.Board[y, x]);
				}
			}
			Console.WriteLine();

			foreach (var goblin in game.Goblins.OrderBy(g => g.Position.Y).ThenBy(g => g.Position.X))
			{
				Console.WriteLine($"G({goblin.HealthPoints})");
			}
			foreach (var elf in game.Elves.OrderBy(g => g.Position.Y).ThenBy(g => g.Position.X))
			{
				Console.WriteLine($"E({elf.HealthPoints})");
			}

			Console.ReadLine();
		}

		private string GetScore(Game game)
		{
			long hp = game.Elves.Select(e => e.HealthPoints).Sum() + game.Goblins.Select(e => e.HealthPoints).Sum();
			return (hp * (long)game.Round).ToString();
		}

		private static List<Game> ParseData(IEnumerable<string> data)
		{
			var toReturn = new List<Game>();
			var accrue = new List<string>();
			foreach (var line in data)
			{
				if (string.IsNullOrWhiteSpace(line))
				{
					toReturn.Add(new Game(accrue));
					accrue = new List<string>();
				}
				else
				{
					accrue.Add(line);
				}
			}
			toReturn.Add(new Game(accrue));
			return toReturn;
		}

		public class Game
		{
			public char[,] Board { get; set; }
			public List<Unit> Elves { get; set; }
			public List<Unit> Goblins { get; set; }
			public int Round { get; set; }
			public List<Unit> AllUnits => Elves.Concat(Goblins).ToList();
			public List<Unit> GetEnemies(Unit unit) => Elves.Contains(unit) ? Goblins : Elves;

			public Game(List<string> board)
			{
				Round = 0;
				Elves = new List<Unit>();
				Goblins = new List<Unit>();
				Board = new char[board.Count,board.First().Length];
				for (var y = 0; y < Board.GetLength(0); ++y)
				{
					for (var x = 0; x < Board.GetLength(1); ++x)
					{
						Board[y, x] = board[y][x];
						if (Board[y, x] == 'G') Goblins.Add(new Unit(x, y));
						if (Board[y, x] == 'E') Elves.Add(new Unit(x, y));
					}
				}
			}
		}

		public class PathNode<T>
		{
			public PathNode<T> Prev { get; set; }
			public T Value { get; set; }
			public int Distance => Prev?.Distance + 1 ?? 1;
			public PathNode<T> SecondStep => 
				Prev == null ? null :
				Prev.Prev == null ? this :
				Prev.SecondStep;

			public PathNode<T> Travel(T value)
			{
				return new PathNode<T>
				{
					Prev = this,
					Value = value
				};
			}
		}

		public class Unit
		{
			public Point Position { get; set; }
			public int AttackPoints { get; set; }
			public int HealthPoints { get; set; }
			public bool Alive => HealthPoints > 0;

			public Unit(int x, int y)
			{
				AttackPoints = 3;
				HealthPoints = 200;
				Position = new Point(x, y);
			}
		}
	}
}
