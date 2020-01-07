using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ModSettingsTab.Framework;
using ModSettingsTab.Framework.Integration;
using ModSettingsTab.Framework.Components;

namespace ModSettingsTab
{
    public static class ModData
    {
        public const int Offset = 192;

        public static bool NeedReload = false;

        public static readonly Api Api;

        public static TabConfig Config;

        /// <summary>
        /// collection of loaded mods, only those that have settings
        /// </summary>
        public static ModList ModList { get; private set; }

        /// <summary>
        /// list of all modification settings
        /// </summary>
        public static List<OptionsElement> Options;

        public static SmapiIntegration SMAPI;

        public static readonly Texture2D Tabs;

        static ModData()
        {
            Api = new Api();
            Config = Helper.ReadConfig<TabConfig>();
            Tabs = Helper.Content.Load<Texture2D>("assets/Tabs.png");
        }

        /// <summary>
        /// initialization of master data
        /// </summary>
        public static async void Init()
        {
            await LoadOptions();
            Helper.Console.Info($"Load {ModList.Count} mods and {Options.Count} Options");
        }

        private static Task LoadOptions()
        {
            return Task.Run(() =>
            {
                ModList = new ModList();
                SMAPI = new SmapiIntegration();
                Options = ModList.SelectMany(mod => mod.Value.Options).ToList();
                FavoriteData.LoadOptions();
            });
        }
    }
}