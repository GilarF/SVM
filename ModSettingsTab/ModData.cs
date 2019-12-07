using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StardewValley.Menus;
using OptionsElement = ModSettingsTab.Framework.Components.OptionsElement;

namespace ModSettingsTab
{
    public static class ModData
    {
        public static TextBox CurrentTextBox { get; set; }
        public static ModList ModList { get; private set; }
        public static List<OptionsElement> Options;
        public static List<OptionsElement> FavoriteOptions;

        public static void Init()
        {
            ModList = new ModList();
            Options = ModList.SelectMany(mod => mod.Value.Options).ToList();
            UpdateFavoriteOptions();
        }

        private static void UpdateFavoriteOptions()
        {
            Task.Run(() =>
            {
                FavoriteOptions = ModList.Where(mod => mod.Value.Favorite)
                    .SelectMany(mod => mod.Value.Options).ToList();
            });
        }

        public static void ChangeFavoriteMod(string uniqueId, bool status)
        {
            ModList[uniqueId].Favorite = status;
            UpdateFavoriteOptions();
        }
    }
}