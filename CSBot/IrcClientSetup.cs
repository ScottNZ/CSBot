using System;
using System.IO;
using System.Xml.Serialization;

namespace CSBot
{
	public class IrcClientSetup : MarshalByRefObject
	{
		public string Nickname { get; set; }
		public string Username { get; set; }
		public string Realname { get; set; }
		public string ServerHostname { get; set; }
		public int Port { get; set; }
		public int IOTimeout { get; set; }
		public string Password { get; set; }
		public string AdminUser { get; set; }
		public string[] AutoJoinChannels { get; set; }

		public override object InitializeLifetimeService() { return null; }

		public static IrcClientSetup Deserialize(TextReader textReader)
		{
			var serializer = new XmlSerializer(typeof(IrcClientSetup));
			return (IrcClientSetup)serializer.Deserialize(textReader);
		}

		public void Serialize(TextWriter textWriter)
		{
			var serializer = new XmlSerializer(typeof(IrcClientSetup));
			serializer.Serialize(textWriter, this);
		}
	}
}
