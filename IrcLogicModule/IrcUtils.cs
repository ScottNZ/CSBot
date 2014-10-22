using System.Linq;

namespace IrcLogicModule
{
	// extension methods allow the caller to stay on the same appdomain so complex objects don't need to be serialised.
	public static class IrcUtils
	{
		public static void WriteLine(this IrcLogic ircLogic, string format, params object[] args)
		{
			ircLogic.WriteLine(string.Format(format, args));
		}

		public static void Pong(this IrcLogic ircLogic, string reply)
		{
			ircLogic.WriteLine("PONG {0}", reply);
		}

		public static void Join(this IrcLogic ircLogic, string channel)
		{
			ircLogic.WriteLine("JOIN {0}", channel);
		}

		public static void SendNickname(this IrcLogic ircLogic, string nickname)
		{
			ircLogic.WriteLine("NICK {0}", nickname);
		}

		public static void SendUsername(this IrcLogic ircLogic, string username, string realname)
		{
			ircLogic.WriteLine("USER {0} 0 * :{1}", username, realname);
		}

		public static void SendPassword(this IrcLogic ircLogic, string password)
		{
			ircLogic.WriteLine("PASS {0}", password);
		}

		public static void SendMessage(this IrcLogic ircLogic, string target, string message)
		{
			ircLogic.WriteLine("PRIVMSG {0} :{1}", target, message);
		}

		public static void SendMessage(this IrcLogic ircLogic, string target, string format, params object[] args)
		{
			ircLogic.SendMessage(target, string.Format(format, args));
		}

		public static void SendMessageReply(this IrcLogic ircLogic, string user, string target, string message)
		{
			ircLogic.SendMessage(IsChannel(target) ? target : User.Parse(user).Nickname, message);
		}

		public static void SendMessageReply(this IrcLogic ircLogic, string user, string target, string format, params object[] args)
		{
			ircLogic.SendMessageReply(user, target, string.Format(format, args));
		}

		public static bool IsChannel(string target)
		{
			return !string.IsNullOrEmpty(target) && "#&+!".Contains(target[0]);
		}
	}
}