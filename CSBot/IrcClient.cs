using System;
using System.IO;
using System.Net.Sockets;

namespace CSBot
{
	public class IrcClient : MarshalByRefObject, IDisposable
	{
		public IrcClientSetup Setup { get; private set; }

		readonly ModuleManager moduleManager;
		TcpClient socket;
		NetworkStream stream;
		StreamReader reader;
		StreamWriter writer;
		bool disposed;

		public override object InitializeLifetimeService() { return null; }

		public IrcClient(ModuleManager moduleManager, IrcClientSetup setup)
		{
			this.moduleManager = moduleManager;
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
			moduleManager.InvokeRootModules(m => m.OnConnect(this));
			try
			{
				string l;
				while (!disposed && (l = reader.ReadLine()) != null)
				{
					var line = l;
					Console.WriteLine(line);
					moduleManager.InvokeRootModules(m => m.OnLineRead(this, line));
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
				moduleManager.InvokeRootModules(m => m.OnDisconnect(this));
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
