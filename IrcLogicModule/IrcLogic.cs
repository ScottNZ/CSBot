using CSBot;

namespace IrcLogicModule
{
	// Having most of the logic in its own module allows you to change most of it without having to restart the bot.
	public class IrcLogic : CSBotModule
	{
		public override void OnConnect(IrcClient client)
		{
			client.SendNickname(client.Setup.Nickname);
			client.SendUsername(client.Setup.Username ?? client.Setup.Nickname, client.Setup.Realname ?? client.Setup.Nickname);
		}

		public override void OnLineRead(IrcClient client, string line)
		{
			var parts = line.Split(new[] { ' ' }, 4);

			if (parts[0] == "PING")
				client.Pong(parts[1]);

			if (parts[0].StartsWith(":"))
			{
				var user = parts[0].Substring(1);
				var command = parts[1];
				var target = parts[2];

				switch (command)
				{
					case "001":
						ModuleManager.InvokeModules(m => m.OnRegister(client));
						break;
					case "PRIVMSG":
						var message = parts[3].Substring(1);
						ModuleManager.InvokeModules(m => m.OnMessage(client, user, target, message));
						if (Utils.IsChannel(target))
							ModuleManager.InvokeModules(m => m.OnPublicMessage(client, user, target, message));
						else
							ModuleManager.InvokeModules(m => m.OnPrivateMessage(client, user, target, message));
						break;
				}
			}
		}

		public override void OnRegister(IrcClient client)
		{
			foreach (var channel in client.Setup.AutoJoinChannels)
				client.Join(channel);
		}
	}
}
