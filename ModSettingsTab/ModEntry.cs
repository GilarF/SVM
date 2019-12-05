using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.Menus;

namespace ModSettingsTab
{
    public class ModEntry : Mod
    {
        public new static IModHelper Helper;
        public static IMonitor Console;
        public static ITranslationHelper I18N;

        public override void Entry(IModHelper helper)
        {
            Helper = helper;
            Console = Monitor;
            I18N = helper.Translation;

            Helper.Events.Display.MenuChanged += MenuChanged;
        }

        private void MenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is GameMenu menu)
            {
//                menu.tabs.Insert(GameMenu.optionsTab, new OptionsPage());
            }
        }
    }
}