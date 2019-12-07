using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModSettingsTab.Framework.Interfaces;

namespace ModSettingsTab.Framework.Components
{
    /// <summary>
    /// base options element
    /// </summary>
    public abstract class OptionsElement : IModOption
    {
        /// <summary>
        /// active zone
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        /// element name (JToken path)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// mod UniqueId
        /// </summary>
        public string ModId { get; set; }

        /// <summary>
        /// the setting header is displayed in the list
        /// </summary>
        /// <remarks>
        /// Name by default
        /// </remarks>
        public string Label { get; set; }

        /// <summary>
        /// inactive state
        /// </summary>
        public bool GreyedOut { get; set; }

        protected OptionsElement(
            string name,
            string modId,
            string label,
            int x = 32,
            int y = 16,
            int width = 36,
            int height = 36)
        {
            Bounds = new Rectangle(x, y, width, height);
            Name = name;
            ModId = modId;
            Label = !string.IsNullOrEmpty(label) ? label.Replace(".", " > ") : "";
        }

        public virtual void ReceiveLeftClick(int x, int y)
        {
        }

        public virtual void LeftClickHeld(int x, int y)
        {
        }

        public virtual void LeftClickReleased(int x, int y)
        {
        }

        public virtual void ReceiveKeyPress(Keys key)
        {
        }

        public virtual void Draw(SpriteBatch b, int slotX, int slotY)
        {
        }
    }
}