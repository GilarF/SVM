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
        private readonly List<ClickableTextureComponent> _favoriteSideTabs = new List<ClickableTextureComponent>();
        private readonly Dictionary<int, IClickableMenu> _pagesCollections = new Dictionary<int, IClickableMenu>();
        private readonly List<IClickableMenu> _favoritePagesCollections = new List<IClickableMenu>();
       
        
        private const int TabHeight = 64;
        private const int FavoriteTabSize = 48;
        private const int WidthToMoveActiveTab = 16;
        private const int RegionOriginalOptions = 7001;
        private const int RegionOptionsMod = 7002;
        private const int RegionFavoriteOptionsMod = 7003;
        private const int OriginalOptionsTab = 0;
        private const int ModOptionsTab = 1;
        private const int SmapiOptionsTab = 2;
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
                Game1.content.LoadString("Strings\\UI:GameMenu_Options"), ModData.Tabs,
                new Rectangle(32, 176, 16, 16),
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
                    yPositionOnScreen + DistanceFromMenuBottomBeforeNewPage + TabHeight,
                    64, 64),
                "",
                ModEntry.I18N.Get("OptionsPage:Tab_StaticSettings"), ModData.Tabs,
                new Rectangle(0, 0, 64, 64), 1f)
            {
                myID = RegionOptionsMod,
                upNeighborID = RegionOriginalOptions,
                downNeighborID = RegionFavoriteOptionsMod,
                rightNeighborID = 0
            };
            _sideTabs.Add(modOptionsComponent);
            _pagesCollections.Add(ModOptionsTab, new OptionsModPage(x, y, width, height));
            // -------- SMAPI tab ---------
            var smapiOptionsComponent = new ClickableTextureComponent("",
                new Rectangle(
                    xPositionOnScreen - 48,
                    yPositionOnScreen + height - DistanceFromMenuBottomBeforeNewPage,
                    64, 64), "",
                "SMAPI", ModData.Tabs,
                new Rectangle(0, 64, 64, 64),
                1f)
            {
                myID = RegionFavoriteOptionsMod + _favoriteSideTabs.Count,
                downNeighborID = 0,
                rightNeighborID = 0
            };
            _sideTabs.Add(smapiOptionsComponent);
            _pagesCollections.Add(SmapiOptionsTab, new StardewValley.Menus.OptionsPage(x, y, width, height));

            // -------- favorite mod tab ---------
            InitFavoriteTabs();
            ModData.UpdateFavoriteMod = InitFavoriteTabs;
        }

        private void InitFavoriteTabs()
        {
            _favoriteSideTabs.Clear();
            _favoritePagesCollections.Clear();
            var fModCount = ModData.FavoriteMod.Count;
            for (int i = fModCount, c = 0; i > 0; i--,c++)
            {
                var favoriteModComponent = new ClickableTextureComponent("",
                    new Rectangle(
                        xPositionOnScreen - 48,
                        yPositionOnScreen + DistanceFromMenuBottomBeforeNewPage + TabHeight * 2 + 16 + FavoriteTabSize * c,
                        64, FavoriteTabSize), "",
                    ModData.FavoriteMod[i-1].Manifest.Name, ModData.Tabs,
                    ModData.FavoriteTabSource[i-1], 2f)
                {
                    myID = RegionFavoriteOptionsMod + c,
                    upNeighborID = RegionFavoriteOptionsMod + c-1,
                    downNeighborID = RegionFavoriteOptionsMod + c+1,
                    rightNeighborID = 0
                };
                _favoriteSideTabs.Add(favoriteModComponent);
                _favoritePagesCollections.Add(new FavoriteOptionsModPage(xPositionOnScreen, yPositionOnScreen, width, height, i-1));
            }

            _sideTabs[SmapiOptionsTab].myID = _favoriteSideTabs.Count;
            _sideTabs[SmapiOptionsTab].upNeighborID = _favoriteSideTabs.Last().myID;
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
            if (_currentTab < _sideTabs.Count)
                _pagesCollections[_currentTab].receiveScrollWheelAction(direction);
            else 
                _favoritePagesCollections[_currentTab - _sideTabs.Count].receiveScrollWheelAction(direction);
        }

        
        public override void snapToDefaultClickableComponent()
        {
            base.snapToDefaultClickableComponent();
            currentlySnappedComponent = getComponentWithID(0);
            snapCursorToCurrentSnappedComponent();
            if (_currentTab < _sideTabs.Count)
                _pagesCollections[_currentTab].snapToDefaultClickableComponent();                
            else _favoritePagesCollections[_currentTab - _sideTabs.Count].snapToDefaultClickableComponent();
        }

        public override void leftClickHeld(int x, int y)
        {
            base.leftClickHeld(x, y);
            if (_currentTab <_sideTabs.Count)
                _pagesCollections[_currentTab].leftClickHeld(x, y);
            else 
                _favoritePagesCollections[_currentTab - _sideTabs.Count].leftClickHeld(x, y);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.receiveLeftClick(x, y, playSound);
            if (x > xPositionOnScreen+borderWidth)
            {
                if (_currentTab < _sideTabs.Count)
                    _pagesCollections[_currentTab].receiveLeftClick(x, y, playSound);
                else 
                    _favoritePagesCollections[_currentTab - _sideTabs.Count].receiveLeftClick(x, y, playSound);
                return;
            }

            var tabsCount = _sideTabs.Count + _favoriteSideTabs.Count;
            for (var index = 0; index < tabsCount; index++)
            {
                if (index < _sideTabs.Count)
                {
                    if (!_sideTabs[index].containsPoint(x, y) || _currentTab == index) continue;
                    ResetTab();
                    _currentTab = index;
                    _sideTabs[index].bounds.X += WidthToMoveActiveTab;
                    return;
                }
                if (!_favoriteSideTabs[index-_sideTabs.Count].containsPoint(x, y) || _currentTab == index) continue;
                ResetTab();
                _currentTab = index;
                _favoriteSideTabs[index - _sideTabs.Count].bounds.X += WidthToMoveActiveTab;
                return;
            }

            void ResetTab()
            {
                if (_currentTab < _sideTabs.Count)
                    _sideTabs[_currentTab].bounds.X -= WidthToMoveActiveTab;
                else
                    _favoriteSideTabs[_currentTab - _sideTabs.Count].bounds.X -= WidthToMoveActiveTab;
                   
                    
                Game1.playSound("smallSelect");
            }
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            if (GameMenu.forcePreventClose)
                return;
            if (x < xPositionOnScreen-borderWidth) return;
            if (_currentTab < _sideTabs.Count)
                _pagesCollections[_currentTab].receiveRightClick(x, y, playSound);
            else 
                _favoritePagesCollections[_currentTab - _sideTabs.Count].receiveRightClick(x, y, playSound);
        }

        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction(x, y);
            _hoverText = "";
            _value = -1;
            if (x > xPositionOnScreen+borderWidth)
            {
                if (_currentTab < _sideTabs.Count)
                    _pagesCollections[_currentTab].performHoverAction(x, y);
                else 
                    _favoritePagesCollections[_currentTab - _sideTabs.Count].performHoverAction(x, y);
                return;
            }
            
            using (var enumerator = _sideTabs.Where(sideTab => sideTab.containsPoint(x, y)).GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    _hoverText = enumerator.Current?.hoverText;
                    return;
                }
            }

            using (var enumerator = _favoriteSideTabs.Where(sideTab => sideTab.containsPoint(x, y)).GetEnumerator())
            {
                if (!enumerator.MoveNext()) return;
                _hoverText = enumerator.Current?.hoverText;
            }
        }

        public override void releaseLeftClick(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.releaseLeftClick(x, y);
            if (_currentTab < _sideTabs.Count)
                _pagesCollections[_currentTab].releaseLeftClick(x, y);
            else 
                _favoritePagesCollections[_currentTab - _sideTabs.Count].releaseLeftClick(x, y);
        }

        public override void snapCursorToCurrentSnappedComponent()
        {
            if (_currentTab < _sideTabs.Count)
                _pagesCollections[_currentTab].snapCursorToCurrentSnappedComponent();
            else 
                _favoritePagesCollections[_currentTab - _sideTabs.Count].snapCursorToCurrentSnappedComponent();
        }

        public override void receiveKeyPress(Keys key)
        {
            if (_currentTab < _sideTabs.Count)
                _pagesCollections[_currentTab].receiveKeyPress(key);
            else 
                _favoritePagesCollections[_currentTab - _sideTabs.Count].receiveKeyPress(key);
            base.receiveKeyPress(key);
        }

        public override void draw(SpriteBatch b)
        {
            upperRightCloseButton.draw(b);
            foreach (var sideTab in _sideTabs)
                sideTab.draw(b);
            foreach (var sideTab in _favoriteSideTabs)
                sideTab.draw(b);
            b.End();
            b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp,
                null, null, null, new Matrix?());
            if (_currentTab < _sideTabs.Count)
                _pagesCollections[_currentTab].draw(b);
            else 
                _favoritePagesCollections[_currentTab - _sideTabs.Count].draw(b);
            b.End();
            b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null,
                null, null, new Matrix?());
            if (_hoverText.Equals(""))
                return;
            drawHoverText(b, _hoverText, Game1.smallFont, 0, 0, _value);
        }
    }
}