using System;
using System.Linq;
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

		public static void SendPassword(this IrcClient client, string password)
		{
			client.WriteLine("PASS {0}", password);
		}

		public static void SendMessage(this IrcClient client, string target, string message)
		{
			client.WriteLine("PRIVMSG {0} :{1}", target, message);
		}

		public static void SendMessage(this IrcClient client, string target, string format, params object[] args)
		{
			client.SendMessage(target, string.Format(format, args));
		}

		public static void SendMessageReply(this IrcClient client, string user, string target, string message)
		{
			client.SendMessage(IsChannel(target) ? target : User.Parse(user).Nickname, message);
		}

		public static void SendMessageReply(this IrcClient client, string user, string target, string format, params object[] args)
		{
			client.SendMessageReply(user, target, string.Format(format, args));
		}

		public static bool IsChannel(string target)
		{
			return !string.IsNullOrEmpty(target) && "#&+!".Any(p => p == target[0]);
		}

		internal static void InvokeModules(this ModuleManager self, Action<CSBotModule> func)
		{
			foreach (var module in self.LoadedModules)
			{
				try
				{
					func(module.Value.Module);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
		}
	}
}
