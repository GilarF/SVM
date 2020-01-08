using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModSettingsTab.Framework;
using StardewValley;
using StardewValley.Menus;

namespace ModSettingsTab.Menu
{
    public class TitleOptionsPage : BaseOptionsPage
    {
        public TitleOptionsPage() : base(
            (int)((Game1.viewport.Width *1.1f - (800 + borderWidth * 2 + ModData.Offset)) / 2f),
            (int)((Game1.viewport.Height * 1.1f - (600 + borderWidth * 2)) / 2f - 48),
            800 + borderWidth * 2 + ModData.Offset,
            600 + borderWidth * 2)
        {
            SideTabs[0].bounds.X += WidthToMoveActiveTab;
            // -------- favorite mod tab ---------
            UpdateFavoriteTabs();
            FavoriteData.UpdateMod = UpdateFavoriteTabs;
            ResetTab(SavedTab);
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            TitleMenu.subMenu = null;
        }

        public override void releaseLeftClick(int x, int y)
        {
            base.releaseLeftClick((int)(x * 1.1f), (int)(y*1.1f));
        }

        public override void leftClickHeld(int x, int y)
        {
            base.leftClickHeld((int)(x * 1.1f), (int)(y*1.1f));
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick((int)(x * 1.1f), (int)(y*1.1f), playSound);
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            base.receiveRightClick((int)(x * 1.1f), (int)(y*1.1f), playSound);
        }

        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction((int)(x * 1.1f), (int)(y*1.1f));
        }

        public override void draw(SpriteBatch b)
        {
            var viewport = Game1.graphics.GraphicsDevice.Viewport;
            b.Draw(Game1.fadeToBlackRect, viewport.Bounds, Color.Multiply(Color.Black, 0.6f));
            b.End();
            b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null,
                Matrix.CreateScale(0.9f));
            Game1.drawDialogueBox(xPositionOnScreen, yPositionOnScreen, width, height, false, true);
            base.draw(b);
        }
    }
}