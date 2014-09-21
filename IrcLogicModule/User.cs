namespace IrcLogicModule
{
	public class User
	{
		public string Nickname { get; private set; }
		public string Username { get; private set; }
		public string Hostname { get; private set; }

		public static User Parse(string user)
		{
			var u = new User();
			var ex = user.IndexOf('!');
			if (ex != -1)
			{
				u.Nickname = user.Substring(0, ex);
				var at = user.IndexOf('@');
				u.Username = user.Substring(ex + 1, at - ex);
				u.Hostname = user.Substring(at + 1);
			}
			else
				u.Nickname = u.Username = u.Hostname = user;
			return u;
		}
	}
}
