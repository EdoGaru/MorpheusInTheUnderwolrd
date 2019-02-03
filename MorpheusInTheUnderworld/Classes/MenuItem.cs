using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace MorpheusInTheUnderworld.Classes 
{
    public class MenuItem
    {
        public MenuItem(BitmapFont font, string text)
        {
            Text = text;
            Font = font;
            Color = Color.White;
        }

        public BitmapFont Font { get; set; }
        public string Text { get; set; }
        /// <summary>
        /// This Property set the current MenuItem Position
        /// NOTE: we have modified the set method so each time this property 
        ///       gets modified it will update bounding rectangle position aswell!
        /// </summary>
        private Vector2 position;
        public Vector2 Position { get { return position; } set { position = value; BoundingRectangle = new RectangleF(value, Font.MeasureString(Text)); } }
        public Color Color { get; set; }
        public RectangleF BoundingRectangle { get; set; }
        public Action Action { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Position, Color);
        }

    }
}