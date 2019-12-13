using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModSettingsTab.Menu;
using StardewValley;

namespace ModSettingsTab.Framework.Components
{
    public class FilterTextBox : StardewValley.Menus.TextBox
  {
    private static readonly Texture2D SearchBox = ModEntry.Helper.Content.Load<Texture2D>("assets/FilterTextBox.png");
    private string _text = "";
    private readonly BaseOptionsModPage _optionsModPage;
    private readonly List<OptionsElement> _options;

    public FilterTextBox(int x, int y, BaseOptionsModPage optionsModPage)
      : base(SearchBox, null, Game1.smallFont, Game1.textColor)
    {
      _optionsModPage = optionsModPage;
      _options = optionsModPage.Options;
      Width = 268;
      X = x;
      Y = y;
    }

    private void BackspacePressed(TextBox _)
    {
      if (Text.Length <= 0)
        return;
      Text = Text.Substring(0, Text.Length - 1);
      SetFilter();
      if (Game1.gameMode == 3)
        return;
      Game1.playSound("tinyWhip");
    }

    public new void Update()
    {
      Selected = new Rectangle(X, Y, Width, Height).Contains(new Point(Game1.getMouseX(), Game1.getMouseY()));
    }

    public override void RecieveTextInput(string text)
    {
      if (!Selected || -textLimit != -1 && Text.Length >= textLimit)
        return;
      Text += text;
      SetFilter();
    }

    public override void RecieveCommandInput(char command)
    {
      if (!Selected || command != '\b' || Text.Length <= 0)
        return;
      Text = Text.Substring(0, Text.Length - 1);
      SetFilter();
      if (Game1.gameMode == 3)
        return;
      Game1.playSound("tinyWhip");
    }

    private new string Text
    {
      get => _text;
      set
      {
        _text = value ?? "";
        if (_text == "")
          return;
        _text = Program.sdk.FilterDirtyWords(value?.Where(ch => _font.Characters.Contains(ch)).Aggregate("", (current, ch) => current + ch));
        if (!limitWidth || _font.MeasureString(_text).X <= (double) (Width - 112))
          return;
        Text = _text.Substring(0, _text.Length - 1);
      }
    }

    private void SetFilter()
    {
      if (Text.Length < 1 || Text.Trim() == string.Empty)
      {
        _optionsModPage.Options = _options;
        _optionsModPage.SetScrollBarToCurrentIndex();
      }
      else
      {
        if (_optionsModPage.Options.Count > 0)
        {
          _optionsModPage.snapToDefaultClickableComponent();
          _optionsModPage.SetScrollBarToCurrentIndex();
        }
        //_optionsModPage.Options = _options.Where(o => OptionsPage.ModList[o.Mod].Manifest.Name.Trim().ToLower().Contains(Text.Trim().ToLower())).ToList();
      }
    }

    public override void RecieveTextInput(char inputChar)
    {
      if (!Selected || textLimit != -1 && Text.Length >= textLimit)
        return;
      Text += inputChar.ToString();
      SetFilter();
    }

    private new bool Selected
    {
      get => base.Selected;
      set
      {
        if (base.Selected == value)
          return;
        base.Selected = value;
        if (base.Selected)
          ModData.CurrentTextBox =  this;
        else if (ModData.CurrentTextBox == this)
          ModData.CurrentTextBox =  null;
      }
    }

    public override void Draw(SpriteBatch spriteBatch, bool drawShadow = true)
    {
      bool flag = DateTime.UtcNow.Millisecond % 1000 >= 500;
      string text = Text;
      if (_textBoxTexture != null)
      {
        spriteBatch.Draw(_textBoxTexture, new Rectangle(X, Y, 28, Height), new Rectangle(0, 0, 28, Height), Color.White);
        spriteBatch.Draw(_textBoxTexture, new Rectangle(X + 28, Y, Width - 112, Height), new Rectangle(28, 0, 8, Height), Color.White);
        spriteBatch.Draw(_textBoxTexture, new Rectangle(X + Width - 84, Y, 84, Height), new Rectangle(_textBoxTexture.Bounds.Width - 84, 0, 84, Height), Color.White);
      }
      else
        Game1.drawDialogueBox(X - 32, Y - 112 + 10, Width + 80, Height, false, true, null, false, false);
      Vector2 vector2;
      for (vector2 = _font.MeasureString(text); (double) vector2.X > (double) Width - 112.0; vector2 = _font.MeasureString(text))
        text = text.Substring(1);
      if (flag && Selected)
        Utility.drawTextWithShadow(spriteBatch, "|", _font, new Vector2(X + 30 + (int) vector2.X + 2, Y + 18), Game1.textColor);
      if (drawShadow)
        Utility.drawTextWithShadow(spriteBatch, text, _font, new Vector2(X + 30, Y + 21), Game1.textColor);
      else
        spriteBatch.DrawString(_font, text, new Vector2(X + 30, Y + 21), Game1.textColor, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);
    }
  }
}