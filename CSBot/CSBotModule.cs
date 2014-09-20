using System;

namespace CSBot
{
	public abstract class CSBotModule : MarshalByRefObject
	{
		public override object InitializeLifetimeService() { return null; }

		public ModuleManager ModuleManager { get; internal set; }

		public virtual void OnConnect(IrcClient client) { }
		public virtual void OnDisconnect(IrcClient client) { }
		public virtual void OnLineRead(IrcClient client, string line) { }
		public virtual void OnRegister(IrcClient client) { }
		public virtual void OnMessage(IrcClient client, string user, string target, string message) { }
		public virtual void OnPublicMessage(IrcClient client, string user, string target, string message) { }
		public virtual void OnPrivateMessage(IrcClient client, string user, string target, string message) { }
		public virtual void OnNotice(IrcClient client, string user, string target, string message) { }
		public virtual void OnPublicNotice(IrcClient client, string user, string target, string message) { }
		public virtual void OnPrivateNotice(IrcClient client, string user, string target, string message) { }
		public virtual void OnCtcp(IrcClient client, string user, string target, string message) { }
		public virtual void OnPublicCtcp(IrcClient client, string user, string target, string message) { }
		public virtual void OnPrivateCtcp(IrcClient client, string user, string target, string message) { }
		public virtual void OnJoin(IrcClient client, string user, string channel) { }
		public virtual void OnPart(IrcClient client, string user, string channel, string message) { }
		public virtual void OnQuit(IrcClient client, string user, string message) { }
		public virtual void OnMode(IrcClient client, string user, string targetUser, string modes) { }
		public virtual void OnTopic(IrcClient client, string user, string channel, string topic) { }
		public virtual void OnKick(IrcClient client, string user, string channel, string targetUser, string message) { }
		public virtual void OnInvite(IrcClient client, string user, string channel) { }
		public virtual void OnNick(IrcClient client, string user, string newNickname) { }
	}
}
