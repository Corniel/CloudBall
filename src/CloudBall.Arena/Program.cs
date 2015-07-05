using CloudBall.Arena.Configuration;
using log4net;
using System;
using System.Diagnostics;
using System.Linq;

namespace CloudBall.Arena
{
	public class Program
	{
		/// <summary>The logger. For now, only to guarantee that the log4net assembly is available for bots.</summary>
		private ILog Log = LogManager.GetLogger(typeof(Program));

		public static void Main(string[] args)
		{
			Console.WindowWidth = 84;
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
			data.Load(settings.EngineDirectory);

			PluginCollection.Instance.Load(settings.PluginDirectory);

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

			Rnd = new Random(ArenaSettings.Instance.Seed);
			EngineWatcher = new ArenaEngineWatcher();
			PluginWatcher = new PluginWatcher();
		}

		public Random Rnd { get; protected set; }
		public ArenaEngineWatcher EngineWatcher { get; protected set; }
		public PluginWatcher PluginWatcher { get; protected set; }

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
					// if failure, just reload all engines.
					if (!Run(red, blue))
					{
						ArenaData.Instance = ArenaData.Load(ArenaSettings.Instance.DataFile);
					}
				}

				pairings.Remove(red);
				pairings.Remove(blue);
			}
		}
		public bool Run(TeamData red, TeamData blue)
		{
			using (var engine = new CloudBallEngine(red.CreateInstance(), blue.CreateInstance()))
			{
				CloudBallScore score = null;
				ConsoleX.WritePairing(red, blue);
				var sw = Stopwatch.StartNew();
				try
				{
					score = engine.Run();
				}
				catch (SimulationFailedException)
				{
					ConsoleX.WriteError("crashed");
					return false;
				}
				ConsoleX.WriteResult(red, blue, score, sw);

				UpdateResults(red, blue, score);
				UpdateRatings(red, blue, score);
				ApplyPostResultHandlers(red, blue, engine);

				ArenaData.Instance.Sort();
				ArenaData.Instance.SaveRankings(ArenaSettings.Instance.RankingsFile);
				ArenaData.Instance.Save(ArenaSettings.Instance.DataFile);
				return true;
			}
		}

		private static void UpdateResults(TeamData red, TeamData blue, CloudBallScore score)
		{
			red.Results.GoalsFor += score.Red;
			red.Results.GoalsAgainst += score.Blue;

			blue.Results.GoalsFor += score.Blue;
			blue.Results.GoalsAgainst += score.Red;

			if (score.RedWins)
			{
				red.Results.Wins++;
				blue.Results.Loses++;
			}
			else if (score.BlueWins)
			{
				blue.Results.Wins++;
				red.Results.Loses++;
			}
			else
			{
				red.Results.Draws++;
				blue.Results.Draws++;
			}
		}

		private static void UpdateRatings(TeamData red, TeamData blue, CloudBallScore score)
		{
			var zScore = Elo.GetZScore(red.Rating, blue.Rating);

			var kR = red.GetK(ArenaSettings.Instance.K, ArenaSettings.Instance.Stabilizer);
			red.Rating += kR * ((double)score.RedScore - zScore);

			var kB = blue.GetK(ArenaSettings.Instance.K, ArenaSettings.Instance.Stabilizer);
			blue.Rating += kB * ((double)score.BlueScore - (1d - zScore));
		}

		private static void ApplyPostResultHandlers(TeamData red, TeamData blue, CloudBallEngine engine)
		{
			foreach (var handler in PluginCollection.Instance.PostResultHandlers)
			{
				try
				{
					handler.Apply(engine.Game, red.Rating, blue.Rating);
				}
				catch (Exception x)
				{
					ConsoleX.WriteWarning("Plug-in '{0}' failed: {1}", handler.GetType().Name, x);
				}
			}
		}
	}
}
