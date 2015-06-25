using CloudBall.Arena.Configuration;
using log4net;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CloudBall.Arena
{
	public class Program
	{
		/// <summary>The logger. For now, only to guarantee that the log4net assembly is available for bots.</summary>
		private ILog Log = LogManager.GetLogger(typeof(Program));

		public static void Main(string[] args)
		{
			if (args != null & args.Length > 0 && args[0] == "clear")
			{
				var settings = new ArenaAppConfigSettings();
				var data = ArenaData.Load(settings.DataFile);
				foreach (var team in data.Teams)
				{
					team.Clear();
				}
				data.Save(settings.DataFile);
			}
			while (true)
			{
				try
				{
					var arena = new Program();

					while (true)
					{
						arena.Run();
					}
				}
				catch (Exception x)
				{
					ConsoleX.WriteError("Arena crashed: {0}", x);
					Console.ReadLine();
				}
			}
		}

		public Program()
		{
			ConsoleX.WriteHeader();

			var settings = new ArenaAppConfigSettings();
			var data = ArenaData.Load(settings.DataFile);

			foreach (var file in settings.EngineDirectory.GetFiles("*.dll"))
			{
				try
				{
					var assembly = Assembly.LoadFile(file.FullName);
					data.AddOrUpdate(assembly);
				}
				catch (Exception x)
				{
					ConsoleX.WriteWarning("Loading Assembly failed: {0}", x.Message);
				}
			}

			Init(settings, data);
		}

		public Program(IArenaSettings settings, ArenaData data)
		{
			ConsoleX.WriteHeader();

			Init(settings, data);
		}

		protected void Init(IArenaSettings settings, ArenaData data)
		{
			ArenaSettings.Instance = settings;
			ArenaData.Instance = data;

			this.Rnd = new Random(ArenaSettings.Instance.Seed);
			this.Watcher = new ArenaEngineWatcher();
		}

		public Random Rnd { get; protected set; }
		public ArenaEngineWatcher Watcher { get; protected set; }

		public void Run()
		{
			var pairings = ArenaData.Instance.Teams.Where(t => t.IsActive).OrderBy(t => this.Rnd.Next()).ToList();
			while (pairings.Count > 1)
			{
				var red = pairings[0];
				var blue = pairings[1];

				// if removed in the mean time, this will fail.
				if (red.IsActive && blue.IsActive)
				{
					Run(red, blue);
				}

				pairings.Remove(red);
				pairings.Remove(blue);
			}
		}
		public void Run(TeamData red, TeamData blue)
		{
			using (var engine = new CloudBallEngine(red.CreateInstance(), blue.CreateInstance()))
			{
				try
				{
					ConsoleX.WritePairing(red, blue);
					var sw = Stopwatch.StartNew();
					var score = engine.Run();
					ConsoleX.WriteResult(red, blue, score, sw);

					red.GoalsFor += score.Red;
					red.GoalsAgainst += score.Blue;

					blue.GoalsFor += score.Blue;
					blue.GoalsAgainst += score.Red;

					if (score.RedWins)
					{
						red.Wins++;
						blue.Loses++;
					}
					else if (score.BlueWins)
					{
						blue.Wins++;
						red.Loses++;
					}
					else
					{
						red.Draws++;
						blue.Draws++;
					}
					if (ArenaSettings.Instance.ReplayDirectory.Exists)
					{
						var file = new FileInfo(Path.Combine(
								ArenaSettings.Instance.ReplayDirectory.FullName,
								string.Format("{0}-{1} {2:00}-{3:00} {4:yyyy-MM-dd_hh_mm_ss}.cbr",
								red.Name, blue.Name, score.Red, score.Blue, DateTime.Now)));
						engine.Save(file);
					}

					var zScore = Elo.GetZScore(red.Rating, blue.Rating);

					if (!red.IsReferenceEngine)
					{
						var kR = (ArenaSettings.Instance.K + ArenaSettings.Instance.K * 10 / (1 + red.Matches));
						red.Rating += kR * ((double)score.RedScore - zScore);
					}
					if (!blue.IsReferenceEngine)
					{
						var kB = (ArenaSettings.Instance.K + ArenaSettings.Instance.K * 10 / (1 + blue.Matches));
						blue.Rating += kB * ((double)score.BlueScore - (1d - zScore));
					}

					ArenaData.Instance.Sort();
					ArenaData.Instance.SaveRankings(ArenaSettings.Instance.RankingsFile);
					ArenaData.Instance.Save(ArenaSettings.Instance.DataFile);
				}
				catch (SimulationFailedException) { }
			}
		}
	}
}
