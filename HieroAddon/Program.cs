using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HieroConv
{
	class HieroConv
	{
		private static List<string> fontLines;
		private static List<string> xmlLines;
		private static List<string> processedLines;
		private static string fontPath;
		static void Main(string[] args)
		{
			if (args.Length == 1)
			{
				fontPath = args[0];
				if (fontPath.Replace("\"", "").Substring(fontPath.Length - 3) == "fnt")
				{
					processHiero(fontPath);
					writeHiero(xmlLines);
				}
				else
				{
					Console.WriteLine("You must provide an .fnt file, not some random one..");
					return;
				}
			}
			else if (args.Length == 2 && args[1] == "-p")
			{
				fontPath = args[0];
				if (fontPath.Replace("\"", "").Substring(fontPath.Length - 3) == "fnt")
				{
					processHiero(fontPath);
				}
				else
				{
					Console.WriteLine("You must provide an .fnt file, not some random one..");
					return;
				}
			}
			else
			{
				Console.WriteLine("Hiero converter v. " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
				Console.WriteLine("You have to pass an .fnt file path and (optional) preview argument (-p).");
				Console.WriteLine();
			}
			Console.ReadKey();
		}

		private static void writeHiero(List<string> xmlLines)
		{
			fontPath = fontPath.Replace(".fnt", "_xml.fnt");
			using (var fontWriter = new StreamWriter(fontPath))
			{
				foreach (var line in xmlLines)
				{
					fontWriter.WriteLine(line);
				}
			}
		}

		private static List<string> processHiero(string fontPath)
		{
			fontLines = new List<string>();
			xmlLines = new List<string>();
			processedLines = new List<string>();

			try
			{
				using (var fontReader = new StreamReader(fontPath))
				{
					string line = fontReader.ReadLine();
					fontLines.Add(line);
					while (line != null)
					{
						line = fontReader.ReadLine();
						fontLines.Add(line);
					}
				}

			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine("Something's wrong with your file...\n" + e.Message);
				return new List<string>();
			}


			foreach (var initialLine in fontLines)
			{
				var newLine = initialLine;
				var stringMatches = new List<string>();
				MatchCollection matches;

				if (initialLine != null)
				{
					matches = Regex.Matches(initialLine, "=(-?\\d+[^a-zA-Z0-9]|\\n)+");
					foreach (var match in matches)
					{
						stringMatches.Add(match.ToString());
					}
				}

				string[] distinctMatches = stringMatches.Distinct().ToArray();

				for (int i = 0; i < distinctMatches.Length; i++)
				{
					newLine = newLine.Replace(distinctMatches[i], distinctMatches[i].Replace("=", "=\"").Replace(" ", "\" "));
				}
				processedLines.Add(newLine);

				//Console.WriteLine("Initial line :");
				//Console.WriteLine(initialLine);
				//Console.WriteLine("Processed line :");
				//Console.WriteLine(newLine);
			}

			xmlLines.Add("<?xml version=\"1.0\"?>");
			xmlLines.Add("<font>");
			xmlLines.Add("<" + processedLines[0] + "\" />");
			xmlLines.Add("<" + processedLines[1].Replace("packed=", "packed=\"") + "\" />");
			xmlLines.Add("<pages>");
			xmlLines.Add("<" + processedLines[2] + " />");
			xmlLines.Add("</pages>");
			xmlLines.Add("<" + processedLines[3].Replace("count=", "count=\"") + "\" />");

			processedLines.RemoveRange(0, 4);
			processedLines.RemoveAt(processedLines.Count - 1);
			foreach (var line in processedLines)
			{
				xmlLines.Add("<" + line + " />");
			}
			xmlLines.Add("</chars>");
			xmlLines.Add("</font>");

			Console.WriteLine("Resulting lines are: ");
			Console.WriteLine("====================\n\n");
			foreach (var line in xmlLines)
			{
				Console.WriteLine(line + "\n");
			}

			return xmlLines;
		}

	}
}
