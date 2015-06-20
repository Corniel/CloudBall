using Common;
using System.Collections.Generic;

namespace CloudBall.Engines
{
	[BotName("Stonewall")]
	public class Stonewall : ITeam
	{
		private Dictionary<PlayerType, Vector> WallPositions = new Dictionary<PlayerType, Vector>()
		{
			{ PlayerType.LeftForward, new Vector(24, 422) },
			{ PlayerType.LeftDefender, new Vector(40, 469) },
			{ PlayerType.Keeper, new Vector(49, 516) },
			{ PlayerType.RightDefender, new Vector(49, 563) },
			{ PlayerType.CenterForward, new Vector(40, 610) },
			{ PlayerType.RightForward, new Vector(24, 659) },
		};

		public void Action(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
		{
			foreach (var player in myTeam.Players)
			{
				if (player.CanPickUpBall(ball))
				{
					player.ActionPickUpBall();
				}
				else if (player == ball.Owner)
				{
					player.ActionShootGoal();
				}
				else if (ball.Owner != null && ball.Owner.Team != myTeam && player.CanTackle(ball.Owner))
				{
					player.ActionTackle(ball.Owner);
				}
				else if (WallPositions[player.PlayerType].GetDistanceTo(player) > 1.0)
				{
					player.ActionGo(WallPositions[player.PlayerType]);
				}
				else
				{
					player.ActionWait();
				}
			}
		}
	}
}
