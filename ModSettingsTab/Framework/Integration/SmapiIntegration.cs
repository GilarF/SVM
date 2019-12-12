using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ModSettingsTab.Framework.Components;
using ModSettingsTab.Menu;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StardewModdingAPI;
using StardewValley;

namespace ModSettingsTab.Framework.Integration
{
    public class SmapiIntegration
    {
        public readonly List<OptionsElement> Options = new List<OptionsElement>();
        private readonly StaticConfig _staticConfig;

        public SmapiIntegration()
        {
            var configPath = Path.Combine(Constants.ExecutionPath, "smapi-internal/config.json");
            if (!File.Exists(configPath))
            {
                ModEntry.Console.Log("SMAPI Config not found? :)", LogLevel.Error);
                return;
            }

            var json = File.ReadAllText(configPath);
            json = Regex.Replace(json, "\\/{2}\"(ParanoidWarnings|UseBetaChannel)\": true,", "\"$1\": false,");
            var jObj = JObject.Parse(json);
            _staticConfig = new StaticConfig(configPath, jObj);

            InitOptions();
        }

        private void InitOptions()
        {
            const string uniqueId = "Pathoschild.SMAPI";
            var lang = LocalizedContentManager.CurrentLanguageCode;
            
            var json = File.ReadAllText(Path.Combine(ModEntry.Helper.DirectoryPath, "data/SmapiIntegrations.json"));
            var integration = JsonConvert.DeserializeObject<ModIntegrationSettings>(json) ??
                              new ModIntegrationSettings();
            Options.Add(new SmapiHeading(BaseOptionsModPage.SlotSize)
            {
                Label = "SMAPI",
                HoverText = integration.Description[lang] ??
                            "The default values are mirrored in StardewModdingAPI.Framework.Models.SConfig to log custom changes.",
                HoverTitle = "Pathoschild",
            });
            foreach (KeyValuePair<string, JToken> param in _staticConfig)
            {
                var iOpt = integration.Config.FindLast(o => o.Name == param.Key);
                if (iOpt == null)
                {
                    StaticInit(param.Value, param.Key);
                    continue;
                }

                if (iOpt.Ignore) continue;
                switch (iOpt.Type)
                {
                    case ParamType.CheckBox:
                        Options.Add(new OptionsCheckbox(param.Key, uniqueId, iOpt.Label ?? param.Key,
                            _staticConfig,
                            BaseOptionsModPage.SlotSize)
                        {
                            HoverText = iOpt.Description?[lang],
                            ShowTooltip = !string.IsNullOrEmpty(iOpt.Description?[lang])
                        });
                        continue;
                    case ParamType.DropDown:
                        Options.Add(new OptionsDropDown(param.Key, uniqueId, iOpt.Label ?? param.Key,
                            _staticConfig, BaseOptionsModPage.SlotSize, iOpt.DropDownOptions)
                        {
                            HoverText = iOpt.Description?[lang],
                            ShowTooltip = !string.IsNullOrEmpty(iOpt.Description?[lang])
                        });
                        continue;
                    case ParamType.PlusMinus:
                        Options.Add(new OptionsPlusMinus(param.Key, uniqueId, iOpt.Label ?? param.Key,
                            _staticConfig, BaseOptionsModPage.SlotSize, iOpt.PlusMinusOptions??new List<string>())
                        {
                            HoverText = iOpt.Description?[lang],
                            ShowTooltip = !string.IsNullOrEmpty(iOpt.Description?[lang])
                        });
                        continue;
                    default:
                        StaticInit(param.Value, param.Key);
                        continue;
                }
            }

            void StaticInit(JToken opt, string name)
            {
                switch (opt.Type)
                {
                    case JTokenType.String:
                        var str = opt.ToString().Trim();
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