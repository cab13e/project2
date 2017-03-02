using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using GAF;
using GAF.Operators;

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
		return Tuple.Create(cap, knapsack);
	}




	public static void Main()
	{
		GA phase = new GA();
		List<item> knapsack = new List<item>();

		string filename;
		Console.Write("enter the filename: ");
		filename = Console.ReadLine();

		Tuple<int, List<item>> tuple = phase.CSVin(filename);

		int capacity = tuple.Item1;
		knapsack = tuple.Item2;

	}
}
