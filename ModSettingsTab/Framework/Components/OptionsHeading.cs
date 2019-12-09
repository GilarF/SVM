using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.BellsAndWhistles;

namespace ModSettingsTab.Framework.Components
{
    public class OptionsHeading : OptionsElement
    {
        private static readonly Rectangle Hl = new Rectangle(325, 318, 11, 18);
        private static readonly Rectangle Hc = new Rectangle(337, 318, 1, 18);
        private static readonly Rectangle Hr = new Rectangle(338, 318, 12, 18);

        public OptionsHeading(string modId, IManifest manifest, Point slotSize)
            : base("", modId, "", null, 32, 16, slotSize.X, slotSize.Y + 4)
        {
            var length1 = manifest.Name.Length;
            var length2 = manifest.Name.Length;
            Label = length1 + length2 <= 32 || length1 <= 25 
                ? $"{manifest.Name} v.{manifest.Version}"
                : $"{manifest.Name.Substring(0, 25)}... v{manifest.Version}";
        }

        public override void Draw(SpriteBatch b, int slotX, int slotY)
        {
            b.Draw(Game1.mouseCursors, new Rectangle(slotX + 32, slotY + Bounds.Y, 44, 72), Hl, Color.White);
            b.Draw(Game1.mouseCursors, new Rectangle(slotX + 32 + 44, slotY + Bounds.Y, Bounds.Width - 64 - 48 - 44, 72), Hc, Color.White);
            b.Draw(Game1.mouseCursors, new Rectangle(slotX + 32 + Bounds.Width - 64 - 48, slotY + Bounds.Y, 48, 72), Hr, Color.White);
            SpriteText.drawString(b, Label, slotX + 32 + 44 + 32, slotY + Bounds.Y + 12, 999, Bounds.Width - 64 - 48, 72, 1f, 0.1f);
        }
    }
}