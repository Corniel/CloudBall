using System;
using System.IO;

namespace CloudBall.Arena.Configuration
{
    public interface IArenaSettings
    {
		Int32 Seed { get; }
		Int32 K { get; }

        FileInfo DataFile { get; }
        DirectoryInfo EngineDirectory { get; }
        FileInfo RankingsFile { get; }
        DirectoryInfo ReplayDirectory { get; }

        bool FileSystemWatcherIsEnabled { get; }
    }
}
