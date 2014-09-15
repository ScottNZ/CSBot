using System;
using CSBot;
using IrcLogicModule;

namespace ModuleLoaderModule
{
    public class ModuleLoader : CSBotModule
    {
	    public override void OnPublicMessage(IrcClient client, string user, string target, string message)
	    {
		    if (user == client.Setup.AdminUser)
		    {
			    if (message.StartsWith("!loadmod ", StringComparison.OrdinalIgnoreCase))
			    {
				    var filename = message.Substring(message.IndexOf(' ') + 1);
				    client.ModuleManager.LoadModuleDeferred(filename);
			    }
			    else if (message.StartsWith("!unloadmod ", StringComparison.OrdinalIgnoreCase))
			    {
				    var filename = message.Substring(message.IndexOf(' ') + 1);
					client.ModuleManager.UnloadModuleDeferred(filename);
			    }
			    else if (message.Equals("!loadedmods", StringComparison.OrdinalIgnoreCase))
				    client.SendMessage(target, string.Join(", ", client.ModuleManager.LoadedModules.Keys));
		    }
	    }
    }
}