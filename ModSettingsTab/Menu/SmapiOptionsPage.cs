namespace ModSettingsTab.Menu
{
    public class SmapiOptionsPage : BaseOptionsModPage
    {
        public SmapiOptionsPage(int x, int y, int width, int height) : base(x, y, width, height)
        {
            Options = ModData.SMAPI.Options;
        }
    }
}