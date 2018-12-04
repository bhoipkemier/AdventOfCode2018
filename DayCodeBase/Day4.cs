using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.DayCodeBase
{
	public class Day4: DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData()
				.Select(ParseInput)
				.OrderBy(i => i.Date);
			var sleepTime = GetSleepSchedule(data);
			var guardId = GetMostSleep(sleepTime);
			var minute = GetMostOccurMinute(sleepTime[guardId]).Item1;
			return (guardId * minute).ToString();
		}

		public override string Problem2()
		{
			var data = GetData()
				.Select(ParseInput)
				.OrderBy(i => i.Date);
			var sleepTime = GetSleepSchedule(data);
			var mostOften = GetMostOftenCounts(sleepTime).OrderByDescending(m => m.Count).First();
			return (mostOften.Guard * mostOften.Minute).ToString();
		}

		private static List<MostOften> GetMostOftenCounts(Dictionary<int, List<Tuple<DateTime, TimeSpan>>> sleepTime)
		{
			return sleepTime.Keys
				.Select(guardId => new {guardId, t = GetMostOccurMinute(sleepTime[guardId])})
				.Select(o => new MostOften {Count = o.t.Item2, Minute = o.t.Item1, Guard = o.guardId})
				.ToList();
		}

		struct MostOften
		{
			public int Guard { get; set; }
			public int Minute { get; set; }
			public int Count { get; set; }
		}


		private static Tuple<int, int> GetMostOccurMinute(List<Tuple<DateTime, TimeSpan>> tuples)
		{
			var minuteFreq = new int[60];
			foreach (var sleepTime in tuples)
			{
				for (var dur = 0; dur < sleepTime.Item2.Minutes; ++dur)
				{
					var min = sleepTime.Item1.Minute + dur;
					minuteFreq[min] += 1;
				}
			}

			var minute = 0;
			for(var i = 0; i<60; ++i)
				if (minuteFreq[i] > minuteFreq[minute])
				{
					minute = i;
				}

			return new Tuple<int, int>(minute, minuteFreq[minute]);
		}

		private int GetMostSleep(Dictionary<int, List<Tuple<DateTime, TimeSpan>>> sleepTime)
		{
			return sleepTime.Keys.OrderByDescending(k => GetMinutesAsleep(sleepTime[k])).First();
		}

		private int GetMinutesAsleep(List<Tuple<DateTime, TimeSpan>> tuples)
		{
			return tuples.Select(t => t.Item2.Minutes).Sum();
		}

		private static Dictionary<int, List<Tuple<DateTime, TimeSpan>>> GetSleepSchedule(IEnumerable<InputFormat> data)
		{
			var toReturn = new Dictionary<int, List<Tuple<DateTime, TimeSpan>>>();
			var guardId = int.MinValue;
			var sleepTime = DateTime.MinValue;
			foreach (var line in data)
			{
				if (line.Event.Contains("falls asleep"))
				{
					sleepTime = line.Date;
				}
				else if(line.Event.Contains("wakes up"))
				{
					if(!toReturn.ContainsKey(guardId)) toReturn.Add(guardId, new List<Tuple<DateTime, TimeSpan>>());
					toReturn[guardId].Add(new Tuple<DateTime, TimeSpan>(sleepTime, line.Date - sleepTime));
				}else
				{
					var match = Regex.Matches(line.Event, @"Guard\s\#(\d+)\sbegins\sshift")[0];
					guardId = int.Parse(match.Groups[1].Value);
				}
			}
			return toReturn;
		}

		struct InputFormat
		{
			public DateTime Date { get; set; }
			public string Event { get; set; }
		}

		private static InputFormat ParseInput(string data)
		{
			var pattern = @"\[(.*)]\s(.*)";
			var match = Regex.Matches(data, pattern)[0];
			return new InputFormat
			{
				Date = DateTime.Parse(match.Groups[1].Value),
				Event = match.Groups[2].Value
			}; 
		}
	}
}
