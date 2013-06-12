using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGeneratorMonoGame
{
    class CubeModel
    {
        public Game1 game;
        public int[] indices = new int[36];
        public VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[24];

        public int width;
        public int height;
        public int depth;
        public CubeModel(Game1 game, int width, int height, int depth, TexturePosInfo texturePosInfo)
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
            vertices[0] = new VertexPositionNormalTexture(new Vector3(0, height, depth), new Vector3(0, 0, 1), texturePosInfo.front.First());
            vertices[1] = new VertexPositionNormalTexture(new Vector3(width, height, depth), new Vector3(0, 0, 1), texturePosInfo.front.Second());
            vertices[2] = new VertexPositionNormalTexture(new Vector3(0, 0, depth), new Vector3(0, 0, 1), texturePosInfo.front.Third());
            vertices[3] = new VertexPositionNormalTexture(new Vector3(width, 0, depth), new Vector3(0, 0, 1), texturePosInfo.front.Fourth());

            //Right
            vertices[4] = new VertexPositionNormalTexture(new Vector3(width, height, depth), new Vector3(1, 0, 0), texturePosInfo.right.First());
            vertices[5] = new VertexPositionNormalTexture(new Vector3(width, height, 0), new Vector3(1, 0, 0), texturePosInfo.right.Second());
            vertices[6] = new VertexPositionNormalTexture(new Vector3(width, 0, depth), new Vector3(1, 0, 0), texturePosInfo.right.Third());
            vertices[7] = new VertexPositionNormalTexture(new Vector3(width, 0, 0), new Vector3(1, 0, 0), texturePosInfo.right.Fourth());

            //Rear
            vertices[8] = new VertexPositionNormalTexture(new Vector3(width, height, 0), new Vector3(0, 0, -1), texturePosInfo.rear.First());
            vertices[9] = new VertexPositionNormalTexture(new Vector3(0, height, 0), new Vector3(0, 0, -1), texturePosInfo.rear.Second());
            vertices[10] = new VertexPositionNormalTexture(new Vector3(width, 0, 0), new Vector3(0, 0, -1), texturePosInfo.rear.Third());
            vertices[11] = new VertexPositionNormalTexture(new Vector3(0, 0, 0), new Vector3(0, 0, -1), texturePosInfo.rear.Fourth());

            //Left
            vertices[12] = new VertexPositionNormalTexture(new Vector3(0, height, 0), new Vector3(-1, 0, 0), texturePosInfo.left.First());
            vertices[13] = new VertexPositionNormalTexture(new Vector3(0, height, depth), new Vector3(-1, 0, 0), texturePosInfo.left.Second());
            vertices[14] = new VertexPositionNormalTexture(new Vector3(0, 0, 0), new Vector3(-1, 0, 0), texturePosInfo.left.Third());
            vertices[15] = new VertexPositionNormalTexture(new Vector3(0, 0, depth), new Vector3(-1, 0, 0), texturePosInfo.left.Fourth());

            //Top
            vertices[16] = new VertexPositionNormalTexture(new Vector3(0, height, 0), new Vector3(0, 1, 0), texturePosInfo.top.First());
            vertices[17] = new VertexPositionNormalTexture(new Vector3(width, height, 0), new Vector3(0, 1, 0), texturePosInfo.top.Second());
            vertices[18] = new VertexPositionNormalTexture(new Vector3(0, height, depth), new Vector3(0, 1, 0), texturePosInfo.top.Third());
            vertices[19] = new VertexPositionNormalTexture(new Vector3(width, height, depth), new Vector3(0, 1, 0), texturePosInfo.top.Fourth());

            //Bottom
            vertices[20] = new VertexPositionNormalTexture(new Vector3(0, 0, depth), new Vector3(0, -1, 0), texturePosInfo.bottom.First());
            vertices[21] = new VertexPositionNormalTexture(new Vector3(width, 0, depth), new Vector3(0, -1, 0), texturePosInfo.bottom.Second());
            vertices[22] = new VertexPositionNormalTexture(new Vector3(0, 0, 0), new Vector3(0, -1, 0), texturePosInfo.bottom.Third());
            vertices[23] = new VertexPositionNormalTexture(new Vector3(width, 0, 0), new Vector3(0, -1, 0), texturePosInfo.bottom.Fourth());


            int cur = 0;
            for (int i = 0; i < 24; i += 4)
            {
                indices[cur + 0] = 0 + i;
                indices[cur + 1] = 1 + i;
                indices[cur + 2] = 2 + i;
                indices[cur + 3] = 1 + i;
                indices[cur + 4] = 3 + i;
                indices[cur + 5] = 2 + i;
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
                game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, vertices, 0, 24, indices, 0, 12);
            }
        }
    }
}
