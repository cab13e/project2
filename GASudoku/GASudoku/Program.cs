using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace GASudoku
{
	public struct puzzleBox
	{
		public int[][] puzzle;
	}

	// be able to read in a puzzle, for now will just have 1 static puzzle



	public static List<string> Genesis(int length)
	{
		Random rand = new Random();
		var pop = new List<string>();
		
		int j = 0;
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

		pop.Sort((a,b) =>Darwin(a).CompareTo(Darwin(b)));
		//Console.WriteLine("finished Genesis");
		return pop;	
	}










	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
		}
	}
}
