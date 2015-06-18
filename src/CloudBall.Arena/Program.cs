using CloudBall.Arena.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CloudBall.Arena
{
	public class Program
	{
		public static void Main(string[] args)
		{
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

			foreach (var team in data.Teams)
			{
				team.IsActive = false;
			}
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

				Run(red, blue);

				pairings.Remove(red);
				pairings.Remove(blue);
			}

		}
		public void Run(TeamData r, TeamData b)
		{
			// Strongest team first.
			var red = r.Rating < b.Rating ? b : r;
			var blue = r.Rating < b.Rating ? r : b;

			using (var engine = new CloudBallEngine(red.CreateInstance(), blue.CreateInstance()))
			{
				try
				{
					var score = engine.Run();

					ConsoleX.WriteResult(red, blue, score);

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

					red.Rating += ArenaSettings.Instance.K * ((double)score.RedScore - zScore);
					blue.Rating += ArenaSettings.Instance.K * ((double)score.BlueScore - (1d - zScore));

					ArenaData.Instance.Sort();
					ArenaData.Instance.SaveRankings(ArenaSettings.Instance.RankingsFile);
					ArenaData.Instance.Save(ArenaSettings.Instance.DataFile);
				}
				catch (SimulationFailedException) { }
			}
		}
	}
}
