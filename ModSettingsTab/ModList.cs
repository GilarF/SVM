using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StardewModdingAPI;

namespace ModSettingsTab
{
    public class ModList : Dictionary<string, Mod>
    {
        public ModList()
        {
            var path = Path.Combine(ModEntry.Helper.DirectoryPath, "..");
            Parallel.ForEach(Directory.GetDirectories(path), directory =>
            {
                try
                {
                    var configPath = Path.Combine(directory, "config.json");
                    var manifestPath = Path.Combine(directory, "manifest.json");
                    // necessary files exist
                    if (!File.Exists(configPath) || !File.Exists(manifestPath)) return;
                    var uniqueId = JObject.Parse(File.ReadAllText(manifestPath))["UniqueID"].ToString();
                    // check if the mod is loaded without errors
                    if (!ModEntry.Helper.ModRegistry.IsLoaded(uniqueId)) return;
                    var jObj = JObject.Parse(configPath);
                    var staticConfig = new StaticConfig(configPath, jObj);
                    Add(uniqueId, new Mod(uniqueId, directory, staticConfig));
                }
                catch (Exception e)
                {
                    ModEntry.Console.Log(e.Message, LogLevel.Warn);
                }
            });
        }
    }
}