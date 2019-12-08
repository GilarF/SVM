using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public static List<OptionsElement> FavoriteOptions;

        /// <summary>
        /// initialization of master data
        /// </summary>
        public static void Init()
        {
            LoadOptionsAsync();
            LoadIntegrationsAsync();
        }

        /// <summary>
        /// Changes favorite mods
        /// </summary>
        /// <param name="uniqueId">
        /// unique mod identifier
        /// </param>
        /// <param name="status"></param>
        public static void ChangeFavoriteMod(string uniqueId, bool status)
        {
            ModList[uniqueId].Favorite = status;
            UpdateFavoriteOptionsAsync();
        }

        private static async void LoadOptionsAsync()
        {
            await LoadOptions();
            ModEntry.Console.Log($"Load {ModList.Count} mods and {Options.Count} Options", LogLevel.Info);
        }

        private static async void LoadIntegrationsAsync()
        {
            await LoadIntegrations();
            ModEntry.Console.Log($"Load {PredefinedIntegration.Count} Integrations", LogLevel.Info);
        }

        /// <summary>
        /// asynchronously updates the list of settings of selected mods
        /// </summary>
        private static async void UpdateFavoriteOptionsAsync()
        {
            await LoadFavoriteOptions();
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
                FavoriteOptions = ModList.Where(mod => mod.Value.Favorite)
                    .SelectMany(mod => mod.Value.Options).ToList();
            });
        }

        private static Task LoadFavoriteOptions()
        {
            return Task.Run(() =>
            {
                FavoriteOptions = ModList.Where(mod => mod.Value.Favorite)
                    .SelectMany(mod => mod.Value.Options).ToList();
            });
        }
    }
}