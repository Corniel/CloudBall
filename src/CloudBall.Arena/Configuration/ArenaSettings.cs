using System;
using System.IO;

namespace CloudBall.Arena.Configuration
{
	public class ArenaSettings : IArenaSettings
	{
		public static IArenaSettings Instance { get; set; }

		public Int32 Seed { get; set; }
		public Int32 K { get; set; }
		public Double Stabilizer { get; set; }

		public String ReferenceEngine { get; set; }

		public FileInfo DataFile { get; set; }
		public DirectoryInfo EngineDirectory { get; set; }
		public FileInfo RankingsFile { get; set; }
		public DirectoryInfo PluginDirectory { get; set; }

		public bool FileSystemWatcherIsEnabled { get; set; }
	}
}
