using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace GASudoku
{
	class GA
	{
		public static List<int[,]> Genesis(int length)
		{
			Random rand = new Random();
			var pop = new List<int[,]>();

			for (int i = 0; i < 100; i++)
			{
				var chrom = _globals._puzzle;

				for (int j = 0; j < chrom.GetLength(0); j++)
				{
					for (int m = 0; m < chrom.GetLength(1); m++)
					{
						if (chrom[j, m] == 0 && !_globals._changes[j, m])
						{
							int next = rand.Next(1, 6);
							chrom[j, m] = next;
						}
					}
				}
				//Console.WriteLine("creating " + chrom);
				pop.Add(chrom);
			}
			pop.Sort((a, b) => Darwin(a).CompareTo(Darwin(b)));

			//Console.WriteLine("finished Genesis");
			return pop;
		}

		public static bool hasMore(int[] arrayToCheck)
		{
			var duplicates = arrayToCheck
			 .GroupBy(s => s)
			 .Where(g => g.Count() > 1)
			 .Select(g => g.Key);

			return (duplicates.Count() > 0);
		}

		public static int Darwin(int[,] puz)
		{
			int totalVal = 0;

			for (int i = 0; i < puz.GetLength(0); i++)
			{
				for (int j = 0; j < puz.GetLength(1); j++)
				{
					totalVal += puz[i, j];
				}
			}

			var r1 = new int[] { puz[0, 0], puz[0, 1], puz[0, 2], puz[0, 3], puz[0, 4], puz[0, 5] };
			var r2 = new int[] { puz[1, 0], puz[1, 1], puz[1, 2], puz[1, 3], puz[1, 4], puz[1, 5] };
			var r3 = new int[] { puz[2, 0], puz[2, 1], puz[2, 2], puz[2, 3], puz[2, 4], puz[2, 5] };
			var r4 = new int[] { puz[3, 0], puz[3, 1], puz[3, 2], puz[3, 3], puz[3, 4], puz[3, 5] };
			var r5 = new int[] { puz[4, 0], puz[4, 1], puz[4, 2], puz[4, 3], puz[4, 4], puz[4, 5] };
			var r6 = new int[] { puz[5, 0], puz[5, 1], puz[5, 2], puz[5, 3], puz[5, 4], puz[5, 5] };

			var c1 = new int[] { puz[0, 0], puz[1, 0], puz[2, 0], puz[3, 0], puz[4, 0], puz[5, 0] };
			var c2 = new int[] { puz[0, 1], puz[1, 1], puz[2, 1], puz[3, 1], puz[4, 1], puz[5, 1] };
			var c3 = new int[] { puz[0, 2], puz[1, 2], puz[2, 2], puz[3, 2], puz[4, 2], puz[5, 2] };
			var c4 = new int[] { puz[0, 3], puz[1, 3], puz[2, 3], puz[3, 3], puz[4, 3], puz[5, 3] };
			var c5 = new int[] { puz[0, 4], puz[1, 4], puz[2, 4], puz[3, 4], puz[4, 4], puz[5, 4] };
			var c6 = new int[] { puz[0, 5], puz[1, 5], puz[2, 5], puz[3, 5], puz[4, 5], puz[5, 5] };

			var b1 = new int[] { puz[0, 0], puz[0, 1], puz[0, 2], puz[1, 0], puz[1, 1], puz[1, 2] };
			var b2 = new int[] { puz[0, 3], puz[0, 4], puz[0, 5], puz[1, 3], puz[1, 4], puz[1, 5] };
			var b3 = new int[] { puz[2, 0], puz[2, 1], puz[2, 2], puz[3, 0], puz[3, 1], puz[3, 2] };
			var b4 = new int[] { puz[2, 3], puz[2, 4], puz[2, 5], puz[3, 3], puz[3, 4], puz[3, 5] };
			var b5 = new int[] { puz[4, 0], puz[4, 1], puz[4, 2], puz[5, 0], puz[5, 1], puz[5, 2] };
			var b6 = new int[] { puz[4, 3], puz[4, 4], puz[4, 5], puz[5, 3], puz[5, 4], puz[5, 5] };


			if (hasMore(r1) || hasMore(r2) || hasMore(r3) || hasMore(r4) || hasMore(r5) || hasMore(r6) || hasMore(c1) || hasMore(c2) || hasMore(c3) || hasMore(c4) || hasMore(c5) || hasMore(c6) || hasMore(b1) || hasMore(b2) || hasMore(b3) || hasMore(b4) || hasMore(b5) || hasMore(b6))
				totalVal = 0;

			return totalVal;
		}


		public static int[,] babies(int[,] par1, int[,] par2)
		{
			Random rand = new Random();

			var cross = rand.Next(0, par1.GetLength(0 + 1));

			var child = new int[par1.GetLength(0), par1.GetLength(1)];

			for (int i = 0; i < child.GetLength(0); i++)
			{
				for (int j = 0; j < child.GetLength(1); j++)
				{
					
					if (i < cross)
						child[i, j] = par1[i, j];
					else
						child[i, j] = par2[i, j];
				}
			}
			/*for (int i = 0; i < child.GetLength(0); i++)
			{
				for (int j = 0; j < child.GetLength(1); j++)
				{
					if (rand.Next(1, 200) == 1 )
					{
						int next = rand.Next(1, 6);
						child[i, j] = next;
					}
				}
			}*/
			return child;
		}

		public static List<int[,]> Apocalypse(List<int[,]> pop)
		{
			Random rand = new Random();

			for (int i = 0; i < pop.Count; i++)
			{
				var newbie = pop[i];
				for (int j = 0; j < newbie.GetLength(0); j++)
				{
					for (int m = 0; m < newbie.GetLength(1); m++)
					{
						if (!_globals._changes[j, m])
						{
							if (rand.Next(1, 5) == 1)
							{
								newbie[j, m] = rand.Next(1, 6);
							}
						}
					}
				}
			}
			return pop;
		}

		public static List<int[,]> Life(List<int[,]> pop, Stopwatch time)
		{
			Random rand = new Random();
			var baseInterval = new TimeSpan(0, 5, 0);
			int apocalypseNow = 0;
			int p1, p2;


			while (Darwin(pop[0]) == 0)
			{
				p1 = Math.Max(rand.Next(0, 99), rand.Next(0, 99));
				p2 = Math.Max(rand.Next(0, 99), rand.Next(0, 99));

				var par1 = pop[p1];
				var par2 = pop[p2];

				var child = babies(par1, par2);
				pop.Add(child);

				pop.Sort((a, b) => Darwin(a).CompareTo(Darwin(b)));
				if (Darwin(pop[0]) == 126)
					return pop;
				
				pop.RemoveAt(pop.Count - 1);

				var equal = pop[0].Rank == pop[pop.Count-1].Rank && Enumerable.Range(0, pop[0].Rank).All(dimension => pop[0].GetLength(dimension) == pop[pop.Count-1].GetLength(dimension)) && pop[0].Cast<int>().SequenceEqual(pop[pop.Count-1].Cast<int>());

				if (equal)
				{
					var temp = new List<int[,]>();
					//int xs = 1;

					foreach (var thing in pop.Skip(0))
					{
						temp.Add(pop[1]);
						//xs++;
					}

					Apocalypse(temp);
					Console.WriteLine(apocalypseNow);
					//temp = Genesis(temp.Count);

					var temp2 = pop[0];
					pop.Clear();
					pop.Add(temp2);
					foreach (var thing in temp)
					{
						pop.Add(thing);
					}
					apocalypseNow++;
				}
				if (TimeSpan.Compare(time.Elapsed, baseInterval) == 1)
					return pop;
			}
			return pop;
		}

		public static class _globals
		{
			public static int[,] _puzzle;
			public static bool[,] _changes;
		}

		public static void Main()
		{
			//GA phase = new GA();

			_globals._puzzle = new int[6, 6];
			_globals._changes = new bool[6, 6];
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					_globals._puzzle[i, j] = 0;
					_globals._changes[i, j] = false;
				}
			}
			_globals._puzzle[0, 2] = 3;
			_globals._puzzle[0, 4] = 1;
			_globals._puzzle[1, 0] = 5;
			_globals._puzzle[1, 1] = 6;
			_globals._puzzle[1, 3] = 3;
			_globals._puzzle[1, 4] = 2;
			_globals._puzzle[2, 1] = 5;
			_globals._puzzle[2, 2] = 4;
			_globals._puzzle[2, 3] = 2;
			_globals._puzzle[2, 5] = 3;
			_globals._puzzle[3, 0] = 2;
			_globals._puzzle[3, 2] = 6;
			_globals._puzzle[3, 3] = 4;
			_globals._puzzle[3, 4] = 5;
			_globals._puzzle[4, 1] = 1;
			_globals._puzzle[4, 2] = 2;
			_globals._puzzle[4, 4] = 4;
			_globals._puzzle[4, 5] = 5;
			_globals._puzzle[5, 1] = 4;
			_globals._puzzle[5, 3] = 1;

			_globals._changes[0, 2] = true;
			_globals._changes[0, 4] = true;
			_globals._changes[1, 0] = true;
			_globals._changes[1, 1] = true;
			_globals._changes[1, 3] = true;
			_globals._changes[1, 4] = true;
			_globals._changes[2, 1] = true;
			_globals._changes[2, 2] = true;
			_globals._changes[2, 3] = true;
			_globals._changes[2, 5] = true;
			_globals._changes[3, 0] = true;
			_globals._changes[3, 2] = true;
			_globals._changes[3, 3] = true;
			_globals._changes[3, 4] = true;
			_globals._changes[4, 1] = true;
			_globals._changes[4, 2] = true;
			_globals._changes[4, 4] = true;
			_globals._changes[4, 5] = true;
			_globals._changes[5, 1] = true;
			_globals._changes[5, 3] = true;

			for (int i = 0; i < _globals._puzzle.GetLength(0); i++)
			{
				for (int j = 0; j < _globals._puzzle.GetLength(1); j++)
					Console.Write(_globals._puzzle[i, j] + ",");
				Console.WriteLine();
			}

			Console.WriteLine("\n\n\n\n\n");


			var pop = Genesis(_globals._puzzle.GetLength(0 + 1));

			Stopwatch time = new Stopwatch();

			time.Start();
			var endOfTime = Life(pop, time);
			time.Stop();

			var final = endOfTime[0];
			for (int i = 0; i < final.GetLength(0); i++)
			{
				for (int j = 0; j < final.GetLength(1); j++)
					Console.Write(final[i, j] + ",");
				Console.WriteLine();
			}

			Console.WriteLine(Darwin(final));

			Console.Beep();
			Console.Beep();
		}
	}

}