using System;
using CSBot;
using IrcLogicModule;

namespace AdminModule
{
	public class Admin : CSBotModule
	{
		public override void OnMessage(IrcClient client, string user, string target, string message)
		{
			if (user == client.Setup.AdminUser)
			{
				if (message.StartsWith("!loadmod ", StringComparison.OrdinalIgnoreCase))
				{
					var filename = message.Substring(message.IndexOf(' ') + 1);
					client.ModuleManager.LoadModuleDeferred(filename);
				}

				else if (message.StartsWith("!unloadmod ", StringComparison.OrdinalIgnoreCase))
				{
					var filename = message.Substring(message.IndexOf(' ') + 1);
					client.ModuleManager.UnloadModuleDeferred(filename);
				}

				else if (message.Equals("!loadedmods", StringComparison.OrdinalIgnoreCase))
					client.SendMessageReply(user, target, string.Join(", ", client.ModuleManager.LoadedModules.Keys));

				else if (message.StartsWith("!send ", StringComparison.OrdinalIgnoreCase))
					client.WriteLine(message.Substring(message.IndexOf(' ') + 1));

				else if (message.StartsWith("!say ", StringComparison.OrdinalIgnoreCase))
					client.SendMessageReply(user, target, message.Substring(message.IndexOf(' ') + 1));
			}
		}
	}
}
