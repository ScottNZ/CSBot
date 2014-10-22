using System;
using System.Diagnostics;
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

			var logic = ModuleManager.Get<IrcLogic>();

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
				logic.SendMessageReply(user, target, string.Join(", ", ModuleManager.LoadedModules.Keys));

			else if (message.StartsWith("!send ", StringComparison.OrdinalIgnoreCase))
				logic.WriteLine(message.Substring(message.IndexOf(' ') + 1).Replace("\\n", "\n"));

			else if (message.StartsWith("!say ", StringComparison.OrdinalIgnoreCase))
				foreach (var line in message.Substring(message.IndexOf(' ') + 1).Split(new[] { "\\n" }, StringSplitOptions.RemoveEmptyEntries))
					logic.SendMessageReply(user, target, line);

			else if (message.Equals("!uptime", StringComparison.OrdinalIgnoreCase))
			{
				var uptime = DateTime.Now - Process.GetCurrentProcess().StartTime;
				logic.SendMessageReply(user, target, string.Format("Uptime: {0} days, {1} hours, {2} minutes, {3} seconds", uptime.Days, uptime.Hours, uptime.Minutes, uptime.Seconds));
			}
			
		}
	}
}
