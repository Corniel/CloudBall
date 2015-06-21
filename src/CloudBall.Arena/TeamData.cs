using CloudBall.Arena.Configuration;
using Common;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace CloudBall.Arena
{
	[Serializable]
	public class TeamData : IEquatable<TeamData>
	{
		public static readonly Elo InitalRating = 1600d;

		public string Name { get; set; }

		[XmlIgnore]
		public Type Implementation { get; set; }
		public Assembly Assembly { get { return this.Implementation == null ? null : this.Implementation.Assembly; } }

		public bool IsActive { get; set; }
		public Elo Rating { get; set; }
		public int Wins { get; set; }
		public int Draws { get; set; }
		public int Loses { get; set; }

		public int GoalsFor { get; set; }
		public int GoalsAgainst { get; set; }

		/// <summary>Returns true if the engine is configured as reference engine.</summary>
		/// <remarks>
		/// If so, it's rating will not change.
		/// </remarks>
		public bool IsReferenceEngine
		{
			get
			{
				return String.Compare(Name, ArenaSettings.Instance.ReferenceEngine, StringComparison.InvariantCultureIgnoreCase) == 0;
			}

		}

		public int Matches { get { return Wins + Draws + Loses; } }

		public decimal? Score
		{
			get
			{
				if (Matches == 0) { return 0; }

				return (Wins + Draws * 0.5m) / Matches;
			}
		}

		#region IEquatable

		/// <summary>Returns true if this instance and the other instance are equal, otherwise false.</summary>
		/// <param name="other">Other team data.</param>
		public bool Equals(TeamData other)
		{
			if (object.ReferenceEquals(other, null)) { return false; }
			return
				this.Name == other.Name;
		}

		/// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
		/// <param name="obj">An object to compare with.</param>
		public override bool Equals(object obj)
		{
			if (obj is TeamData)
			{
				return Equals((TeamData)obj);
			}
			else return false;
		}

		/// <summary>Returns the hash code for this Elo rating.</summary>
		/// <returns>
		/// A 32-bit signed integer hash code.
		/// </returns>
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}

		/// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator ==(TeamData left, TeamData right)
		{
			if (!object.ReferenceEquals(left, null))
			{
				return left.Equals(right);
			}
			if (!object.ReferenceEquals(right, null))
			{
				return right.Equals(left);
			}
			return true;
		}

		/// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator !=(TeamData left, TeamData right)
		{
			return !(left == right);
		}

		#endregion

		public ITeam CreateInstance()
		{
			if (this.Implementation == null)
			{
				throw new ArgumentNullException("this.Implementation", string.Format(
					"The implementation of {0} is not set.", this.Name));
			}
			return (ITeam)Activator.CreateInstance(this.Implementation);
		}

		public override string ToString()
		{
			return string.Format("{0} ({1:0000})", this.Name, this.Rating);
		}

		public static TeamData Create(Assembly assembly)
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

			var botType = tps[0];

			var nameAttr = botType.GetCustomAttribute<BotNameAttribute>();

			var name = nameAttr != null ? nameAttr.Name : GetNameFromFile(file);
			
			var team = new TeamData()
			{
				Name = name,
				Rating = InitalRating,
				IsActive = true,
				Implementation = botType,
			};
			return team;
		}
		private static string GetNameFromFile(FileInfo file)
		{
			var name = Path.GetFileNameWithoutExtension(file.FullName);
			return name.Substring(name.LastIndexOf('.') + 1);
		}
	}
}
