using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace CloudBall.Arena
{
	[Serializable]
	public class ArenaData
	{
		public static ArenaData Instance { get; set; }

		public ArenaData()
		{
			this.Teams = new List<TeamData>();
		}

		public List<TeamData> Teams { get; set; }

		public void AddOrUpdate(Assembly assembly)
		{
			try
			{
				var team = TeamData.Create(assembly);

				var existing = this.Teams.FirstOrDefault(t => t == team);

				if (existing == null)
				{
					this.Teams.Add(team);
					Console.WriteLine("Added: {0}", team);
				}
				else
				{
					existing.IsActive = true;
					existing.Implementation = team.Implementation;
					Console.WriteLine("Activated: {0}", existing);
				}

			}
			catch (Exception x)
			{
				ConsoleX.WriteWarning("AddOrUpdate failed: {0}", x);
			}
		}

		public void DeActivate(string path)
		{
			try
			{
				var team = this.Teams.FirstOrDefault(t => t.Assembly.Location == path);

				if (team != null)
				{
					team.IsActive = false;
					team.Implementation = null;
					Console.WriteLine("De-activated: {0}", team);
				}
			}
			catch (Exception x)
			{
				ConsoleX.WriteWarning("De-activating dll failed: {0}", x);
			}
		}

		public void Sort()
		{
			this.Teams.Sort(new TeamDataComparer());
		}

		public void SaveRankings(FileInfo file)
		{
			int i = 1;
			using (var writer = new StreamWriter(file.FullName))
			{
				foreach (var team in ArenaData.Instance.Teams)
				{
					writer.WriteLine("{0,4}  {1,-20}  {2:0000}  {3,6}  {4,6}  {5,6}  {6,6}  {7:000%} {8}",
						i++,
						team.Name,
						team.Rating,
						team.Wins,
						team.Draws,
						team.Loses,
						team.Matches,
						team.Score,
						team.IsActive ? "" : "*");
				}
			}
		}

		#region I/O

		public void Save(FileInfo file)
		{
			using (var stream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write))
			{
				var serializer = new XmlSerializer(typeof(ArenaData));
				serializer.Serialize(stream, this);
			}
		}
		public static ArenaData Load(FileInfo file)
		{
			if (file.Exists)
			{
				using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
				{
					var serializer = new XmlSerializer(typeof(ArenaData));
					return (ArenaData)serializer.Deserialize(stream);
				}
			}
			else
			{
				return new ArenaData();
			}
		}

		#endregion
	}
}
