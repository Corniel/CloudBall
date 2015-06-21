using Common;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CloudBall
{
	public static class TeamFactory
	{
		/// <summary>Gets the type in the assembly that implements <see cref="Common.ITeam"/>.</summary>
		/// <param name="assembly">
		/// The assembly to scan.
		/// </param>
		/// <remarks>
		/// Only assemblies with exactly one implementation are supported.
		/// </remarks>
		public static Type GetTeamType(Assembly assembly)
		{
			var tps = assembly.GetTypes().Where(tp => tp.GetInterfaces().Contains(typeof(ITeam))).ToList();

			var file = new FileInfo(assembly.Location);

			if (tps.Count == 0)
			{
				throw new NotSupportedException(string.Format("The Assembly '{0}' does not contain an ITeam implementation.", file.Name));
			}
			if (tps.Count > 1)
			{
				throw new NotSupportedException(string.Format("The Assembly '{0}' does not contain multiple ITeam implementations.", file.Name));
			}
			return tps[0];
		}

		/// <summary>Loads an <see cref="Common.ITeam"/> from an assembly.</summary>
		public static ITeam Load(String assemblyPath) { return Load(new FileInfo(assemblyPath)); }

		/// <summary>Loads an <see cref="Common.ITeam"/> from an assembly.</summary>
		public static ITeam Load(FileInfo assemblyPath)
		{
			var assembly = Assembly.LoadFile(assemblyPath.FullName);
			return Load(assembly);
		}

		/// <summary>Loads an <see cref="Common.ITeam"/> from an assembly.</summary>
		public static ITeam Load(Assembly assembly)
		{
			var type = GetTeamType(assembly);
			return (ITeam)Activator.CreateInstance(type);
		}
	}
}
