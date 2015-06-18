using System;
using System.IO;

namespace CloudBall.Viewer
{
    [Serializable]
    public class CloudBallBallReplay
    {
        public float X { get; set; }
        public float Y { get; set; }

        public override string ToString()
        {
            return string.Format("Ball({0:0.0}, {1:0.0})", X, Y);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this.X);
            writer.Write(this.Y);
        }

        public static CloudBallBallReplay Read(BinaryReader reader)
        {
            var ball = new CloudBallBallReplay();
            ball.X = reader.ReadSingle();
            ball.X = reader.ReadSingle();
            return ball;
        }
    }
}

