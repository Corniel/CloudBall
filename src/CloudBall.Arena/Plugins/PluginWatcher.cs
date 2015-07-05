using CloudBall.Arena.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace CloudBall.Arena
{
	public class PluginWatcher
	{
		public PluginWatcher()
		{
			this.Watcher = new FileSystemWatcher();

			this.Watcher.Path = ArenaSettings.Instance.PluginDirectory.FullName;
			this.Watcher.Filter = "*.dll";

			this.Watcher.NotifyFilter =
				NotifyFilters.LastAccess |
				NotifyFilters.LastWrite |
				NotifyFilters.FileName |
				NotifyFilters.DirectoryName;

			// Add event handlers.
			this.Watcher.Created += new FileSystemEventHandler(OnCreated);
			this.Watcher.Deleted += new FileSystemEventHandler(OnDeleted);

			// Begin watching.
			this.Watcher.EnableRaisingEvents = ArenaSettings.Instance.FileSystemWatcherIsEnabled;
		}

		protected FileSystemWatcher Watcher { get; set; }

		protected static void OnCreated(object source, FileSystemEventArgs e)
		{
			try
			{
				var assembly = Assembly.LoadFile(e.FullPath);
				PluginCollection.Instance.AddOrUpdate(assembly);
			}
			catch (Exception x)
			{
				ConsoleX.WriteWarning("Creating dll failed: {0}", x);
			}
		}

		protected static void OnDeleted(object source, FileSystemEventArgs e)
		{
			PluginCollection.Instance.DeActivate(e.FullPath);
		}
	}
}
