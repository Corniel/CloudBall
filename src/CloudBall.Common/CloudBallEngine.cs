using Common;
using GameCommon;
using System;
using System.IO;
using System.Reflection;

namespace CloudBall
{
	public class CloudBallEngine : IDisposable
	{
		public CloudBallEngine(ITeam red, ITeam blue)
		{
			if (red == null) { throw new ArgumentNullException("red"); }
			if (blue == null) { throw new ArgumentNullException("blue"); }

			Red = red;
			Blue = blue;
			InnerEngine = new b(red, blue);
		}

		public ITeam Red { get; protected set; }
		public ITeam Blue { get; protected set; }

		protected b InnerEngine { get; set; }

		public CloudBallScore Run()
		{
			try
			{
				return CloudBallScore.Create(this.InnerEngine.d());
			}
			catch (Exception x)
			{
				throw new SimulationFailedException(x);
			}
		}

		public void Save(FileInfo file)
		{
			FileHandler.SaveReplay(file.FullName, Game);
		}

		public GameHistory Game
		{
			get
			{
				if (m_Game == null)
				{
					InnerEngine.c();
					var prop = typeof(b).GetField("e", BindingFlags.Instance | BindingFlags.NonPublic);
					m_Game = (GameHistory)prop.GetValue(InnerEngine);
					m_Game.Team1Name = TeamFactory.GetName(Red);
					m_Game.Team2Name = TeamFactory.GetName(Blue);
				}
				return m_Game;
			}
		}
		protected GameHistory m_Game;
	

		public void Dispose()
		{
			if (this.InnerEngine != null)
			{
				try
				{
					this.InnerEngine.Dispose();
					this.InnerEngine = null;
				}
				catch { }
			}
		}
	}
}
