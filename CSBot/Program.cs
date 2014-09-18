using System.IO;

namespace CSBot
{
	public static class Program
	{
		static void Main(string[] args)
		{
			var setupFilename = args.Length > 0 ? args[0] : "CSBot.xml";

			IrcClientSetup setup;
			using (var streamReader = new StreamReader(setupFilename))
				setup = IrcClientSetup.Deserialize(streamReader);

			var moduleManager = new ModuleManager();
			moduleManager.LoadAllModules();

			var client = new IrcClient(moduleManager, setup);
			client.Connect();
		}
	}
}
