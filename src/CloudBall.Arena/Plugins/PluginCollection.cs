using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CloudBall.Arena
{
	public class PluginCollection
	{
		public static readonly PluginCollection Instance = new PluginCollection();

		private PluginCollection() { }

		public IEnumerable<IPostResultHandler> PostResultHandlers { get { return HandlerLookup.Values; } }
		protected Dictionary<Type, IPostResultHandler> HandlerLookup = new Dictionary<Type, IPostResultHandler>();

		public int Count { get { return HandlerLookup.Count; } }

		public void Load(DirectoryInfo directory)
		{
			foreach (var file in directory.GetFiles("*.dll"))
			{
				try
				{
					var assembly = Assembly.LoadFile(file.FullName);
					AddOrUpdate(assembly);
				}
				catch (Exception x)
				{
					ConsoleX.WriteWarning("Loading Assembly failed: {0}", x.Message);
				}
			}
		}

		public void AddOrUpdate(Assembly assembly)
		{
			var types = assembly.GetTypes().Where(tp => tp.GetInterfaces().Contains(typeof(IPostResultHandler)));

			foreach (var tp in types)
			{
				HandlerLookup[tp] = (IPostResultHandler)Activator.CreateInstance(tp);
			}

		}

		public void DeActivate(string path)
		{
			var toRemove = this.HandlerLookup.Where(kvp => kvp.Key.Assembly.Location == path).Select(kvp=> kvp.Key).ToArray();

			foreach (var tp in toRemove)
			{
				HandlerLookup.Remove(tp);
			}
		}
	}
}
