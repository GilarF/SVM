using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;

namespace ModSettingsTab.Menu
{
    public class OptionsPage : IClickableMenu
    {
        private string _hoverText = "";
        private readonly List<ClickableTextureComponent> _sideTabs = new List<ClickableTextureComponent>();
        private readonly Dictionary<int, IClickableMenu> _pagesCollections = new Dictionary<int, IClickableMenu>();
        private static readonly Texture2D Tabs = ModEntry.Helper.Content.Load<Texture2D>("assets/Tabs.png");
        private const int WidthToMoveActiveTab = 8;
        private const int RegionOriginalOptions = 7001;
        private const int RegionOptionsMod = 7002;
        private const int RegionFavoriteOptionsMod = 7003;
        private const int OriginalOptionsTab = 0;
        private const int ModOptionsTab = 1;
        private const int FavoriteModOptionsTab = 2;
        private const int DistanceFromMenuBottomBeforeNewPage = 128;
        private int _currentTab;
        private int _value;

        public OptionsPage(int x, int y, int width, int height) : base(x, y, width, height, true)
        {
            // move close button left
            upperRightCloseButton.bounds.X -= 42;
            // -------- standard options tab ---------
            var originalOptionsComponent = new ClickableTextureComponent("",
                new Rectangle(
                    xPositionOnScreen - 48 + WidthToMoveActiveTab,
                    yPositionOnScreen + DistanceFromMenuBottomBeforeNewPage,
                    64, 64), "",
                Game1.content.LoadString("Strings\\UI:GameMenu_Options"), Tabs,
                new Rectangle(0, 0, 16, 16),
                4f)
            {
                myID = RegionOriginalOptions,
                downNeighborID = RegionOptionsMod,
                rightNeighborID = 0
            };
            _sideTabs.Add(originalOptionsComponent);
            _pagesCollections.Add(OriginalOptionsTab, new StardewValley.Menus.OptionsPage(x, y, width, height));

            // -------- mod options tab ---------
            var modOptionsComponent = new ClickableTextureComponent("",
                new Rectangle(
                    xPositionOnScreen - 48,
                    yPositionOnScreen + DistanceFromMenuBottomBeforeNewPage + 64,
                    64, 64),
                "",
                ModEntry.I18N.Get("OptionsPage:Tab_StaticSettings"), Tabs,
                new Rectangle(16, 0, 16, 16), 4f)
            {
                myID = RegionOptionsMod,
                upNeighborID = RegionOriginalOptions,
                downNeighborID = RegionFavoriteOptionsMod,
                rightNeighborID = 0
            };
            _sideTabs.Add(modOptionsComponent);
            _pagesCollections.Add(ModOptionsTab, new OptionsModPage(x, y, width, height));

            // -------- favorite mod tab ---------
            var favoriteModComponent = new ClickableTextureComponent("",
                new Rectangle(
                    xPositionOnScreen - 48,
                    yPositionOnScreen + DistanceFromMenuBottomBeforeNewPage + 64 * 2, 64, 64), "",
                ModEntry.I18N.Get("OptionsPage:Tab_DynamicSettings"), Tabs,
                new Rectangle(32, 0, 16, 16), 4f)
            {
                myID = RegionFavoriteOptionsMod,
                upNeighborID = RegionOptionsMod,
                rightNeighborID = 0
            };
            _sideTabs.Add(favoriteModComponent);
            _pagesCollections.Add(FavoriteModOptionsTab, new StardewValley.Menus.OptionsPage(x, y, width, height));
        }

        protected override void customSnapBehavior(int direction, int oldRegion, int oldId)
        {
            base.customSnapBehavior(direction, oldRegion, oldId);
        }

        public override bool shouldDrawCloseButton() => false;

        public override void receiveScrollWheelAction(int direction)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.receiveScrollWheelAction(direction);
            _pagesCollections[_currentTab].receiveScrollWheelAction(direction);
        }

        public override void snapToDefaultClickableComponent()
        {
            base.snapToDefaultClickableComponent();
            currentlySnappedComponent = getComponentWithID(0);
            snapCursorToCurrentSnappedComponent();
            _pagesCollections[_currentTab].snapToDefaultClickableComponent();
        }

        public override void leftClickHeld(int x, int y)
        {
            base.leftClickHeld(x, y);
            _pagesCollections[_currentTab].leftClickHeld(x, y);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.receiveLeftClick(x, y, playSound);
            for (var index = 0; index < _sideTabs.Count; ++index)
            {
                if (!_sideTabs[index].containsPoint(x, y) || _currentTab == index) continue;
                Game1.playSound("smallSelect");
                _sideTabs[_currentTab].bounds.X -= WidthToMoveActiveTab;
                _currentTab = index;
                _sideTabs[index].bounds.X += WidthToMoveActiveTab;
            }

            _pagesCollections[_currentTab].receiveLeftClick(x, y, playSound);
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            if (GameMenu.forcePreventClose)
                return;
            _pagesCollections[_currentTab].receiveRightClick(x, y, playSound);
        }

        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction(x,y);
            _hoverText = "";
            _value = -1;
            using (var enumerator = _sideTabs.Where(sideTab => sideTab.containsPoint(x, y)).GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    _hoverText = enumerator.Current?.hoverText;
                    return;
                }
            }

            _pagesCollections[_currentTab].performHoverAction(x, y);
        }

        public override void releaseLeftClick(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.releaseLeftClick(x, y);
            _pagesCollections[_currentTab].releaseLeftClick(x, y);
        }

        public override void snapCursorToCurrentSnappedComponent()
        {
            _pagesCollections[_currentTab].snapCursorToCurrentSnappedComponent();
        }

        public override void receiveKeyPress(Keys key)
        {
            _pagesCollections[_currentTab].receiveKeyPress(key);
            base.receiveKeyPress(key);
        }

        public override void draw(SpriteBatch b)
        {
            upperRightCloseButton.draw(b);
            foreach (var sideTab in _sideTabs)
                sideTab.draw(b);
            b.End();
            b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp,
                null, null, null, new Matrix?());
            _pagesCollections[_currentTab].draw(b);
            b.End();
            b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null,
                null, null, new Matrix?());
            if (_hoverText.Equals(""))
                return;
            drawHoverText(b, _hoverText, Game1.smallFont, 0, 0, _value);
        }
    }
}