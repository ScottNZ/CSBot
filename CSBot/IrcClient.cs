using System;
using System.IO;
using System.Net.Sockets;

namespace CSBot
{
	public class IrcClient : MarshalByRefObject, IDisposable
	{
		public IrcClientSetup Setup { get; private set; }

		internal event Action OnConnect = () => { };
		internal event Action<string> OnLineRead = l => { };
		internal event Action OnDisconnect = () => { };

		TcpClient socket;
		NetworkStream stream;
		StreamReader reader;
		StreamWriter writer;
		bool disposed;

		public override object InitializeLifetimeService() { return null; }

		public IrcClient(IrcClientSetup setup)
		{
			Setup = setup;
		}

		public void Connect()
		{
			if (disposed)
				throw new ObjectDisposedException(GetType().FullName);

			socket = new TcpClient();
			socket.ReceiveTimeout = socket.SendTimeout = Setup.IOTimeout;
			socket.Connect(Setup.ServerHostname, Setup.Port);
			stream = socket.GetStream();
			reader = new StreamReader(stream);
			writer = new StreamWriter(stream) { AutoFlush = true };
			Console.WriteLine("Connected");
			OnConnect();
			try
			{
				string l;
				while (!disposed && (l = reader.ReadLine()) != null)
				{
					var line = l;
					Console.WriteLine(line);
					OnLineRead(line);
				}
			}
			finally
			{
				if (socket != null)
					socket.Close();
				if (stream != null)
					stream.Close();
				if (reader != null)
					reader.Close();
				if (writer != null)
					writer.Close();
				Console.WriteLine("Disconnected");
				OnDisconnect();
			}
		}

		public void WriteLine(string format, params object[] args)
		{
			writer.WriteLine(format, args);
		}

		public void WriteLine(string line)
		{
			writer.WriteLine(line);
		}

		public void Close()
		{
			disposed = true;
		}

		void IDisposable.Dispose()
		{
			Close();
		}
	}
}
