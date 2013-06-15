using DeveMazeGenerator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGeneratorMonoGame
{
    class VierkantjeModel
    {


        public VierkantjeModel()
        {



            //GoGenerateVertices(TexturePosInfoGenerator.FullImage);
        }

        public void GoGenerateVertices(float posx, float posy, VertexPositionNormalTexture[] vertices, int[] indices, ref int curVertice, ref int curIndice)
        {
            TexturePosInfo texturePosInfo = TexturePosInfoGenerator.FullImage;

            float height = 0.1f;

            int howmuchvertices = 4;

            float amount = 0.4f;

            //Front
            vertices[curVertice + 0] = new VertexPositionNormalTexture(new Vector3(posx - amount, height, posy - amount), new Vector3(0, 0, 1), texturePosInfo.front.First());
            vertices[curVertice + 1] = new VertexPositionNormalTexture(new Vector3(posx + amount, height, posy - amount), new Vector3(0, 0, 1), texturePosInfo.front.Second());
            vertices[curVertice + 2] = new VertexPositionNormalTexture(new Vector3(posx - amount, height, posy + amount), new Vector3(0, 0, 1), texturePosInfo.front.Third());
            vertices[curVertice + 3] = new VertexPositionNormalTexture(new Vector3(posx + amount, height, posy + amount), new Vector3(0, 0, 1), texturePosInfo.front.Fourth());

            //Rear
            //vertices[curVertice + 4] = new VertexPositionNormalTexture(new Vector3(mazeWall.xstart, height, mazeWall.ystart), new Vector3(0, 0, -1), texturePosInfo.rear.Second());
            //vertices[curVertice + 5] = new VertexPositionNormalTexture(new Vector3(mazeWall.xstart, 0, mazeWall.ystart), new Vector3(0, 0, -1), texturePosInfo.rear.Fourth());
            //vertices[curVertice + 6] = new VertexPositionNormalTexture(new Vector3(mazeWall.xend, height, mazeWall.yend), new Vector3(0, 0, -1), texturePosInfo.rear.First());
            //vertices[curVertice + 7] = new VertexPositionNormalTexture(new Vector3(mazeWall.xend, 0, mazeWall.yend), new Vector3(0, 0, -1), texturePosInfo.rear.Third());



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



            //float hhh = height;
            //float www = mazeWall.yend - mazeWall.ystart + mazeWall.xend - mazeWall.xstart;

            //////This stuff is for repeating the texture
            //for (int i = curVertice; i < curVertice + howmuchvertices; i++)
            //{
            //    var vert = vertices[i];
            //    vert.TextureCoordinate.X *= (www / 1.0f);
            //    vert.TextureCoordinate.Y *= (hhh / 1.0f);
            //    vertices[i] = vert;
            //}

            curVertice += howmuchvertices;


        }
    }
}
