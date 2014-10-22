using System;
using System.Linq;

namespace CSBot
{
	public static class Utils
	{
		// stay on the caller's appdomain, may go away when i fix ModuleManager to properly resolve things in the main appdomain

		public static T Get<T>(this ModuleManager moduleManager) where T : CSBotModule
		{
			return moduleManager.LoadedModules.Values.Select(m => m.Module as T).FirstOrDefault(m => m != null);
		}

		public static void InvokeModules(this ModuleManager moduleManager, Action<CSBotModule> func)
		{
			foreach (var module in moduleManager.LoadedModules)
			{
				try
				{
					func(module.Value.Module);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
		}
	}
}
