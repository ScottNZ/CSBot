using System;
using System.IO;
using System.Net.Sockets;

namespace CSBot
{
	public class IrcClient : MarshalByRefObject, IDisposable
	{
		public string Nickname { get; private set; }
		public IrcClientSetup Setup { get; private set; }
		public ModuleManager ModuleManager { get; private set; }

		TcpClient socket;
		NetworkStream stream;
		StreamReader reader;
		StreamWriter writer;
		bool disposed;

		public IrcClient(ModuleManager moduleManager, IrcClientSetup setup)
		{
			ModuleManager = moduleManager;
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
			RootInvokeModules(m => m.OnConnect(this));
			try
			{
				string l;
				while (!disposed && (l = reader.ReadLine()) != null)
				{
					var line = l;
					Console.WriteLine(line);
					RootInvokeModules(m => m.OnLineRead(this, line));
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
				RootInvokeModules(m => m.OnDisconnect(this));
			}
		}

		void RootInvokeModules(Action<CSBotModule> func)
		{
			foreach (var module in ModuleManager.LoadedModules)
				func(module.Value.Module);
			ModuleManager.ProcessDeferredModuleLoads();
		}

		public void WriteLine(string format, params object[] args)
		{
			writer.WriteLine(format, args);
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
