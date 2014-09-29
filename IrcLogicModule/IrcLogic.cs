﻿using CSBot;

namespace IrcLogicModule
{
	// Having most of the logic in its own module allows you to change most of it without having to restart the bot.
	public class IrcLogic : CSBotModule
	{
		public override void OnLineRead(string line)
		{
			var parts = line.Split(new[] { ' ' }, 3);

			if (parts[0] == "PING")
				Client.Pong(parts[1]);

			if (parts[0].StartsWith(":"))
			{
				var user = parts[0].Substring(1);
				var command = parts[1];
				var args = parts[2];

				switch (command)
				{
					case "001":
						ModuleManager.InvokeModules(m => m.OnRegister());
						break;
					case "NOTICE":
					case "PRIVMSG":
						{
							var target = args.Substring(0, args.IndexOf(' '));
							var message = args.Substring(args.IndexOf(':') + 1);
							if (command == "PRIVMSG")
							{
								ModuleManager.InvokeModules(m => m.OnMessage(user, target, message));
								if (Utils.IsChannel(target))
									ModuleManager.InvokeModules(m => m.OnPublicMessage(user, target, message));
								else
									ModuleManager.InvokeModules(m => m.OnPrivateMessage(user, target, message));
							}
							else
							{
								ModuleManager.InvokeModules(m => m.OnNotice(user, target, message));
								if (Utils.IsChannel(target))
									ModuleManager.InvokeModules(m => m.OnPublicNotice(user, target, message));
								else
									ModuleManager.InvokeModules(m => m.OnPrivateNotice(user, target, message));
							}
						}
						break;
					case "JOIN":
						{
							var colon = args.IndexOf(':');
							var channel = colon != -1 ? args.Substring(colon + 1) : args;
							ModuleManager.InvokeModules(m => m.OnJoin(user, channel));
						}
						break;
					case "PART":
						{
							var colon = args.IndexOf(':');
							var channel = colon != -1 ? args.Substring(0, args.IndexOf(' ')) : args;
							var message = colon != -1 ? args.Substring(colon + 1) : null;
							ModuleManager.InvokeModules(m => m.OnPart(user, channel, message));
						}
						break;
					case "QUIT":
						{
							var colon = args.IndexOf(':');
							var message = colon != -1 ? args.Substring(colon + 1) : null;
							ModuleManager.InvokeModules(m => m.OnQuit(user, message));
						}
						break;
					case "NICK":
						ModuleManager.InvokeModules(m => m.OnNick(user, args.Substring(args.IndexOf(':') + 1)));
						break;
					case "KICK":
						{
							var argsSplit = args.Split(new[] { ' ' }, 3);
							var channel = argsSplit[0];
							var targetNickname = argsSplit[1];
							var message = argsSplit.Length >= 3 ? argsSplit[2].Substring(1) : null;
							ModuleManager.InvokeModules(m => m.OnKick(user, channel, targetNickname, message));
						}
						break;
				}
			}
		}

		public override void OnConnect()
		{
			Client.SendNickname(Client.Setup.Nickname);
			Client.SendUsername(Client.Setup.Username ?? Client.Setup.Nickname, Client.Setup.Realname ?? Client.Setup.Nickname);
		}

		public override void OnRegister()
		{
			foreach (var channel in Client.Setup.AutoJoinChannels)
				Client.Join(channel);
		}
	}
}
