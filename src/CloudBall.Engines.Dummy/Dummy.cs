using Common;

namespace CloudBall.Engines
{
	public class Dummy : ITeam
	{
		public void Action(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
		{
			foreach (var player in myTeam.Players)
			{
				player.ActionWait();
			}
		}
	}
}
