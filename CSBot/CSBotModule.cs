using System;

namespace CSBot
{
	public abstract class CSBotModule : MarshalByRefObject
	{
		public override object InitializeLifetimeService() { return null; }

		public virtual void OnConnect(IrcClient client) { }
		public virtual void OnDisconnect(IrcClient client) { }
		public virtual void OnLineRead(IrcClient client, string line) { }
		public virtual void OnRegister(IrcClient client) { }
		public virtual void OnPublicMessage(IrcClient client, string user, string target, string message) { }
	}
}
