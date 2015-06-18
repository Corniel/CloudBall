using Common;
using GameCommon;
using System;
using System.IO;

namespace CloudBall
{
	public class CloudBallEngine : IDisposable
	{
		public CloudBallEngine(ITeam red, ITeam blue)
		{
			if (red == null) { throw new ArgumentNullException("red"); }
			if (red == null) { throw new ArgumentNullException("blue"); }

			this.InnerEngine = new b(red, blue);
		}

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
			using (var stream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write))
			{
				Save(stream);
			} 
		}
		public void Save(Stream stream)
		{
			var data = this.InnerEngine.c();
			stream.Write(data, 0, data.Length);
		}

		public GameHistory Game
		{
			get
			{
				var file = new FileInfo(Path.GetTempFileName());
				if(!file.Directory.Exists)
				{
					file.Directory.Create();
				}
				Save(file);

				var history = FileHandler.LoadReplay(file.FullName);
				file.Delete();

				return history;
			}
		}

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
