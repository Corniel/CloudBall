using GameCommon;

namespace CloudBall.Arena
{
	public interface IPostResultHandler
	{
		void Apply(GameHistory game, Elo redRating, Elo blueRating);
	}
}
