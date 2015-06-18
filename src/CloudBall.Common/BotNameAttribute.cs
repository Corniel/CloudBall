using System;

namespace CloudBall
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class BotNameAttribute: Attribute
	{
		private BotNameAttribute() { }
		public BotNameAttribute(string name) { Name = name; }
		public string Name { get; set; }

		public override string ToString() { return Name; }
	}
}
