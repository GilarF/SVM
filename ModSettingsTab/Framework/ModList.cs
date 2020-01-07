using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json.Linq;
using StardewModdingAPI;

namespace ModSettingsTab.Framework
{
    /// <summary>
    /// main collection of modifications
    /// </summary>
    /// <remarks>
    /// Dictionary&lt;uniqueId, Mod&gt;
    /// </remarks>
    public class ModList : Dictionary<string, Mod>
    {
        public ModList()
        {
            var path = Path.Combine(Constants.ExecutionPath, "Mods");
            var directories = Directory.GetDirectories(path);
            foreach (var directory in directories)
            {
                try
                {
                    var configPath = Path.Combine(directory, "config.json");
                    var manifestPath = Path.Combine(directory, "manifest.json");
                    // necessary files exist
                    if (!File.Exists(configPath) || !File.Exists(manifestPath)) continue;
                    var uniqueId = JObject.Parse(File.ReadAllText(manifestPath))["UniqueID"].ToString();
                    // reading and parsing config.json
                    var jObj = JObject.Parse(File.ReadAllText(configPath));
                    var staticConfig = new StaticConfig(configPath, jObj, uniqueId);
                    Add(uniqueId, new Mod(uniqueId, directory, staticConfig));
                }
                catch (Exception e)
                {
                    Helper.Console.Warn(e.Message);
                }
            }
        }
    }
}