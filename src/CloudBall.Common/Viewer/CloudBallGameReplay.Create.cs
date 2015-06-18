using System.Collections.Generic;
using System.Linq;
using GameCommon;

namespace CloudBall.Viewer
{
    public partial class CloudBallGameReplay
    {
        public static CloudBallGameReplay Create(GameHistory history)
        {
            var game = new CloudBallGameReplay()
            {
                Turns = new List<CloudBallTurnReplay>(),
                TeamRed = history.Team1Name,
                TeamBlue = history.Team2Name,
            };

            int scoreRed = 0;
            int socreBlue = 0;

            foreach (var s in history)
            {
                var turn = new CloudBallTurnReplay()
                {
                    Players = new List<CloudBallPlayerReplay>(),
                    Turn = game.Turns.Count,
                };

                turn.RedScored = s.GetScore.Team1Score > scoreRed;
                turn.BlueScored = s.GetScore.Team2Score > socreBlue;
                
                scoreRed = s.GetScore.Team1Score;
                socreBlue = s.GetScore.Team2Score;

                turn.Ball = new CloudBallBallReplay() { X = s.GetBall.Pos.X, Y = s.GetBall.Pos.Y };

                var r = s.GetTeams[0];
                var b = s.GetTeams[1];

                for (int i = 0; i < 6; i++)
                {
                    var rp = r.GetPlayers[i];
                    var bp = b.GetPlayers[i];

                    turn.Players.Add(new CloudBallPlayerReplay() { F = rp.HasFallen(), X = rp.Pos.X, Y = rp.Pos.Y, N = (int)rp.PlayerType, T = 0 });
                    turn.Players.Add(new CloudBallPlayerReplay() { F = bp.HasFallen(), X = bp.Pos.X, Y = bp.Pos.Y, N = (int)bp.PlayerType, T = 1 });
                }
                game.Turns.Add(turn);
            }

            return game;
        }
    }
}
