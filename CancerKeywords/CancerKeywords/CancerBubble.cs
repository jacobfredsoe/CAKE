using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CancerKeywords
{
    class CancerBubble : TextBubble, IComparable
    {
        public TextBubble Anchor { get; set; }
        public int Distance { get; set; }
        public float Angle { get; set; }
        private const int MIN_SIZE = 20;
        public List<int> abstractIDs { get; set; }
        public bool Selected { get; set; }

        public CancerBubble(Texture2D texture, SpriteFont stdFont, string name, int distance, TextBubble anchor) : base(texture, stdFont)
        {
            Anchor = anchor;
            Distance = distance;
            Text = name;
            abstractIDs = new List<int>();

            //Defaults
            Angle = 0.0f;
            Cases = 0;
            color = Color.White;
            Selected = false;
        }

        /// <summary>
        /// Adds a case to the bubble
        /// </summary>
        /// <param name="abstractID">pubmed ID of the abstract</param>
        public void addCase(int abstractID)
        {
            abstractIDs.Add(abstractID);
            base.Cases = abstractIDs.Count;
        }

        /// <summary>
        /// Creates a bounding circle that is slightly larger than the bubble (radius + 2% of distance)
        /// </summary>
        /// <returns>A bounding circle object</returns>
        public BoundingCircle getBoundingCircle()
        {
            return new BoundingCircle(this.getCenter(), (float)Size/2 + Distance/50);
        }

        /// <summary>
        /// Overide of the Update function of parent. Updates the size based on the total cases
        /// and updates the position based on the angle and distance.
        /// </summary>
        /// <param name="gameTime">gametime object from XNA framework</param>
        public override void Update(GameTime gameTime)
        {
            //Set the size, with a minimum size
            if(Cases != 0 || Anchor.highestCount != 0)
            {
                Size = (int)(((float)Cases / (float)Anchor.highestCount) * Distance/2);
                if (Size < MIN_SIZE)
                {
                    Size = MIN_SIZE;
                }
            }
            else
            {
                Size = MIN_SIZE;
            }

            color = Misc.colorFromNumber((int)Math.Round((Size-9.1667)/10.833, 0));

            //Update the position
            Position = new Vector2((float)(Anchor.getCenter().X + (Distance * Math.Cos(MathHelper.ToRadians(Angle)))) - Size / 2, (float)(Anchor.getCenter().Y + (Distance * Math.Sin(MathHelper.ToRadians(Angle)))) - Size / 2);
        }

        /// <summary>
        /// If two bounding circles are found to collide, nudge both a bit away from eachother
        /// </summary>
        /// <param name="otherBubble">The colliding bubble</param>
        public void collide(CancerBubble otherBubble)
        {
            float angularDistance = this.Angle - otherBubble.Angle;

            //Is the other bubble to the right or left of this one?
            if(Math.Abs(angularDistance) > 180)
            {
                if(angularDistance > 0)
                {
                    angularDistance = angularDistance - 360;
                }
                else
                {
                    angularDistance = angularDistance + 360;
                }
            }

            //If the other bubble is to the left, nudge this bubble clockwise and the other count clockwise
            if(angularDistance > 0)
            {
                nudgeClockwise(0.1f);
                otherBubble.nudgeCounterClockwise(0.1f);
            }
            else //or if the other bubble is to the right, do it opposite
            {
                nudgeCounterClockwise(0.1f);
                otherBubble.nudgeClockwise(0.1f);
            }
        }

        /// <summary>
        /// Move this bubble a number of degrees in a clockwise direction
        /// </summary>
        /// <param name="amount">Number of degrees this bubble is moved</param>
        public void nudgeClockwise(float amount)
        {
            if(Angle + amount > 360)
            {
                Angle = Angle + amount - 360;
            }
            else
            {
                Angle = Angle + amount;
            }
        }

        /// <summary>
        /// Move this bubble a number of degrees in a counter clockwise direction
        /// </summary>
        /// <param name="amount">Number of degrees this bubble is moved</param>
        public void nudgeCounterClockwise(float amount)
        {
            if (Angle - amount < 0)
            {
                Angle = Angle - amount + 360;
            }
            else
            {
                Angle = Angle - amount;
            }
        }

        /// <summary>
        /// Implementation of IComparable, sorts by number of cases
        /// </summary>
        /// <param name="otherBubble">another textbubble</param>
        /// <returns></returns>
        int IComparable.CompareTo(object otherBubble)
        {
            TextBubble otherCancerBubble = (TextBubble)otherBubble;

            return otherCancerBubble.Cases - this.Cases;
        }

        /// <summary>
        /// Draw function to be called from Draw in the XNA framework
        /// </summary>
        /// <param name="spriteBatch">spritebatch from XNA framework</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if(Selected)
            {
                spriteBatch.Begin();
                Primitives2D.DrawCircle(spriteBatch, getCenter(), Size / 2, 48, Color.Black, 1.5f);
                spriteBatch.End();
            }
            
        }
    }
}
