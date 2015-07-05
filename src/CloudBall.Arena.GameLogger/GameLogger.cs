using GameCommon;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace CloudBall.Arena.GameLogger
{
	public class AllGameLogger : IPostResultHandler
	{
		/// <summary>Gets the directory (app.config.key "Arena.ReplayDirectory") to log games too.</summary>
		public DirectoryInfo ReplayDirectory 
		{
			get
			{
				var path = ConfigurationManager.AppSettings["Arena.ReplayDirectory"];
				try
				{
					return new DirectoryInfo(path);
				}
				catch
				{
					return new DirectoryInfo("Replays");
				}
			}
		}

		/// <summary>Logs every game that has a final result.</summary>
		public void Apply(GameHistory game, Elo redRating, Elo blueRating)
		{
			if (!ReplayDirectory.Exists)
			{
				try
				{
					ReplayDirectory.Create();
				}
				catch { return; }
			}
			var score = game.Last().GetScore;

			var file = String.Format("{4:yyyy-MM-dd_hh_mm_ss} {2:00}-{3:00} {0}-{1}.cbr",
				game.Team1Name,
				game.Team2Name,
				score.Team1Score,
				score.Team2Score, 
				DateTime.Now);

			var path = Path.Combine(ReplayDirectory.FullName, file);
			FileHandler.SaveReplay(path, game);
		}
	}
}
