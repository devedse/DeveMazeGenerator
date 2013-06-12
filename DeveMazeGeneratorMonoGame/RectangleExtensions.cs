using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeveMazeGeneratorMonoGame
{
    public static class RectangleExtensions
    {
        public static float width = 64f;
        public static float height = 32f;

        public static Vector2 First(this Rectangle rect)
        {
            Vector2 vect = new Vector2(rect.X / width, rect.Y / height);
            return vect;
        }

        public static Vector2 Second(this Rectangle rect)
        {
            Vector2 vect = new Vector2((rect.X + rect.Width) / width, rect.Y / height);
            return vect;
        }

        public static Vector2 Third(this Rectangle rect)
        {
            Vector2 vect = new Vector2(rect.X / width, (rect.Y + rect.Height) / height);
            return vect;
        }

        public static Vector2 Fourth(this Rectangle rect)
        {
            Vector2 vect = new Vector2((rect.X + rect.Width) / width, (rect.Y + rect.Height) / height);
            return vect;
        }
    }
}
