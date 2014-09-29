using System;
using CSBot;
using IrcLogicModule;

namespace AdminModule
{
	public class Admin : CSBotModule
	{
		public override void OnMessage(string user, string target, string message)
		{
			if (user != Client.Setup.AdminUser)
				return;

			if (message.StartsWith("!loadmod ", StringComparison.OrdinalIgnoreCase))
			{
				var filename = message.Substring(message.IndexOf(' ') + 1);
				ModuleManager.LoadModuleDeferred(filename);
			}

			else if (message.StartsWith("!unloadmod ", StringComparison.OrdinalIgnoreCase))
			{
				var filename = message.Substring(message.IndexOf(' ') + 1);
				ModuleManager.UnloadModuleDeferred(filename);
			}

			else if (message.Equals("!loadedmods", StringComparison.OrdinalIgnoreCase))
				Client.SendMessageReply(user, target, string.Join(", ", ModuleManager.LoadedModules.Keys));

			else if (message.StartsWith("!send ", StringComparison.OrdinalIgnoreCase))
				Client.WriteLine(message.Substring(message.IndexOf(' ') + 1));

			else if (message.StartsWith("!say ", StringComparison.OrdinalIgnoreCase))
				Client.SendMessageReply(user, target, message.Substring(message.IndexOf(' ') + 1));
		}
	}
}
