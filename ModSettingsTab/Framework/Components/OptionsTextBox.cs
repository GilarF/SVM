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
            string modId,
            string label,
            StaticConfig config,
            Point slotSize,
            bool floatOnly = false,
            bool numbersOnly = false,
            bool numAsString = false)
            : base(name, modId, label, config, 32, slotSize.Y / 2 - 4, slotSize.X / 2 - 64, 48)
        {
            // text field value
            var str = config[name].ToString();
            // if the string is short
            if (str.Length < 12) Bounds.Width /= 2 ;
            Offset.Y = 8;

            _textBox = new TextBox(name, config, TextBoxTexture, Game1.smallFont, Game1.textColor)
            {
                TitleText = name,
                Width = Bounds.Width,
                Height = Bounds.Height,
                numbersOnly = numbersOnly,
                NumAsString = numAsString,
                FloatOnly = floatOnly,
                Text = !floatOnly ? str : config[name].ToString(Formatting.Indented)
            };
        }

        public override void ReceiveLeftClick(int x, int y)
        {
            _textBox.Update();
        }

        public override void Draw(SpriteBatch b, int slotX, int slotY)
        {
            base.Draw(b, slotX, slotY);
            _textBox.X = slotX + Bounds.X;
            _textBox.Y = slotY + Bounds.Y;
            _textBox.Draw(b);
        }
    }
}