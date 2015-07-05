using GameCommon;
using System.IO;

namespace CloudBall
{
	public class GameReplay
	{
		public static GameHistory Load(Stream stream)
		{
			var path = Path.Combine(Path.GetTempPath(), "temp.cbr");
			try
			{
				using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
				{
					stream.CopyTo(file);
				}
				return FileHandler.LoadReplay(path);
			}
			finally
			{
				try
				{
					File.Delete(path);
				}
				catch { }
			}
		}
	}
}
