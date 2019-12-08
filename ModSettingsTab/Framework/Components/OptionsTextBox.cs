using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewValley;

namespace ModSettingsTab.Framework.Components
{
    public class OptionsTextBox : OptionsElement
    {
        private static readonly Texture2D TextBoxTexture = Game1.content.Load<Texture2D>("LooseSprites\\textBox");
        private readonly TextBox _textBox;

        public OptionsTextBox(
            string name,
            string label,
            StaticConfig config,
            Rectangle slotSize,
            bool floatOnly = false,
            bool numbersOnly = false,
            bool numAsString = false)
            : base(name, label, config, 32, slotSize.Height / 2 - 4, slotSize.Width / 2, 48)
        {
            // text field value
            var str = config[name].ToString(Formatting.Indented);
            // if the string is short
            if (str.Length < 8) Bounds.Width /= 2;

            _textBox = new TextBox(name, config, TextBoxTexture, Game1.smallFont, Game1.textColor)
            {
                TitleText = name,
                Width = Bounds.Width,
                Height = Bounds.Height,
                numbersOnly = numbersOnly,
                NumAsString = numAsString,
                FloatOnly = floatOnly,
                Text = str
            };
        }

        public override void ReceiveLeftClick(int x, int y)
        {
            _textBox.Update();
        }

        public override void Draw(SpriteBatch b, int slotX, int slotY)
        {
            Utility.drawTextWithShadow(b, Label, Game1.dialogueFont,
                new Vector2(slotX + Bounds.X + _textBox.Width + 8, slotY + Bounds.Y + 8),
                GreyedOut ? Game1.textColor * 0.33f : Game1.textColor, 1f, 0.1f);
            _textBox.X = slotX + Bounds.X;
            _textBox.Y = slotY + Bounds.Y;
            _textBox.Draw(b);
        }
    }
}