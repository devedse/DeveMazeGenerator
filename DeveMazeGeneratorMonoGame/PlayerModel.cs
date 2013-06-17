using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGeneratorMonoGame
{
    public class PlayerModel
    {
        CubeModelForPlayer headModel;
        CubeModelForPlayer bodyModel;

        CubeModelForPlayer armModelLeft;
        CubeModelForPlayer armModelRight;

        CubeModelForPlayer legModelLeft;
        CubeModelForPlayer legModelRight;



        public PlayerModel(Game1 game)
        {
            headModel = new CubeModelForPlayer(game, 8, 8, 8, TexturePosInfoGenerator.Head);
            bodyModel = new CubeModelForPlayer(game, 8, 12, 4, TexturePosInfoGenerator.Body);

            armModelLeft = new CubeModelForPlayer(game, 4, 12, 4, TexturePosInfoGenerator.ArmLeft);
            armModelRight = new CubeModelForPlayer(game, 4, 12, 4, TexturePosInfoGenerator.ArmRight);

            legModelLeft = new CubeModelForPlayer(game, 4, 12, 4, TexturePosInfoGenerator.LegLeft);
            legModelRight = new CubeModelForPlayer(game, 4, 12, 4, TexturePosInfoGenerator.LegRight);
        }

        public void Update(GameTime gameTime)
        {
            //value += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //headModel.Update(gameTime);
            //bodyModel.Update(gameTime);
        }

        public void Draw(Matrix parentMatrix, BasicEffect effect, float value, float headTurn)
        {
            Matrix headTranslation = Matrix.CreateTranslation(new Vector3(-4, 0, -4));
            headTranslation *= Matrix.CreateFromYawPitchRoll(headTurn, (float)Math.Sin(value * 8) / 10, 0);
            //headTranslation *= Matrix.CreateRotationX((float)Math.Sin(value * 8) / 10);
            //headTranslation *= Matrix.CreateRotationY(headTurn);
            headTranslation *= Matrix.CreateTranslation(new Vector3(4, 0, 4));

            //Matrix headTranslation = MatrixExtensions.CreateRotationX(new Vector3(4, 0, 4), (float)Math.Sin(value * 8) / 10);
            //headTranslation *= Matrix.;
            headModel.Draw(headTranslation * Matrix.CreateTranslation(0, 12, -2) * parentMatrix, effect);

            bodyModel.Draw(parentMatrix, effect);

            Matrix armLeftTranslation = MatrixExtensions.CreateRotationX(new Vector3(2, 10, 2), (float)Math.Sin(value * 5) / 2);
            armLeftTranslation *= MatrixExtensions.CreateRotationZ(new Vector3(2, 10, 2), (float)Math.Sin(value * 9) / 8 - 1.0f / 8.0f);
            armModelLeft.Draw(armLeftTranslation * Matrix.CreateTranslation(-4, 0, 0) * parentMatrix, effect);

            Matrix armRightTranslation = MatrixExtensions.CreateRotationX(new Vector3(2, 10, 2), (float)Math.Sin(value * 5 - Math.PI) / 2);
            armRightTranslation *= MatrixExtensions.CreateRotationZ(new Vector3(2, 10, 2), (float)Math.Sin(value * 9 - Math.PI) / 8 + 1.0f / 8.0f);
            armModelRight.Draw(armRightTranslation * Matrix.CreateTranslation(8, 0, 0) * parentMatrix, effect);

            Matrix legLeftTranslation = MatrixExtensions.CreateRotationX(new Vector3(2, 12, 2), (float)Math.Sin(value * 7) / 1);
            legModelLeft.Draw(legLeftTranslation * Matrix.CreateTranslation(0, -12, 0) * parentMatrix, effect);

            Matrix legRightTranslation = MatrixExtensions.CreateRotationX(new Vector3(2, 12, 2), (float)Math.Sin(value * 7 - Math.PI) / 1);
            legModelRight.Draw(legRightTranslation * Matrix.CreateTranslation(4, -12, 0) * parentMatrix, effect);
        }
    }
}
