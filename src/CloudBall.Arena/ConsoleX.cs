using System;
using System.Diagnostics;

namespace CloudBall.Arena
{
	public static class ConsoleX
	{
		public static void WriteHeader()
		{
			Console.Clear();
			var fg = Console.ForegroundColor;
			var bg = Console.BackgroundColor;
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(@"                                                                              ");
			Console.WriteLine(@"                                                                              ");
			Console.WriteLine(@"                             C L O U D B A L L   arena                        ");

			Console.WriteLine(@"                                                                              ");
			ConsoleX.WriteField(@"                                                         ");
			ConsoleX.WriteField(@"  .---------------------------------------------------.  ");
			ConsoleX.WriteField(@"  |                         |                         |  ");
			ConsoleX.WriteField(@"  |                         |                         |  ");
			ConsoleX.WriteField(@"  |____                     |                     ____|  ");
			ConsoleX.WriteField(@"  |    |                  __|__                  |    |  ");
			ConsoleX.WriteField(@"  |    |                 /  |  \                 |    |  ");
			ConsoleX.WriteField(@"  |    |                |   o   |                |    |  ");
			ConsoleX.WriteField(@"  |    |                 \__|__/                 |    |  ");
			ConsoleX.WriteField(@"  |____|                    |                    |____|  ");
			ConsoleX.WriteField(@"  |                         |                         |  ");
			ConsoleX.WriteField(@"  |                         |                         |  ");
			ConsoleX.WriteField(@"  |                         |                         |  ");
			ConsoleX.WriteField(@"  '---------------------------------------------------'  ");
			ConsoleX.WriteField(@"                                                         ");
			Console.BackgroundColor = ConsoleColor.Black;
			Console.WriteLine(@"                                                                              ");
			Console.WriteLine(@"                                               {0,20}", string.Format("version {0}", typeof(Program).Assembly.GetName().Version));
			Console.ForegroundColor = fg;
			Console.BackgroundColor = bg;
		}

		public static void WriteField(string format, params object[] args)
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.Write("       ");
			Console.BackgroundColor = ConsoleColor.Red;
			Console.Write("  ");
			Console.BackgroundColor = ConsoleColor.Blue;
			Console.Write("  ");

			var line = string.Format(format, args);

			bool isDark = true;

			while (line.Length > 3)
			{
				Console.BackgroundColor = isDark ? ConsoleColor.DarkGreen : ConsoleColor.Green;
				Console.Write(line.Substring(0, 3));

				line = line.Substring(3);
				isDark = !isDark;
			}
			Console.BackgroundColor = isDark ? ConsoleColor.DarkGreen : ConsoleColor.Green;
			Console.Write(line);

			Console.BackgroundColor = ConsoleColor.Blue;
			Console.Write("  ");
			Console.BackgroundColor = ConsoleColor.Red;
			Console.WriteLine("  ");
		}

		public static void WriteWarning(string format, params object[] args)
		{
			var color = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(format, args);
			Console.ForegroundColor = color;
		}

		public static void WriteError(string format, params object[] args)
		{
			var color = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(format, args);
			Console.ForegroundColor = color;
		}

		public static void WritePairing(TeamData red, TeamData blue)
		{
			var color = Console.ForegroundColor;

			Console.ForegroundColor = ConsoleColor.Red;

			Console.Write("  {0,-20} ({1:0000})", red.Name, red.Rating);
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" - ");
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.Write("{0,-20} ({1:0000})", blue.Name, blue.Rating);
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" : ");
			Console.ForegroundColor = color;
		}

		public static void WriteResult(TeamData red, TeamData blue, CloudBallScore score, Stopwatch timer)
		{
			var color = Console.ForegroundColor;
			if /**/ (score.RedWins) { Console.ForegroundColor = ConsoleColor.Red; }
			else if (score.BlueWins) { Console.ForegroundColor = ConsoleColor.Blue; }
			else { Console.ForegroundColor = ConsoleColor.White; }

			Console.Write(score);

			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine("{0,4}s", timer.Elapsed.TotalSeconds.ToString("0.0"));

			Console.ForegroundColor = color;
		}
	}
}
