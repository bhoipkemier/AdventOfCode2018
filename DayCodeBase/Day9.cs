using System;
using System.Linq;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day9 : DayCodeBase
	{
		public override string Problem1() => GetOutputWithMultiplier();

		public override string Problem2() => GetOutputWithMultiplier(100);

		private string GetOutputWithMultiplier(int multiplier = 1)
		{
			var input = GetData(1).Select(ParseData);
			return string.Join("\r\n",
				input.Select(i => new Game(i.Item1, i.Item2 * multiplier))
					.Select(g => g.PlayGame())
					.ToArray());
		}

		private Tuple<long, long> ParseData(string data)
		{
			var splitData = data.Split(' ');
			return new Tuple<long, long>(long.Parse(splitData[0]), long.Parse(splitData[6]));
		}

		private class Game
		{
			private readonly LoopList<long> _state = new LoopList<long>(0);
			private readonly long _numMarbles = 0;
			private readonly long[] _playerScores;
			private long _lastMarblePlayed = 0;

			public Game(long players, long marbles)
			{
				_numMarbles = marbles;
				_playerScores = new long[players];
			}

			public string PlayGame()
			{
				while (_lastMarblePlayed < _numMarbles)
				{
					Move();
				}

				return _playerScores.Max().ToString();
			}

			private void Move()
			{
				_lastMarblePlayed++;
				if (_lastMarblePlayed % 23 == 0)
				{
					var player = _lastMarblePlayed % _playerScores.Length;
					_state.Move(-7);
					_playerScores[player] += _lastMarblePlayed + _state.Value;
					_state.Remove();
				}
				else
				{
					_state.Move(2);
					_state.Insert(_lastMarblePlayed);
				}
			}


			private class LoopList<T>
			{
				private Item<T> _current;
				public T Value => _current.Value;

				public LoopList(T init)
				{
					_current = new Item<T>
					{
						Value = init
					};
					_current.Before = _current;
					_current.After = _current;
				}

				public string Debug()
				{
					var cur = _current;
					var toReturn = $"{cur.Value} ";
					for (cur = _current.After; cur != _current; cur = cur.After)
					{
						toReturn += $"{cur.Value} ";
					}

					return toReturn;
				}

				public void Insert(T val)
				{
					var prev = _current.Before;
					var next = _current;
					_current = new Item<T> {Value = val, Before = prev, After = next};
					prev.After = _current;
					next.Before = _current;
				}

				public void Remove()
				{
					var prev = _current.Before;
					var next = _current.After;
					prev.After = next;
					next.Before = prev;
					_current = next;
				}

				public void Move(long offset)
				{
					if (offset > 0)
					{
						for (var i = 0; i < offset; ++i)
						{
							_current = _current.After;
						}
					}
					else
					{
						for (var i = 0; i > offset; --i)
						{
							_current = _current.Before;
						}
					}
				}

				public class Item<T>
				{
					public Item<T> Before;
					public Item<T> After;
					public T Value;
				}
			}


		}
	}
}
