using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeveMazeGeneratorMonoGame
{
    public static class ContentDing
    {
        public static Texture2D skyTexture1;
        public static Texture2D blankTexture;
        public static Texture2D redTexture;
        public static Texture2D semiTransparantTexture;

        public static Texture2D endTexture;
        public static Texture2D startTexture;

        public static Texture2D win98WallTexture;
        public static Texture2D win98FloorTexture;
        public static Texture2D win98RoofTexture;
        public static Texture2D win98LegoTexture;

        //public static SpriteFont spriteFont;

        public static void GoLoadContent(GraphicsDevice graphicsDevice, ContentManager Content)
        {
            skyTexture1 = Content.Load<Texture2D>("sky");

            endTexture = Content.Load<Texture2D>("end");
            startTexture = Content.Load<Texture2D>("start");

            win98FloorTexture = Content.Load<Texture2D>("floor");
            win98LegoTexture = Content.Load<Texture2D>("lego");
            win98RoofTexture = Content.Load<Texture2D>("roof");
            win98WallTexture = Content.Load<Texture2D>("wall");

            blankTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blankTexture.SetData(new[] { Color.White });

            redTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            redTexture.SetData(new[] { Color.Red });

            semiTransparantTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            semiTransparantTexture.SetData(new[] { new Color(0, 0, 0, 128) });

            //spriteFont = Content.Load<SpriteFont>("spriteFont1.xnb");
        }
    }
}
