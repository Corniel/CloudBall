using Common;
using GameCommon;
using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace CloudBall.Arena
{
	public class GameImageLogger : IPostResultHandler
	{
		/// <summary>Gets the directory (app.config.key "Arena.ReplayDirectory") to log games too.</summary>
		public virtual DirectoryInfo ReplayDirectory
		{
			get
			{
				var path = ConfigurationManager.AppSettings["Arena.ReplayImageDirectory"];
				try
				{
					return new DirectoryInfo(path);
				}
				catch
				{
					return new DirectoryInfo("Images");
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

			var file = String.Format("{4:yyyy-MM-dd_hh_mm_ss} {2:00}-{3:00} {0}-{1}.png",
				game.Team1Name,
				game.Team2Name,
				score.Team1Score,
				score.Team2Score,
				DateTime.Now);

			var xMax = 1 + (int)Field.Borders.Right.X / 4;
			var yMax = 1 + (int)Field.Borders.Bottom.Y / 4;

			var r = new int[xMax, yMax];
			var g = new int[xMax, yMax];
			var b = new int[xMax, yMax];

			foreach (var round in game)
			{
				var pB = GetPointer(round.GetBall.Pos);
				r[pB.Item1, pB.Item2]++;
				g[pB.Item1, pB.Item2]++;
				b[pB.Item1, pB.Item2]++;

				foreach (var player in round.GetTeams[0].GetPlayers)
				{
					var pRed = GetPointer(player.Pos);
					r[pRed.Item1, pRed.Item2]++;
				}
				foreach (var player in round.GetTeams[1].GetPlayers)
				{
					var pBlue = GetPointer(player.Pos);
					b[pBlue.Item1, pBlue.Item2]++;
				}
			}

			double max = 0;

			for (var x = 0; x < xMax; x++)
			{
				for (var y = 0; y < yMax; y++)
				{
					max = Math.Max(max, r[x, y]);
					max = Math.Max(max, g[x, y]);
					max = Math.Max(max, b[x, y]);
				}
			}

			var bitmap = new Bitmap(xMax, yMax);
			for (var x = 0; x < xMax; x++)
			{
				for (var y = 0; y < yMax; y++)
				{
					var rC = (int)Math.Ceiling(55 * r[x, y] / max);
					var gC = (int)Math.Ceiling(55 * g[x, y] / max);
					var bC = (int)Math.Ceiling(55 * b[x, y] / max);

					if (rC > 0) { rC += 200; }
					if (gC > 0) { gC += 200; }
					if (bC > 0) { bC += 200; }

					var color = Color.FromArgb(rC, gC, bC);

					bitmap.SetPixel(x, y, color);
				}
			}
			var path = Path.Combine(ReplayDirectory.FullName, file);
			bitmap.Save(path, ImageFormat.Png);
		}

		private static Tuple<int, int> GetPointer(Vector v)
		{
			var x = (int)Math.Round(v.X / 4);
			var y = (int)Math.Round(v.Y / 4);
			return new Tuple<int, int>(x, y);
		}
	}
}
