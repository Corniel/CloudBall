using System;
using System.IO;

namespace CloudBall.Arena.Configuration
{
	public class ArenaSettings : IArenaSettings
	{
		public static IArenaSettings Instance { get; set; }

		public Int32 Seed { get; set; }
		public Int32 K { get; set; }

		public FileInfo DataFile { get; set; }
		public DirectoryInfo EngineDirectory { get; set; }
		public FileInfo RankingsFile { get; set; }
		public DirectoryInfo ReplayDirectory { get; set; }

		public bool FileSystemWatcherIsEnabled { get; set; }
	}
}
