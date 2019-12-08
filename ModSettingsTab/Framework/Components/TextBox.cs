using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using StardewValley;

namespace ModSettingsTab.Framework.Components
{
    public class TextBox : StardewValley.Menus.TextBox
    {
        public bool FloatOnly;
        public bool NumAsString;
        private readonly string _optionName;
        private readonly StaticConfig _config;

        public TextBox(
            string optionName,
            StaticConfig config,
            Texture2D textBoxTexture,
            SpriteFont font,
            Color textColor)
            : base(textBoxTexture, null, font, textColor)
        {
            _config = config;
            _optionName = optionName;
        }

        public new void Update()
        {
            Selected =
                new Rectangle(X, Y, Width, Height).Contains(new Point(Game1.getMouseX(),
                    Game1.getMouseY()));
        }

        public override void RecieveTextInput(string text)
        {
            if (!Selected || numbersOnly && !int.TryParse(text, out _) ||
                FloatOnly && float.TryParse(Text + text, NumberStyles.Any,
                    CultureInfo.CreateSpecificCulture("en-US"), out _) ||
                -textLimit != -1 && Text.Length >= textLimit)
                return;
            Text += text;
            SaveState();
        }

        private void SaveState()
        {
            Text = Text.Trim();
            if (numbersOnly)
            {
                _config[_optionName] =
                    NumAsString || !int.TryParse(Text, out var result) ? (JToken) Text : (JToken) result;
            }
            else if (FloatOnly)
            {
                _config[_optionName] =
                    NumAsString || !float.TryParse(Text, NumberStyles.Any,
                        CultureInfo.CreateSpecificCulture("en-US"), out var result)
                        ? (JToken) Text
                        : (JToken) result;
            }
            else
                _config[_optionName] = Text;
        }

        public override void RecieveTextInput(char inputChar)
        {
            if (!Selected || FloatOnly && !float.TryParse(Text + inputChar, NumberStyles.Any,
                    CultureInfo.CreateSpecificCulture("en-US"), out _) ||
                numbersOnly && !char.IsDigit(inputChar) ||
                textLimit != -1 && Text.Length >= textLimit)
                return;
            Text += inputChar.ToString();
            SaveState();
        }

        public new bool Selected
        {
            get => base.Selected;
            set
            {
                if (base.Selected == value)
                    return;
                base.Selected = value;
                if (base.Selected)
                    ModData.CurrentTextBox = this;
                else if (ModData.CurrentTextBox == this)
                    ModData.CurrentTextBox = null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, bool drawShadow = true)
        {
            var flag = DateTime.UtcNow.Millisecond % 1000 >= 500;
            var text = Text;
            var color = Game1.textColor;
            if (_textBoxTexture != null)
            {
                spriteBatch.Draw(_textBoxTexture, new Rectangle(X, Y, 16, Height),
                    new Rectangle(0, 0, 16, Height), Color.White * 1f);
                spriteBatch.Draw(_textBoxTexture, new Rectangle(X + 16, Y, Width - 32, Height),
                    new Rectangle(16, 0, 4, Height), Color.White * 1f);
                spriteBatch.Draw(_textBoxTexture, new Rectangle(X + Width - 16, Y, 16, Height),
                    new Rectangle(_textBoxTexture.Bounds.Width - 16, 0, 16, Height),
                    Color.White * 1f);
            }
            else
                Game1.drawDialogueBox(X - 32, Y - 112 + 10, Width + 80, Height, false, true,
                    null, false, false);

            Vector2 vector2;
            for (vector2 = _font.MeasureString(text);
                (double) vector2.X > (double) Width;
                vector2 = _font.MeasureString(text))
                text = text.Substring(1);
            if (flag && Selected)
                Utility.drawTextWithShadow(spriteBatch, "|", _font,
                    new Vector2(X + 16 + (int) vector2.X + 2, Y + 5), color);
            if (drawShadow)
                Utility.drawTextWithShadow(spriteBatch, text, _font,
                    new Vector2(X + 16, Y + 8), color);
            else
                spriteBatch.DrawString(_font, text, new Vector2(X + 16, Y + 12),
                    color, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);
        }
    }
}