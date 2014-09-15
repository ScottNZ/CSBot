﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CSBot
{
	public class LoadedModule : MarshalByRefObject
	{
		public AppDomain Domain { get; internal set; }
		public CSBotModule Module { get; internal set; }
	}

	public class ModuleManager : MarshalByRefObject
	{
		class ModuleLoader : MarshalByRefObject
		{
			public CSBotModule LoadModule(string path)
			{
				var type = Assembly.Load(File.ReadAllBytes(path))
					.GetTypes()
					.FirstOrDefault(typeof(CSBotModule).IsAssignableFrom);

				if (type == null)
					return null;

				AppDomain.CurrentDomain.AssemblyResolve += ResolveModule;

				return (CSBotModule)Activator.CreateInstance(type);
			}

			Assembly ResolveModule(object sender, ResolveEventArgs args)
			{
				var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
				var modulesDirectory = Path.Combine(appDirectory, "Modules");
				var moduleFilename = Path.Combine(modulesDirectory, new AssemblyName(args.Name).Name + ".dll");
				return Assembly.Load(File.ReadAllBytes(moduleFilename));
			}
		}

		class DeferredModuleLoad
		{
			public bool IsLoad;
			public string Filename;
		}

		public IDictionary<string, LoadedModule> LoadedModules { get; private set; }
		readonly string appDirectory;
		readonly string modulesDirectory;
		readonly List<DeferredModuleLoad> defers = new List<DeferredModuleLoad>();

		public ModuleManager()
		{
			LoadedModules = new Dictionary<string, LoadedModule>();
			appDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			modulesDirectory = Path.Combine(appDirectory, "Modules");
			Directory.CreateDirectory(modulesDirectory);
		}

		public void LoadAllModules()
		{
			foreach (var file in Directory.EnumerateFiles(modulesDirectory, "*.dll"))
				LoadModule(Path.GetFileNameWithoutExtension(file));
		}

		public void LoadModule(string filename)
		{
			if (LoadedModules.ContainsKey(filename))
				UnloadModule(filename);

			var setup = new AppDomainSetup { ApplicationBase = appDirectory };
			var domain = AppDomain.CreateDomain(filename, null, setup);

			try
			{
				var path = Path.Combine(modulesDirectory, filename + ".dll");

				var loader = (ModuleLoader)domain.CreateInstanceAndUnwrap(typeof(ModuleLoader).Assembly.FullName, typeof(ModuleLoader).FullName);
				var module = loader.LoadModule(path);
				if (module != null)
				{
					LoadedModules.Add(filename, new LoadedModule { Domain = domain, Module = module });

					Console.WriteLine("Loaded {0}", filename);
				}
				else
					AppDomain.Unload(domain);
			}
			catch
			{
				AppDomain.Unload(domain);
				throw;
			}
		}

		public void UnloadModule(string filename)
		{
			LoadedModule module;
			if (!LoadedModules.TryGetValue(filename, out module))
				return;

			AppDomain.Unload(module.Domain);
			LoadedModules.Remove(filename);

			Console.WriteLine("Unloaded {0}", filename);
		}

		// Defer load/unload requests to prevent racing
		public void LoadModuleDeferred(string filename)
		{
			defers.Add(new DeferredModuleLoad { IsLoad = true, Filename = filename });
		}

		public void UnloadModuleDeferred(string filename)
		{
			defers.Add(new DeferredModuleLoad { IsLoad = false, Filename = filename });
		}

		public void ProcessDeferredModuleLoads()
		{
			foreach (var defer in defers)
			{
				if (defer.IsLoad)
					LoadModule(defer.Filename);
				else
					UnloadModule(defer.Filename);
			}
			defers.Clear();
		}
	}
}