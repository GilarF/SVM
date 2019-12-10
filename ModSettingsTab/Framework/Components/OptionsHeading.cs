using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;

namespace ModSettingsTab.Framework.Components
{
    public class OptionsHeading : OptionsElement
    {
        private readonly IManifest _manifest;
        private static readonly Rectangle Hl = new Rectangle(325, 318, 11, 18);
        private static readonly Rectangle Hc = new Rectangle(337, 318, 1, 18);
        private static readonly Rectangle Hr = new Rectangle(338, 318, 12, 18);
        private static readonly Rectangle Star0 = new Rectangle(310, 392, 16, 16);
        private static readonly Rectangle Star1 = new Rectangle(294, 392, 16, 16);
        private static Rectangle _boundsStar = new Rectangle(92, 36, 32, 32);
        private bool _hover;
        

        public OptionsHeading(string modId, IManifest manifest, Point slotSize)
            : base("", modId, "", null, 32, 16, slotSize.X, slotSize.Y + 4)
        {
            _manifest = manifest;
            var length = manifest.Name.Length;
            Label = length * 2 <= 32 || length <= 25
                ? $"{manifest.Name} v.{manifest.Version}"
                : $"{manifest.Name.Substring(0, 25)}... v{manifest.Version}";
        }

        public override void PerformHoverAction(int x, int y)
        {
            _hover = _boundsStar.Contains(x, y);
        }

        public override void Draw(SpriteBatch b, int slotX, int slotY)
        {
            var star = ModData.ModList[ModId].Favorite ? Star1 : Star0;

            b.Draw(Game1.mouseCursors, new Rectangle(slotX + 32, slotY + Bounds.Y, 44, 72), Hl, Color.White);
            b.Draw(Game1.mouseCursors,
                new Rectangle(slotX + 32 + 44, slotY + Bounds.Y, Bounds.Width - 64 - 48 - 44, 72), Hc, Color.White);
            b.Draw(Game1.mouseCursors, new Rectangle(slotX + 32 + Bounds.Width - 64 - 48, slotY + Bounds.Y, 48, 72), Hr,
                Color.White);
            SpriteText.drawString(b, Label, slotX + 32 + 44 + 64, slotY + Bounds.Y + 12, 999, Bounds.Width - 64 - 48,
                72, 1f, 0.1f);
            b.End();
            b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp,
                null, null, null, new Matrix?());
            b.Draw(Game1.mouseCursors, new Rectangle(slotX + _boundsStar.X, slotY + _boundsStar.Y, _boundsStar.Width, _boundsStar.Height), star,
                Color.White);
            b.End();
            b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null,
                null, null, new Matrix?());
            if (_hover)
                IClickableMenu.drawToolTip(b,_manifest.Description,_manifest.Author,null);
            b.End();
            b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp,
                null, null, null, new Matrix?());
        }
    }
}