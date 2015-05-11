using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CancerKeywords
{
    public class TextBubble
    {
        public Texture2D Texture { get; set; }
        public int Size { get; set; }
        public Vector2 Position { get; set; }
        public string Text { get; set; }
        public SpriteFont StdFont { get; set; }
        public Color color { get; set; }
        public int Cases { get; set; }
        public int highestCount { get; set; }

        public TextBubble(Texture2D texture, SpriteFont stdFont) : this(texture, 0, new Vector2(0, 0), "", stdFont, Color.White) { }

        public TextBubble(Texture2D texture, int size, Vector2 position, string text, SpriteFont stdFont, Color color)
        {
            this.Texture = texture;
            this.Size = size;
            this.Position = position;
            this.Text = text;
            this.color = color;
            this.StdFont = stdFont;

            //Defaults
            Cases = 0;
            highestCount = 0;
        }

        /// <summary>
        /// Calculate the center of the bubble
        /// </summary>
        /// <returns>The center of the bubble</returns>
        public Vector2 getCenter()
        {
            return new Vector2(Position.X + (int)Size / 2, Position.Y + (int)Size / 2);
        }

        /// <summary>
        /// Update function to be called from Update in the XNA framework
        /// </summary>
        /// <param name="gameTime">gametime from XNA framework</param>
        public virtual void Update(GameTime gameTime)
        {
           
        }

        /// <summary>
        /// Adds one to the score
        /// </summary>
        public void addCase()
        {
            Cases++;
        }

        /// <summary>
        /// Draw function to be called from Draw in the XNA framework
        /// </summary>
        /// <param name="spriteBatch">spritebatch from XNA framework</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Calculates the position of the text
            Vector2 textCenter = new Vector2(getCenter().X - StdFont.MeasureString(Text).X / 2, getCenter().Y - StdFont.MeasureString(Text).Y / 2);
            string nText = "n = " + Cases;
            Vector2 nTextCenter = new Vector2(getCenter().X - StdFont.MeasureString(nText).X / 2, getCenter().Y + StdFont.MeasureString(Text).Y / 5);

            //Draw the bubble and text
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Size, Size), color);
            spriteBatch.DrawString(StdFont, Text, textCenter, Color.Black);
            spriteBatch.DrawString(StdFont, nText, nTextCenter, Color.Black);
            spriteBatch.End();
        }
    }
}
