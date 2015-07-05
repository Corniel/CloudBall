using System;
using System.IO;

namespace CloudBall.Arena.Configuration
{
	public interface IArenaSettings
	{
		Int32 Seed { get; }
		Int32 K { get; }
		double Stabilizer { get; }

		String ReferenceEngine { get; }
		FileInfo DataFile { get; }
		DirectoryInfo EngineDirectory { get; }
		FileInfo RankingsFile { get; }
		DirectoryInfo PluginDirectory { get; }

		bool FileSystemWatcherIsEnabled { get; }
	}
}
