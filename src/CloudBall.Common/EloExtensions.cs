using System.Collections.Generic;
using System.Linq;

namespace CloudBall
{
	public static class EloExtensions
	{
		public static Elo Avarage(this IEnumerable<Elo> elos)
		{
			var doubles = elos.Select(elo => (double)elo);
			return doubles.Average();
		}
	}
}
