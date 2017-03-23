using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
//using GAF;
//using GAF.Operators;

class GA
{
	public struct item
	{
		public string name;
		public double cost;
		public double value;
	}

	public Tuple<int, List<item>> CSVin(string filename)
	{
		List<item> knapsack = new List<item>();

		string[] lines = System.IO.File.ReadAllLines(@filename);

		int cap;
		cap = int.Parse(lines[0]);

		foreach (var stuff in lines.Skip(1))
		{
			var temp = stuff.Split(',');

			item thing;

			thing.name = temp[0];
			thing.cost = int.Parse(temp[1]);
			thing.value = int.Parse(temp[2]);
			knapsack.Add(thing);
		}
		//Console.WriteLine("finished");
		return Tuple.Create(cap, knapsack);
	}

	public static List<string> Genesis(int length)
	{
		Random rand = new Random();
		var pop = new List<string>(); 

		int j = 0;
		for (int i = 0; i < 100; i++)	
		{
			j = 0;
			string chrom = "";
			while (j < length)
			{
				int next = rand.Next(2);
				if (next == 1)
					chrom += "1";
				else
					chrom += "0";
				j++;
			}
			//Console.WriteLine("creating " + chrom);
			pop.Add(chrom);
		}

		pop.Sort((a,b) => -1 * Darwin(a).CompareTo(Darwin(b)));
		//foreach (var blep in pop)
			//Console.WriteLine("{0} {1}", blep, Darwin(blep));
		//Console.WriteLine("finished Genesis");
		return pop;
	}

	public static double Darwin(string chrom)
	{
		double cost = 0, val = 0;
		int i = 0;
		foreach(char c in chrom)
		{
			if (c == '1')
			{
				val += _globals.knapsack[i].value;
				cost += _globals.knapsack[i].cost;
			}
			i++;
		}

		_globals._fitCount++;

		return cost > _globals.capacity ? 0 : val;
	}

	public static string logHasChild(string par1, string par2)
	{
		Random rand = new Random();

		var cross = rand.Next(1, par1.Length);

		var child = "";

		for (int i = 0; i < par1.Length; i++)
		{
			if (i < cross)
				child += par1[i];
			else
				child += par2[i];
		}

		for (int i = 0; i < child.Length; i++)
		{
			char thing = child[i];
			if (rand.Next(1, 200) == 1)
			{
				if (thing == '1')
					thing = '0';
				else
				{
					thing = '1';
				}
			}
		}
		//Console.WriteLine(child);
		return child;
	}

	public static List<string> Life(List<string> population, Stopwatch time)
	{
		Random rand = new Random();
		var baseInterval = new TimeSpan(0, 10, 0);
		string par1, par2;
		int p1, p2;
		while (true)
		{
			p1 = Math.Max(rand.Next(100), rand.Next(100));
			p2 = Math.Max(rand.Next(100), rand.Next(100));

			par1 = population[p1];
			par2 = population[p2];

			string child = logHasChild(par1, par2);
			population.Add(child);

			population.Sort((a, b) => -1 * Darwin(a).CompareTo(Darwin(b)));
			population.RemoveAt(100);

			//Console.WriteLine(_globals.apocalypseNow);

			if (string.Equals(population[0],population[99]))
			{
				Catastrophe(population);
			}

			if (_globals.apocalypseNow >= 3)
				return population;
			if (TimeSpan.Compare(time.Elapsed, baseInterval) == 1)
				return population;
		}
	}


	public static List<string> Catastrophe(List<string> pop)
	{
		Random rand = new Random();

		string hold = pop[0];

		//Console.WriteLine(hold + "\n\n");

		if (string.Equals(hold,_globals.survivor))
			_globals.apocalypseNow++;
		else
			_globals.apocalypseNow = 0;

		foreach(var thing in pop.Skip(1))
		{
			if (!string.Equals(thing,hold))
				return pop;
		}


		for (int i = 0; i < pop.Count; i++)
		{
			var newbie = pop[i];
			for (int j = 0; j < newbie.Length; j++)
			{
				char thing = newbie[j];
				if (rand.Next(1, 5) == 1)
				{
					if (thing == '1')
						thing = '0';
					else
						thing = '0';
				}
			}
		}
		_globals.survivor = hold;
		_globals._fitCount++;

		pop.Sort((a, b) => -1 * Darwin(a).CompareTo(Darwin(b)));

		return pop;
	}

	/*
	public static bool TerminateAlgorithm(Population population, int currentGeneration, long currentEvaluation)
	{
		return currentGeneration > 100;
	}*/

	public static class _globals
	{
		public static List<item> knapsack;
		public static double capacity;
		public static int _fitCount;
		public static int apocalypseNow;
		public static string survivor;
	}

	public static void Main()
	{
		GA phase = new GA();
		//List<item> knapsack = new List<item>();

		string filename;
		Console.Write("enter the filename: ");
		filename = Console.ReadLine();

		Stopwatch time = new Stopwatch();

		Tuple<int, List<item>> tuple = phase.CSVin(filename);

		_globals.capacity = tuple.Item1;
		_globals.knapsack = tuple.Item2;

		//Console.WriteLine(_globals.capacity);

		var pop = Genesis(_globals.knapsack.Count);
		//Console.WriteLine(pop[0] + "\n\n");

		time.Start();
		var endGame = Life(pop,time);
		time.Stop();

		var check = endGame[0];
		double totalCost=0, totalvalue=0;
		for (int i = 0; i < endGame[0].Length; i++)
		{
			var thing = check[i];
			if(thing == '1')
			{
				totalvalue += _globals.knapsack[i].value;
				totalCost += _globals.knapsack[i].cost;
			}
		}

		Console.WriteLine("Fitness function called " + _globals._fitCount + " compared with an exhaustive search making " + Math.Pow(2,_globals.knapsack.Count) + " comparisons");

		Console.WriteLine("final result = " + endGame[0] + " with total cost of " + totalCost + " and total value of " + totalvalue + "\nAfter " + time.Elapsed);

		Console.Beep();
		Console.Beep();
	}
}
