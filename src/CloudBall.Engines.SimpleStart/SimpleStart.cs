using Common;
using System;

namespace CloudBall.Engines
{
	/// <summary>
	/// This is the example team TeamTwo.
	/// 
	/// It is a more advanced team. Each player has a position:
	/// 
	/// The keeper (shirt number 1) guards the goal.
	/// The left defender (shirt number 2) helps guarding the goal.
	/// The right defender (shirt number 3) tries to keep the enemies away from the goal.
	/// The forwards (shirt numbers 4,5,6) stay in the front and try to score goals.
	/// 
	/// Except for the positions, and this team tackling, this team is the same as TeamOne. Notice how big the difference is!
	/// </summary>
	[BotName("Simple Start")]
	public class SimpleStart : ITeam
	{
		public void Action(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
		{
			foreach (Player player in myTeam.Players)
			{
				Player closestEnemy = player.GetClosest(enemyTeam);

				//Always shoots for the enemy goal.
				if (ball.Owner == player)
				{
					player.ActionShootGoal();
				}
				//Picks up the ball if possible.
				else if (player.CanPickUpBall(ball))
				{
					player.ActionPickUpBall();
				}
				//Tackles any enemy that is close.
				else if (player.CanTackle(closestEnemy))
				{
					player.ActionTackle(closestEnemy);
				}
				//If the player cannot do anything urgently useful, move to a good position.
				else
				{
					//If the player is closest to the ball, go for it.
					if (player == ball.GetClosest(myTeam))
					{
						player.ActionGo(ball);
					}
					//The keeper protects the goal.
					else if (player.PlayerType == PlayerType.Keeper)
					{
						//The keeper positions himself 50 units out from the goal                                                                                                            //at the same height as the ball, although never leaving the goal
						player.ActionGo(new Vector(50, Math.Max(Math.Min(ball.Position.Y, Field.MyGoal.Bottom.Y), Field.MyGoal.Top.Y)));
					}
					//The left defender helps protect the goal
					else if (player.PlayerType == PlayerType.LeftDefender)
					{
						player.ActionGo(new Vector(Field.Borders.Width * 0.2, ball.Position.Y));
					}
					//The right defender chases the enemy closest to myGoal
					else if (player.PlayerType == PlayerType.RightDefender)
					{
						player.ActionGo(Field.MyGoal.GetClosest(enemyTeam));
					}
					//Right forward stays in position on the midline, until the ball comes close.
					else if (player.PlayerType == PlayerType.RightForward)
					{
						player.ActionGo((Field.Borders.Center + Field.Borders.Bottom + ball.Position) / 3);
					}
					//Left forward stays in position on the midline, until the ball comes close.
					else if (player.PlayerType == PlayerType.LeftForward)
					{
						player.ActionGo((Field.Borders.Center + Field.Borders.Top + ball.Position) / 3);  
					}
					//Center forward stays in position on the enemy side of the field.
					else if (player.PlayerType == PlayerType.CenterForward)
					{
						player.ActionGo((Field.Borders.Center + Field.EnemyGoal.Center + ball.Position) / 3);
					}
				}
			}
		}
	}
}