using System;
using System.Runtime.Serialization;

namespace CloudBall
{
	[Serializable]
	public class SimulationFailedException : Exception
	{
		public SimulationFailedException() : this("The simulation failed.") { }
		public SimulationFailedException(Exception inner) : this("The simulation failed.", inner) { }
		public SimulationFailedException(string message) : base(message) { }
		public SimulationFailedException(string message, Exception inner) : base(message, inner) { }
		protected SimulationFailedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
