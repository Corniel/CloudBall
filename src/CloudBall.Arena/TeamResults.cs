using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CloudBall.Arena
{
	[Serializable]
	public class TeamResults
	{
		[XmlAttribute("w")]
		public int Wins { get; set; }
		[XmlAttribute("d")]
		public int Draws { get; set; }
		[XmlAttribute("l")]
		public int Loses { get; set; }

		[XmlAttribute("gf")]
		public int GoalsFor { get; set; }
		[XmlAttribute("ga")]
		public int GoalsAgainst { get; set; }

		public decimal AverageGoalsFor { get { return Matches == 0 ? 0 : GoalsFor / (decimal)Matches; } }
		public decimal AverageGoalsAgainst { get { return Matches == 0 ? 0 : GoalsAgainst / (decimal)Matches; } }


		public int Matches { get { return Wins + Draws + Loses; } }

		public decimal Score { get { return Matches == 0 ? 0 : (Wins + Draws * 0.5m) / Matches; } }
	}
}
