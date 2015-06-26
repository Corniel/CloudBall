using System.Collections.Generic;

namespace CloudBall.Arena
{
	public class TeamDataComparer : IComparer<TeamData>
	{
		public int Compare(TeamData x, TeamData y)
		{
			if (x.IsActive == y.IsActive)
			{
				return y.Score.GetValueOrDefault().CompareTo(x.Score.GetValueOrDefault());
			}
			return y.IsActive.CompareTo(x.IsActive);
		}
	}
}
