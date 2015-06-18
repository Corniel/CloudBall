using System.Collections.Generic;

namespace CloudBall.Arena
{
	public class TeamDataComparer : IComparer<TeamData>
	{
		public int Compare(TeamData x, TeamData y)
		{
			if (x.IsActive == y.IsActive)
			{
				return y.Rating.CompareTo(x.Rating);
			}
			return x.IsActive.CompareTo(y.IsActive);
		}
	}
}
