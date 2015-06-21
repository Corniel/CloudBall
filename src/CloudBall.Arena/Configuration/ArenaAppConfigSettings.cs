using System;
using System.Configuration;
using System.IO;

namespace CloudBall.Arena.Configuration
{
	public class ArenaAppConfigSettings : IArenaSettings
	{
		public int Seed { get { return TryGet("Arena.Seed", Environment.TickCount); } }
		public int K { get { return TryGet("Arena.K", 30); } }

		public String ReferenceEngine { get { return TryGet("Arena.ReferenceEngine", String.Empty); } }

		public FileInfo DataFile { get { return TryGet("Arena.DataFile", new FileInfo("Arena.xml")); } }
		public FileInfo RankingsFile { get { return TryGet("Arena.RankingsFile", new FileInfo("Arena.Rankings.txt")); } }

		public DirectoryInfo EngineDirectory { get { return TryGet("Arena.EngineDirectory", new DirectoryInfo("Engines")); } }
		public DirectoryInfo ReplayDirectory { get { return TryGet("Arena.ReplayDirectory", new DirectoryInfo("Replays")); } }

		public bool FileSystemWatcherIsEnabled { get { return TryGet("Arena.FileSystemWatcherIsEnabled", true); } }


		protected string TryGet(String configkey, String def)
		{
			return ConfigurationManager.AppSettings[configkey] ?? def;
		}

		protected DirectoryInfo TryGet(String configkey, DirectoryInfo def)
		{
			try
			{
				return new DirectoryInfo(ConfigurationManager.AppSettings[configkey]);
			}
			catch (Exception x)
			{
				ConsoleX.WriteWarning("Failed to resolve app setting for '{0}': {1}", configkey, x.Message);
				return def;
			}
		}
		protected FileInfo TryGet(String configkey, FileInfo def)
		{
			try
			{
				return new FileInfo(ConfigurationManager.AppSettings[configkey]);
			}
			catch (Exception x)
			{
				ConsoleX.WriteWarning("Failed to resolve app setting for '{0}': {1}", configkey, x.Message);
				return def;
			}
		}
		protected int TryGet(String configkey, int def)
		{
			try
			{
				return int.Parse(ConfigurationManager.AppSettings[configkey]);
			}
			catch (Exception x)
			{
				ConsoleX.WriteWarning("Failed to resolve app setting for '{0}': {1}", configkey, x.Message);
				return def;
			}
		}
		protected bool TryGet(String configkey, bool def)
		{
			try
			{
				return bool.Parse(ConfigurationManager.AppSettings[configkey]);
			}
			catch (Exception x)
			{
				ConsoleX.WriteWarning("Failed to resolve app setting for '{0}': {1}", configkey, x.Message);
				return def;
			}
		}
	}
}
