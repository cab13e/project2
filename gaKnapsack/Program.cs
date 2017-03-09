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

		pop.Sort((a,b) =>Darwin(a).CompareTo(Darwin(b)));
		//Console.WriteLine("finished Genesis");
		return pop;
	}

	public static double Darwin(string chrom)
	{
		
		double fitness = 0;
		int i = 0;
		foreach(char c in chrom)
		{
			if(_globals.knapsack[i].cost < _globals.capacity)
				fitness += _globals.knapsack[i].value;
			else
			{
				fitness = 0;
				break;
			}
		}
		_globals._fitCount++;
		return fitness;
	}

	public static string logHasChild(string par1, string par2)
	{
		Random rand = new Random();

		var cross = rand.Next(0, par1.Length);

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
					thing = '0';
				}
			}
		}
		//Console.WriteLine("had a child");
		return child;
	}

	public static List<string> Life(List<string> population, Stopwatch time)
	{
		Random rand = new Random();
		var baseInterval = new TimeSpan(0, 10, 0);
		int apocalypseNow = 0;
		string par1, par2;
		int p1, p2;
		while (true)
		{
			p1 = Math.Max(rand.Next(0, 99), rand.Next(0, 99));
			p2 = Math.Max(rand.Next(0, 99), rand.Next(0, 99));

			par1 = population[p1];
			par2 = population[p2];

			string child = logHasChild(par1, par2);
			population.Add(child);

			population.Sort((a, b) => Darwin(a).CompareTo(Darwin(b)));

			if (population[0] == population[population.Count()-1])
			{
				var temp = new List<string>();

				foreach (var thing in population.Skip(0))
				{
					temp.Add(population[0]);
				}

				Catastrophe(temp);
				Console.WriteLine("Cataclysm occured");
				//temp = Genesis(temp.Count);

				var temp2 = population[0];
				population.Clear();
				population.Add(temp2);
				foreach (var thing in temp)
				{
					population.Add(thing);
				}
				apocalypseNow++;
			}
			if (apocalypseNow >= 3)
				return population;
			if (TimeSpan.Compare(time.Elapsed, baseInterval) == 1)
				return population;
		}
	}


	public static List<string> Catastrophe(List<string> pop)
	{
		Random rand = new Random();
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

		var pop = Genesis(_globals.knapsack.Count);

		time.Start();
		var endGame = Life(pop,time);
		time.Stop();


		//var crossover = new Crossover(1.0, true, CrossoverType.SinglePoint, ReplacementMethod.DeleteLast);

		//var mutation = new BinaryMutate(.05, true);

		//var algo = new GeneticAlgorithm(pop, EvaluateFitness);
		//algo.Run(TerminateAlgorithm);

		//algo.Operators.Add(crossover);
		//algo.Operators.Add(mutation);

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
	}
}
