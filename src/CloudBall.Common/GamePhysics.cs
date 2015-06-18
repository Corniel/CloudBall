using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBall
{
	public static class GamePhysics
	{
		/// <summary>The allowed distance to the ball for picking it up is 5.</summary>
		public const double BallPickUpTolence = 5;

		/// <summary>The maximum speed of a player is 3.</summary>
		public const double MaximumSpeedPlayer = 3;
		/// <summary>The maximum speed of the ball is 12.</summary>
		public const double MaximumSpeedBall = 12;
		
		/// <summary>The minimum power to play the ball with is zero.</summary>
		/// <remarks>
		/// This is of course not effective.
		/// </remarks>
		public const double MinimumPower = 0;
		/// <summary>The minimum power to play the ball with is ten, and will lead to a speed of 12.</summary>
		public const double MaximumPower = 10;

		/// <summary>The acceleration (effectively deceleration) of the ball is 0.989...</summary>
		public const double BallAcceleration = 0.989263071074468;
		/// <summary>The acceleration of a player is 0.06667.</summary>
		/// <remarks>
		/// acceleration = 1 + PlayerAcceleration / (speed - 3);
		/// </remarks>
		public const double PlayerAcceleration = 0.066967;

		/// <summary>Gets the ball velocity for the next turn given the current velocity.</summary>
		public static Vector GetBallVelocity(Vector velocity)
		{
			return velocity * BallAcceleration;
		}
		
		/// <summary>Gets a player velocity for the next turn given the current velocity.</summary>
		public static Vector GetPlayerVelocity(Vector velocity)
		{
			// normalize to 3 if needed.
			if (velocity.Length >= 3d) { return velocity * 3d / velocity.Length; }
			// gets the acceleration.
			var acceleration = 1d + PlayerAcceleration / (velocity.Length - 3d);
			return velocity * acceleration;
		}
	}
}
