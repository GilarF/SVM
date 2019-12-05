using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.Menus;

namespace ModSettingsTab
{
    public class ModEntry : Mod
    {
        /// <summary>
        /// SMAPI helper
        /// </summary>
        public new static IModHelper Helper;

        /// <summary>
        /// SMAPI monitor
        /// see: https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Logging
        /// </summary>
        public static IMonitor Console;

        /// <summary>
        /// SMAPI internationalization helper
        /// </summary>
        public static ITranslationHelper I18N;

        /// <summary>
        /// mod entry point
        /// </summary>
        /// <param name="helper"></param>
        public override void Entry(IModHelper helper)
        {
            Helper = helper;
            Console = Monitor;
            I18N = helper.Translation;

            Helper.Events.Display.MenuChanged += MenuChanged;
        }

        /// <summary>
        /// handler of the change event of the active menu
        /// replaces the settings tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is GameMenu menu)
            {
//                menu.tabs.Insert(GameMenu.optionsTab, new OptionsPage());
            }
        }
    }
}