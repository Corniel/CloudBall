using System;
using System.Collections.Generic;

namespace CloudBall.Viewer
{
    [Serializable]
    public class CloudBallTurnReplay
    {
        public int Turn { get; set; }
        public bool RedScored { get; set; }
        public bool BlueScored { get; set; }
        public CloudBallBallReplay Ball { get; set; }
        public List<CloudBallPlayerReplay> Players { get; set; }

        public override string ToString()
        {
            return string.Format("Turn[{0}]", Turn);
        }
    }
}
