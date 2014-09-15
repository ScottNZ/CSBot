using System.IO;

namespace CSBot
{
	public static class Program
	{
		static void Main()
		{
			IrcClientSetup setup;
			using (var streamReader = new StreamReader("CSBot.xml"))
				setup = IrcClientSetup.Deserialize(streamReader);

			var moduleManager = new ModuleManager();
			moduleManager.LoadAllModules();

			var client = new IrcClient(moduleManager, setup);
			client.Connect();
		}
	}
}
