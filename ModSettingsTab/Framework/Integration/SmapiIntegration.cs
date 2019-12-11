using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ModSettingsTab.Framework.Components;
using ModSettingsTab.Menu;
using Newtonsoft.Json.Linq;
using StardewModdingAPI;

namespace ModSettingsTab.Framework.Integration
{
    public class SmapiIntegration
    {
        public readonly List<OptionsElement> Options = new List<OptionsElement>();
        private readonly StaticConfig _staticConfig;
        
        public SmapiIntegration()
        {
            var configPath = Path.Combine(ModEntry.Helper.DirectoryPath+ "/../../","smapi-internal/config.json");
            if (!File.Exists(configPath))
            {
                ModEntry.Console.Log("SMAPI Config not found? :)",LogLevel.Error);
                return;
            }
            var jObj = JObject.Parse(File.ReadAllText(configPath));
            _staticConfig = new StaticConfig(configPath, jObj);

            InitOptions();
        }
        private void InitOptions()
        {
            
            Options.Add(new SmapiHeading(BaseOptionsModPage.SlotSize)
            {
                Label = "SMAPI",
                HoverText = "The default values are mirrored in StardewModdingAPI.Framework.Models.SConfig to log custom changes.",
                HoverTitle = "Pathoschild",
            });
            foreach (KeyValuePair<string, JToken> param in _staticConfig)
            {
                StaticInit(param.Value, param.Key);
            }

            void StaticInit(JToken opt, string name)
            {
                var uniqueId = "Pathoschild.SMAPI";
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
    }
}