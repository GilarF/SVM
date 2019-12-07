using System;
using System.Collections.Generic;
using System.IO;
using ModSettingsTab.Framework.Components;
using StardewModdingAPI;

namespace ModSettingsTab
{
    public class Mod
    {
        public List<OptionsElement> Options;
        private StaticConfig _staticConfig;
        private bool _favorite;
        public IManifest Manifest { get; }

        public bool Favorite
        {
            get => _favorite;
            set
            {
                FavoriteData.ChangeStatus(Manifest.UniqueID, value);
                _favorite = value;
            }
        }

        public Mod(string uniqueId, string directory, StaticConfig config)
        {
            Manifest = ModEntry.Helper.ModRegistry.Get(uniqueId).Manifest;
            _staticConfig = config;
            _favorite = FavoriteData.IsFavorite(Manifest.UniqueID);
            InitOptions(directory);
        }

        private void InitOptions(string folder)
        {
            var integrationPath = Path.Combine(folder, "settingsTab.json");

            try
            {
                if (File.Exists(integrationPath))
                {
                    // read parameters from file
                }
                else
                {
                    // check for built-in support
                }
            }
            catch (Exception e)
            {
                ModEntry.Console.Log(e.Message, LogLevel.Error);
            }
        }
    }
}