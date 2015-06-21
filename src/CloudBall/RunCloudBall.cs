using Common;
using System;
using System.Collections.Generic;

namespace CloudBall
{
#if WINDOWS || XBOX
	static class RunCloudBall
	{
		/// <summary>The main entry point for the application.</summary>
		[STAThread]
		static void Main(string[] args)
		{
			var teams = new List<ITeam>()
			{
				new CloudBall.Engines.Dummy(),
				//TeamFactory.Load(@"*\Engines\*.dll"),
				new CloudBall.Engines.GoForTheBall(),
				new CloudBall.Engines.SimpleStart(),
				new CloudBall.Engines.Stonewall(),
			};

			using (Client.Client client = new Client.Client(teams[1], teams[3]))
			{
				client.Run();
			}
		}
	}
#endif
}


