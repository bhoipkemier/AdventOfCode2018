using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day13 : DayCodeBase
	{
		public enum Heading
		{
			Up, Right, Down, Left
		}

		public override string Problem1()
		{
			var data = GetData().ToList();
			var carts = LoadCarts(data);
			var track = LoadTrack(data, carts);
			var collisionPoint = Collide(track, carts);
			return $"{collisionPoint.X},{collisionPoint.Y}";
		}

		public override string Problem2()
		{
			var data = GetData().ToList();
			var carts = LoadCarts(data);
			var track = LoadTrack(data, carts);
			var remainingPosition = CollideAll(track, carts);
			return $"{remainingPosition.X},{remainingPosition.Y}";
		}

		private Point CollideAll(char[,] track, List<Cart> carts)
		{
			while (carts.Count(c => !c.Crashed) > 1)
			{
				List<Cart> orderToProcess = carts.Where(c => !c.Crashed).OrderBy(c => c.Position.Y).ThenBy(c => c.Position.X).ToList();
				foreach (var cart in orderToProcess)
				{
					if (!cart.Crashed)
					{
						Move(cart, track);
						var collidedWith = carts.FirstOrDefault(c => !c.Crashed && c != cart && c.Position == cart.Position);
						if (collidedWith != null)
						{
							cart.Crashed = true;
							collidedWith.Crashed = true;
						}
					}
				}
			}
			return carts.First(c => !c.Crashed).Position;
		}

		private Point Collide(char[,] track, List<Cart> carts)
		{
			while (true)
			{
				List<Cart> orderToProcess = carts.OrderBy(c => c.Position.Y).ThenBy(c => c.Position.X).ToList();
				foreach (var cart in orderToProcess)
				{
					Move(cart, track);
					//Debug(track,carts);
					if (carts.Any(c => c != cart && c.Position == cart.Position)) return cart.Position;
				}
			}
		}

		private void Debug(char[,] track, List<Cart> carts)
		{
			for(var y = 0; y < track.GetLength(0); ++y)
			{
				Console.Write("\r\n");
				for (var x = 0; x < track.GetLength(1); ++x)
				{
					var cart = carts.FirstOrDefault(c => !c.Crashed && c.Position.X == x && c.Position.Y == y);
					Console.Write(cart == null ? track[y,x] : "^>v<"[(int)cart.Heading]);
				}
			}

			Console.Write("\r\n\r\n\r\n");
		}

		private void Move(Cart cart, char[,] track)
		{
			var origHeading = cart.Heading;
			cart.Position = cart.Heading == Heading.Up ? new Point(cart.Position.X, cart.Position.Y - 1) :
				cart.Heading == Heading.Down ? new Point(cart.Position.X, cart.Position.Y + 1) :
				cart.Heading == Heading.Left ? new Point(cart.Position.X - 1, cart.Position.Y) :
				cart.Heading == Heading.Right ? new Point(cart.Position.X + 1, cart.Position.Y) : throw new NotImplementedException();
			var newTrack = track[cart.Position.Y, cart.Position.X];
			if ("-|".Contains(newTrack))
			{
				//NoChangeNeeded
			}
			else if ('/' == newTrack && origHeading == Heading.Right)
			{
				cart.Heading = Heading.Up;
			}
			else if ('/' == newTrack && origHeading == Heading.Down)
			{
				cart.Heading = Heading.Left;
			}
			else if ('/' == newTrack && origHeading == Heading.Up)
			{
				cart.Heading = Heading.Right;
			}
			else if ('/' == newTrack && origHeading == Heading.Left)
			{
				cart.Heading = Heading.Down;
			}
			else if ('\\' == newTrack && origHeading == Heading.Right)
			{
				cart.Heading = Heading.Down;
			}
			else if ('\\' == newTrack && origHeading == Heading.Down)
			{
				cart.Heading = Heading.Right;
			}
			else if ('\\' == newTrack && origHeading == Heading.Up)
			{
				cart.Heading = Heading.Left;
			}
			else if ('\\' == newTrack && origHeading == Heading.Left)
			{
				cart.Heading = Heading.Up;
			}
			else if ('+' == newTrack)
			{
				var turn = cart.TurnOrder; // left, straight, right
				cart.TurnOrder = (cart.TurnOrder + 1) % 3;
				if (turn == 0)
				{
					cart.Heading = origHeading == Heading.Up ? Heading.Left :
						origHeading == Heading.Right ? Heading.Up :
						origHeading == Heading.Down ? Heading.Right :
						origHeading == Heading.Left ? Heading.Down : throw new NotImplementedException();
				}
				if (turn == 2)
				{
					cart.Heading = origHeading == Heading.Up ? Heading.Right :
						origHeading == Heading.Right ? Heading.Down :
						origHeading == Heading.Down ? Heading.Left :
						origHeading == Heading.Left ? Heading.Up : throw new NotImplementedException();
				}
			}
		}

		private static char[,] LoadTrack(List<string> data, List<Cart> carts)
		{
			var toReturn = new char[data.Count, data.First().Length];
			for (var y = 0; y < data.Count; ++y)
			{
				for (var x = 0; x < data[y].Length; ++x)
				{
					toReturn[y, x] = data[y][x];
				}
			}

			foreach (var cart in carts)
			{
				toReturn[cart.Position.Y, cart.Position.X] = GetReplacementTrack(toReturn, cart.Position);
			}
			return toReturn;
		}

		private static char GetReplacementTrack(char[,] track, Point cartPosition)
		{
			char above = cartPosition.Y == 0 ? ' ' : track[cartPosition.Y - 1, cartPosition.X];
			char below = cartPosition.Y == track.GetLength(0) - 1 ? ' ' : track[cartPosition.Y + 1, cartPosition.X];
			char right = cartPosition.X == track.GetLength(1) - 1 ? ' ' : track[cartPosition.Y, cartPosition.X + 1];
			char left = cartPosition.X == 0 ? ' ' : track[cartPosition.Y, cartPosition.X - 1];

			if (@"-+/\".Contains(left) && @"-+/\".Contains(right))
			{
				return '-';
			}

			if (@"|+/\".Contains(above) && @"|+/\".Contains(below))
			{
				return '|';
			}
			throw new NotImplementedException();
		}

		private static List<Cart> LoadCarts(List<string> data)
		{
			var toReturn = new List<Cart>();
			for (var y = 0; y < data.Count; ++y)
			{
				for (var x = 0; x < data[y].Length; ++x)
				{
					char cell = data[y][x];
					if ("<>v^".Contains(cell))
					{
						toReturn.Add(new Cart
						{
							Heading = (Heading)"^>v<".IndexOf(cell),
							Position = new Point(x,y)
						});
					}
				}
			}
			return toReturn;
		}

		public class Cart
		{
			public int TurnOrder { get; set; }
			public Heading Heading { get; set; }
			public Point Position { get; set; }
			public bool Crashed { get; set; }
		}
	}
}
