using System;
using GameCommon;

namespace CloudBall
{
    [Serializable]
    public class CloudBallScore
    {
        public int Red { get; set; }
        public int Blue { get; set; }
        public int ScoreDiv { get { return this.Red - this.Blue; } }

        public decimal RedScore
        {
            get
            {
                if (this.Red == this.Blue) { return 0.5m; }
                if (this.Red > this.Blue) { return 1.0m; }
                else { return 0.0m; }
            }
        }
        public decimal BlueScore { get { return 1.0m - this.RedScore; } }
 
        public bool RedWins { get { return this.RedScore == 1.0m; } }
        public bool BlueWins { get { return this.BlueScore == 1.0m; } }
        public bool IsDraw { get { return this.RedScore == 0.5m; } }

        public override string ToString()
        {
            return string.Format("{0,2} - {1,-2}", this.Red, this.Blue);
        }

        public static CloudBallScore Create(Score s)
        {
            var score = new CloudBallScore()
            {
                Red = s.Team1Score,
                Blue = s.Team2Score,
            };
            return score;
        }
    }
}

