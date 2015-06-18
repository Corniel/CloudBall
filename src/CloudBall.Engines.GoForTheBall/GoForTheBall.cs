using Common;

namespace CloudBall.Engines
{
	/// <summary>
	/// This is the example team TeamOne.
	/// 
	/// It is a very basic team, all the players act the same, and they always go for the ball.
	/// </summary>
	[BotName("Going for the ball")]
	public class GoForTheBall : ITeam
	{
		public void Action(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
		{
			//Loop over all players in my team.
			foreach (Player player in myTeam.Players)
			{
				//Gets the closest enemy player.
				Player closestEnemy = player.GetClosest(enemyTeam);

				//If this player has the ball.
				if (ball.Owner == player)
				{
					//Tell this player to shoot towards the goal, at maximum strength!
					player.ActionShootGoal();
				}
				else if (player.CanPickUpBall(ball))
				{
					player.ActionPickUpBall();
				}
				//Worst case just go for the ball
				else { player.ActionGo(ball.Position); }
			}
		}
	}
}
