using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeveMazeGeneratorMonoGame
{
    class CubeModelInvertedForSkybox
    {
        public Game1 game;
        public int[] indices = new int[36];
        public VertexPositionTexture[] vertices = new VertexPositionTexture[24];

        public int width;
        public int height;
        public int depth;
        public CubeModelInvertedForSkybox(Game1 game, int width, int height, int depth, TexturePosInfo texturePosInfo)
        {
            this.game = game;
            this.width = width;
            this.height = height;
            this.depth = depth;
            GoGenerateVertices(texturePosInfo);
        }

        public void GoGenerateVertices(TexturePosInfo texturePosInfo)
        {
            //Front
            vertices[0] = new VertexPositionTexture(new Vector3(0, height, depth), texturePosInfo.front.First());
            vertices[1] = new VertexPositionTexture(new Vector3(width, height, depth), texturePosInfo.front.Second());
            vertices[2] = new VertexPositionTexture(new Vector3(0, 0, depth), texturePosInfo.front.Third());
            vertices[3] = new VertexPositionTexture(new Vector3(width, 0, depth), texturePosInfo.front.Fourth());

            //Right
            vertices[4] = new VertexPositionTexture(new Vector3(width, height, depth), texturePosInfo.right.First());
            vertices[5] = new VertexPositionTexture(new Vector3(width, height, 0), texturePosInfo.right.Second());
            vertices[6] = new VertexPositionTexture(new Vector3(width, 0, depth), texturePosInfo.right.Third());
            vertices[7] = new VertexPositionTexture(new Vector3(width, 0, 0), texturePosInfo.right.Fourth());

            //Rear
            vertices[8] = new VertexPositionTexture(new Vector3(width, height, 0), texturePosInfo.rear.First());
            vertices[9] = new VertexPositionTexture(new Vector3(0, height, 0), texturePosInfo.rear.Second());
            vertices[10] = new VertexPositionTexture(new Vector3(width, 0, 0), texturePosInfo.rear.Third());
            vertices[11] = new VertexPositionTexture(new Vector3(0, 0, 0), texturePosInfo.rear.Fourth());

            //Left
            vertices[12] = new VertexPositionTexture(new Vector3(0, height, 0), texturePosInfo.left.First());
            vertices[13] = new VertexPositionTexture(new Vector3(0, height, depth), texturePosInfo.left.Second());
            vertices[14] = new VertexPositionTexture(new Vector3(0, 0, 0), texturePosInfo.left.Third());
            vertices[15] = new VertexPositionTexture(new Vector3(0, 0, depth), texturePosInfo.left.Fourth());

            //Top
            vertices[16] = new VertexPositionTexture(new Vector3(0, height, 0), texturePosInfo.top.First());
            vertices[17] = new VertexPositionTexture(new Vector3(width, height, 0), texturePosInfo.top.Second());
            vertices[18] = new VertexPositionTexture(new Vector3(0, height, depth), texturePosInfo.top.Third());
            vertices[19] = new VertexPositionTexture(new Vector3(width, height, depth), texturePosInfo.top.Fourth());

            //Bottom
            vertices[20] = new VertexPositionTexture(new Vector3(0, 0, depth), texturePosInfo.bottom.First());
            vertices[21] = new VertexPositionTexture(new Vector3(width, 0, depth), texturePosInfo.bottom.Second());
            vertices[22] = new VertexPositionTexture(new Vector3(0, 0, 0), texturePosInfo.bottom.Third());
            vertices[23] = new VertexPositionTexture(new Vector3(width, 0, 0), texturePosInfo.bottom.Fourth());


            int cur = 0;
            for (int i = 0; i < 24; i += 4)
            {
                indices[cur + 0] = 2 + i;
                indices[cur + 1] = 1 + i;
                indices[cur + 2] = 0 + i;
                indices[cur + 3] = 2 + i;
                indices[cur + 4] = 3 + i;
                indices[cur + 5] = 1 + i;
                cur += 6;
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(Matrix parentMatrix, BasicEffect effect)
        {
            effect.World = parentMatrix;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 24, indices, 0, 12);
            }
        }
    }
}
