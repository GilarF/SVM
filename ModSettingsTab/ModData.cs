using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            ModList = new ModList();
            Options = ModList.SelectMany(mod => mod.Value.Options).ToList();
            UpdateFavoriteOptions();
        }

        /// <summary>
        /// asynchronously updates the list of settings of selected mods
        /// </summary>
        private static void UpdateFavoriteOptions()
        {
            Task.Run(() =>
            {
                FavoriteOptions = ModList.Where(mod => mod.Value.Favorite)
                    .SelectMany(mod => mod.Value.Options).ToList();
            });
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
            UpdateFavoriteOptions();
        }
    }
}