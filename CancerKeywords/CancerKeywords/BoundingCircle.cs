using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CancerKeywords
{
    class BoundingCircle
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        public BoundingCircle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Does this circle contain a given point?
        /// </summary>
        /// <param name="point">A vector with the coordiates of the point of interest</param>
        /// <returns>True if the circle does contain the point</returns>
        public bool contains(Vector2 point)
        {
            return (Center - point).Length() < Radius;
        }

        /// <summary>
        /// Does this circle overlap with another circle?
        /// </summary>
        /// <param name="otherCircle">Another circle</param>
        /// <returns>True if the circles overlap at any point</returns>
        public bool intersect(BoundingCircle otherCircle)
        {
            return (Center - otherCircle.Center).Length() < (Radius + otherCircle.Radius);
        }
    }
}
