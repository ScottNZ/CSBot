using System;

namespace CSBot
{
	public abstract class CSBotModule : MarshalByRefObject
	{
		public override object InitializeLifetimeService() { return null; }

		public ModuleManager ModuleManager { get; internal set; }
		public IrcClient Client { get; internal set; }

		public virtual void OnConnect() { }
		public virtual void OnDisconnect() { }
		public virtual void OnLineRead(string line) { }
		public virtual void OnRegister() { }
		public virtual void OnMessage(string user, string target, string message) { }
		public virtual void OnPublicMessage(string user, string target, string message) { }
		public virtual void OnPrivateMessage(string user, string target, string message) { }
		public virtual void OnNotice(string user, string target, string message) { }
		public virtual void OnPublicNotice(string user, string target, string message) { }
		public virtual void OnPrivateNotice(string user, string target, string message) { }
		public virtual void OnCtcp(string user, string target, string message) { }
		public virtual void OnPublicCtcp(string user, string target, string message) { }
		public virtual void OnPrivateCtcp(string user, string target, string message) { }
		public virtual void OnJoin(string user, string channel) { }
		public virtual void OnPart(string user, string channel, string message) { }
		public virtual void OnQuit(string user, string message) { }
		public virtual void OnMode(string user, string targetUser, string modes) { }
		public virtual void OnTopic(string user, string channel, string topic) { }
		public virtual void OnKick(string user, string channel, string targetNickname, string message) { }
		public virtual void OnInvite(string user, string channel) { }
		public virtual void OnNick(string user, string newNickname) { }
	}
}
