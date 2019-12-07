using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace ModSettingsTab
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ModEntry : StardewModdingAPI.Mod
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
            // mod data initialization
            Helper.Events.GameLoop.GameLaunched += (sender, args) => ModData.Init();
        }

        /// <summary>
        /// handler of the change event of the active menu
        /// replaces the settings tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (!(e.NewMenu is GameMenu menu)) return;
            int offset;
            switch (LocalizedContentManager.CurrentLanguageCode)
            {
                case LocalizedContentManager.LanguageCode.ru:
                    offset = 96;
                    break;
                case LocalizedContentManager.LanguageCode.fr:
                case LocalizedContentManager.LanguageCode.tr:
                    offset = 192;
                    break;
                default:
                    offset = 0;
                    break;
            }

            menu.pages[GameMenu.optionsTab] = new Menu.OptionsPage(
                menu.xPositionOnScreen,
                menu.yPositionOnScreen,
                menu.width + offset,
                menu.height);
        }
    }
}