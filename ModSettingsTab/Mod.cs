using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ModSettingsTab.Framework.Components;
using ModSettingsTab.Menu;
using Newtonsoft.Json.Linq;
using StardewModdingAPI;

namespace ModSettingsTab
{
    public class Mod
    {
        public readonly List<OptionsElement> Options;
        private readonly StaticConfig _staticConfig;
        private bool _favorite;
        public IManifest Manifest { get; }

        public bool Favorite
        {
            get => _favorite;
            set
            {
                FavoriteData.ChangeStatus(Manifest.UniqueID);
                _favorite = value;
            }
        }

        public Mod(string uniqueId, string directory, StaticConfig config)
        {
            Options = new List<OptionsElement>();
            Manifest = ModEntry.Helper.ModRegistry.Get(uniqueId).Manifest;
            _staticConfig = config;
            _favorite = FavoriteData.IsFavorite(Manifest.UniqueID);
            InitOptions(directory);
        }

        private void InitOptions(string folder)
        {
            var uniqueId = Manifest.UniqueID;

            Options.Add(new OptionsHeading(uniqueId, Manifest, BaseOptionsModPage.SlotSize));
            foreach (KeyValuePair<string, JToken> param in _staticConfig)
            {
                StaticInit(param.Value, param.Key);
            }

            void StaticInit(JToken opt, string name)
            {
                switch (opt.Type)
                {
                    case JTokenType.Integer:
                        Options.Add(new OptionsTextBox(name, uniqueId, name, _staticConfig, BaseOptionsModPage.SlotSize,
                            false, true));
                        break;
                    case JTokenType.Float:
                        Options.Add(new OptionsTextBox(name, uniqueId, name, _staticConfig, BaseOptionsModPage.SlotSize,
                            true));
                        break;
                    case JTokenType.String:
                        var str = opt.ToString().Trim();

                        // check int
                        if (int.TryParse(str, out _))
                        {
                            Options.Add(new OptionsTextBox(name, uniqueId, name, _staticConfig,
                                BaseOptionsModPage.SlotSize, false, true, true));
                            break;
                        }

                        // check button
                        if (ButtonTryParse(str, out var btn))
                        {
                            Options.Add(new OptionsInputListener(name, uniqueId, name, _staticConfig,
                                BaseOptionsModPage.SlotSize, btn));
                            break;
                        }

                        // check bool
                        if (str.ToLower().Equals("true") || str.ToLower().Equals("false"))
                        {
                            Options.Add(new OptionsCheckbox(name, uniqueId, name, _staticConfig,
                                BaseOptionsModPage.SlotSize)
                            {
                                AsString = true
                            });
                            break;
                        }

                        // check float
                        if (float.TryParse(str, NumberStyles.Any, CultureInfo.CreateSpecificCulture("en-US"), out _))
                        {
                            Options.Add(new OptionsTextBox(name, uniqueId, name, _staticConfig,
                                BaseOptionsModPage.SlotSize, true, false, true));
                            break;
                        }

                        Options.Add(
                            new OptionsTextBox(name, uniqueId, name, _staticConfig, BaseOptionsModPage.SlotSize));
                        break;
                    case JTokenType.Boolean:
                        Options.Add(new OptionsCheckbox(name, uniqueId, name, _staticConfig,
                            BaseOptionsModPage.SlotSize));
                        break;
                }
            }
        }

        private static bool ButtonTryParse(string str, out SButton btn)
        {
            if (!str.Contains(",") && Enum.TryParse<SButton>(str.Trim(), true, out var result))
            {
                btn = result;
                return true;
            }

            btn = SButton.None;
            return false;
        }
    }
}