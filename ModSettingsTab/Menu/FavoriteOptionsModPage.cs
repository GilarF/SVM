namespace ModSettingsTab.Menu
{
    public class FavoriteOptionsModPage : BaseOptionsModPage
    {
        public FavoriteOptionsModPage(int x, int y, int width, int height, int id) : base(x, y, width, height)
        {
            Options = ModData.FavoriteMod[id].Options;
        }
    }
}