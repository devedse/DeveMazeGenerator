using DeveMazeGenerator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeveMazeGeneratorMonoGame
{
    class WallModel
    {
        public Game1 game;
        public int[] indices = new int[12];
        public VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[24];


        private MazeWall mazeWall;

        public Matrix myMatrix;

        public WallModel(Game1 game, MazeWall mazeWall)
        {
            this.game = game;
            this.mazeWall = mazeWall;


            //GoGenerateVertices(TexturePosInfoGenerator.FullImage);
        }

        public void GoGenerateVertices(VertexPositionNormalTexture[] vertices, int[] indices, ref int curVertice, ref int curIndice)
        {
            TexturePosInfo texturePosInfo = TexturePosInfoGenerator.FullImage;

            int height = 1;

            int howmuchvertices = 4;

            //Front
            vertices[curVertice + 0] = new VertexPositionNormalTexture(new Vector3(mazeWall.xstart, height, mazeWall.ystart), new Vector3(0, 0, 1), texturePosInfo.front.First());
            vertices[curVertice + 1] = new VertexPositionNormalTexture(new Vector3(mazeWall.xend, height, mazeWall.yend), new Vector3(0, 0, 1), texturePosInfo.front.Second());
            vertices[curVertice + 2] = new VertexPositionNormalTexture(new Vector3(mazeWall.xstart, 0, mazeWall.ystart), new Vector3(0, 0, 1), texturePosInfo.front.Third());
            vertices[curVertice + 3] = new VertexPositionNormalTexture(new Vector3(mazeWall.xend, 0, mazeWall.yend), new Vector3(0, 0, 1), texturePosInfo.front.Fourth());

            //Rear
            //vertices[4] = new VertexPositionNormalTexture(new Vector3(width, height, 0), new Vector3(0, 0, -1), texturePosInfo.rear.First());
            //vertices[5] = new VertexPositionNormalTexture(new Vector3(0, height, 0), new Vector3(0, 0, -1), texturePosInfo.rear.Second());
            //vertices[6] = new VertexPositionNormalTexture(new Vector3(width, 0, 0), new Vector3(0, 0, -1), texturePosInfo.rear.Third());
            //vertices[7] = new VertexPositionNormalTexture(new Vector3(0, 0, 0), new Vector3(0, 0, -1), texturePosInfo.rear.Fourth());



            for (int i = 0; i < howmuchvertices; i += 4)
            {
                indices[curIndice + 0] = curVertice + 0 + i;
                indices[curIndice + 1] = curVertice + 1 + i;
                indices[curIndice + 2] = curVertice + 2 + i;
                indices[curIndice + 3] = curVertice + 1 + i;
                indices[curIndice + 4] = curVertice + 3 + i;
                indices[curIndice + 5] = curVertice + 2 + i;
                curIndice += 6;
            }

            curVertice += howmuchvertices;

            ////This stuff is for repeating the texture
            //for (int i = 0; i < vertices.Length; i++)
            //{
            //    var vert = vertices[i];
            //    vert.TextureCoordinate.X *= (width / 10.0f);
            //    vert.TextureCoordinate.Y *= (height / 10.0f);
            //    vertices[i] = vert;
            //}
        }

        //public void Update(GameTime gameTime)
        //{

        //}

        //public void Draw(Matrix parentMatrix, BasicEffect effect)
        //{
        //    effect.World = parentMatrix * myMatrix;

        //    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
        //    {
        //        pass.Apply();
        //        game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, vertices, 0, 8, indices, 0, 4);
        //    }
        //}
    }
}
