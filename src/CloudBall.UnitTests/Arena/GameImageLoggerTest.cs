using CloudBall.Arena;
using NUnit.Framework;
using System.IO;

namespace CloudBall.UnitTests.Arena
{
	[TestFixture]
	public class GameImageLoggerTest
	{
		[Test]
		public void Apply_GameHistoryFile_WritesImage()
		{
			using (var stream = GetType().Assembly.GetManifestResourceStream("CloudBall.UnitTests.Arena.GameHistory.cbr"))
			{
				var game = GameReplay.Load(stream);
				var logger = new GameImageLoggerStub();
				logger.Apply(game, 1200d, 1200d);
			}
		}

		public class GameImageLoggerStub : GameImageLogger
		{
			public override DirectoryInfo ReplayDirectory { get { return new DirectoryInfo("."); } }
		}
	}
}
