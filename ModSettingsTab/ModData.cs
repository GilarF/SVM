using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModSettingsTab.Framework.Integration;
using StardewModdingAPI;
using StardewValley.Menus;
using OptionsElement = ModSettingsTab.Framework.Components.OptionsElement;

namespace ModSettingsTab
{
    public static class ModData
    {
        /// <summary>
        /// selected text field to reset and hold focus
        /// </summary>
        public static TextBox CurrentTextBox { get; set; }

        public const int Offset = 192;

        /// <summary>
        /// collection of loaded mods, only those that have settings
        /// </summary>
        public static ModList ModList { get; private set; }

        /// <summary>
        /// predefined collection of mod settings
        /// </summary>
        /// <remarks>
        /// Dictionary&lt;string:UniqueId, ModIntegrationSettings&gt;
        /// </remarks>
        public static Dictionary<string, ModIntegrationSettings> PredefinedIntegration;

        /// <summary>
        /// list of all modification settings
        /// </summary>
        public static List<OptionsElement> Options;

        /// <summary>
        /// list of favorite mod options
        /// </summary>
        public static List<Mod> FavoriteMod;
        public static readonly Texture2D Tabs;
        public static List<Rectangle> FavoriteTabSource;

        public delegate void Update();

        public static Update UpdateFavoriteMod;

        static ModData()
        {
            Tabs = ModEntry.Helper.Content.Load<Texture2D>("assets/Tabs.png");
            FavoriteTabSource = new List<Rectangle>()
            {
                new Rectangle(0,128,32,24),
                new Rectangle(32,128,32,24),
                new Rectangle(0,152,32,24),
                new Rectangle(32,152,32,24),
                new Rectangle(0,176,32,24),
            };
        }

        /// <summary>
        /// initialization of master data
        /// </summary>
        public static async void Init()
        {
            await LoadIntegrations();
            ModEntry.Console.Log($"Load {PredefinedIntegration.Count} Integrations", LogLevel.Info);
            await LoadOptions();
            ModEntry.Console.Log($"Load {ModList.Count} mods and {Options.Count} Options | {FavoriteMod.Count}",
                LogLevel.Info);
        }

        /// <summary>
        /// asynchronously updates the list of settings of selected mods
        /// </summary>
        public static async void UpdateFavoriteOptionsAsync()
        {
            await Task.Run(LoadFavoriteOptions);
            UpdateFavoriteMod();
        }

        private static Task LoadIntegrations()
        {
            return Task.Run(() =>
            {
                PredefinedIntegration =
                    ModEntry.Helper.Data.ReadJsonFile<Dictionary<string, ModIntegrationSettings>>(
                        "data/integration.json") ?? new Dictionary<string, ModIntegrationSettings>();
            });
        }

        private static Task LoadOptions()
        {
            return Task.Run(() =>
            {
                ModList = new ModList();
                Options = ModList.SelectMany(mod => mod.Value.Options).ToList();
                LoadFavoriteOptions();
            });
        }

        private static void LoadFavoriteOptions()
        {
            FavoriteMod = ModList.Where(mod => mod.Value.Favorite)
                .Select(mod => mod.Value).ToList();
        }
    }
}