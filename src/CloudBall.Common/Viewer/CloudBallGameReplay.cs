using System;
using System.Collections.Generic;

namespace CloudBall.Viewer
{
    [Serializable]
    public partial class CloudBallGameReplay
    {
        public string TeamRed { get; set; }
        public string TeamBlue { get; set; }

        public List<CloudBallTurnReplay> Turns { get; set; }
    }
}
