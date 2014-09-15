using System;
using CSBot;

namespace IrcLogicModule
{
	public static class Utils
	{
		public static void Pong(this IrcClient client, string reply)
		{
			client.WriteLine("PONG {0}", reply);
		}

		public static void Join(this IrcClient client, string channel)
		{
			client.WriteLine("JOIN {0}", channel);
		}

		public static void SendNickname(this IrcClient client, string nickname)
		{
			client.WriteLine("NICK {0}", nickname);
		}

		public static void SendUsername(this IrcClient client, string username, string realname)
		{
			client.WriteLine("USER {0} 0 * :{1}", username, realname);
		}

		public static void SendMessage(this IrcClient client, string target, string message)
		{
			client.WriteLine("PRIVMSG {0} :{1}", target, message);
		}

		internal static void InvokeModules(IrcClient client, Action<CSBotModule> func)
		{
			foreach (var module in client.ModuleManager.LoadedModules)
				func(module.Value.Module);
		}
	}
}
