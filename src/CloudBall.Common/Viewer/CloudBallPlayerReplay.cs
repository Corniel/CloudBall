using System;
using System.IO;

namespace CloudBall.Viewer
{
    [Serializable]
    public class CloudBallPlayerReplay
    {
        /// <summary>Is Fallen.</summary>
        public bool F { get; set; }

        /// <summary>Position X.</summary>
        public float X { get; set; }

        /// <summary>Position Y.</summary>
        public float Y { get; set; }

        /// <summary>Number.</summary>
        public int N { get; set; }

        /// <summary>Team.</summary>
        public int T { get; set; }

        public override string ToString()
        {
            return string.Format("Player({0:0.0}, {1:0.0}), Number: {2}, Team: {3}{4}", X, Y, N, T, F ? "*" : "");
        }
    }
}
