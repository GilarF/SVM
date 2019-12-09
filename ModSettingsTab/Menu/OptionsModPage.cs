namespace ModSettingsTab.Menu
{
    public class OptionsModPage : BaseOptionsModPage
    {
        public OptionsModPage(int x, int y, int width, int height) : base(x, y, width, height)
        {
            Options = ModData.Options;
        }
    }
}