using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGeneratorMonoGame
{
    public static class MatrixExtensions
    {
        public static Matrix CreateRotationX(Vector3 point, float radians)
        {
            Matrix m = Matrix.CreateTranslation(-point);
            m *= Matrix.CreateRotationX(radians);
            m *= Matrix.CreateTranslation(point);
            return m;
        }

        public static Matrix CreateRotationY(Vector3 point, float radians)
        {
            Matrix m = Matrix.CreateTranslation(-point);
            m *= Matrix.CreateRotationY(radians);
            m *= Matrix.CreateTranslation(point);
            return m;
        }

        public static Matrix CreateRotationZ(Vector3 point, float radians)
        {
            Matrix m = Matrix.CreateTranslation(-point);
            m *= Matrix.CreateRotationZ(radians);
            m *= Matrix.CreateTranslation(point);
            return m;
        }
    }
}
