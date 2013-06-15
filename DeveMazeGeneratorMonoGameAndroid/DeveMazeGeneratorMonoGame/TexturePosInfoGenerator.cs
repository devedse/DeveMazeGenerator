using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeveMazeGeneratorMonoGame
{
    public static class TexturePosInfoGenerator
    {
        //private static TexturePosInfo head = null;
        //public static TexturePosInfo Head
        //{
        //    get
        //    {
        //        if (head == null)
        //        {
        //            head = new TexturePosInfo();
        //            head.front = new Rectangle(8, 8, 8, 8);
        //            head.right = new Rectangle(16, 8, 8, 8);
        //            head.rear = new Rectangle(24, 8, 8, 8);
        //            head.left = new Rectangle(0, 8, 8, 8);
        //            head.top = new Rectangle(8, 0, 8, 8);
        //            head.bottom = new Rectangle(16, 0, 8, 8);
        //        }
        //        return head;
        //    }
        //}

        //private static TexturePosInfo body = null;
        //public static TexturePosInfo Body
        //{
        //    get
        //    {
        //        if (body == null)
        //        {
        //            body = new TexturePosInfo();
        //            body.front = new Rectangle(20, 20, 8, 12);
        //            body.right = new Rectangle(28, 20, 4, 12);
        //            body.rear = new Rectangle(32, 20, 8, 12);
        //            body.left = new Rectangle(16, 20, 4, 12);
        //            body.top = new Rectangle(20, 16, 8, 4);
        //            body.bottom = new Rectangle(28, 16, 8, 4);
        //        }
        //        return body;
        //    }
        //}

        //private static TexturePosInfo armLeft = null;
        //public static TexturePosInfo ArmLeft
        //{
        //    get
        //    {
        //        if (armLeft == null)
        //        {
        //            armLeft = new TexturePosInfo();
        //            armLeft.front = new Rectangle(44, 20, 4, 12);
        //            armLeft.right = new Rectangle(48, 20, 4, 12);
        //            armLeft.rear = new Rectangle(52, 20, 4, 12);
        //            armLeft.left = new Rectangle(40, 20, 4, 12);
        //            armLeft.top = new Rectangle(44, 16, 4, 4);
        //            armLeft.bottom = new Rectangle(48, 16, 4, 4);
        //        }
        //        return armLeft;
        //    }
        //}

        //private static TexturePosInfo armRight = null;
        //public static TexturePosInfo ArmRight
        //{
        //    get
        //    {
        //        if (armRight == null)
        //        {
        //            armRight = new TexturePosInfo();
        //            armRight.front = new Rectangle(48, 20, -4, 12);
        //            armRight.left = new Rectangle(52, 20, -4, 12);
        //            armRight.rear = new Rectangle(56, 20, -4, 12);
        //            armRight.right = new Rectangle(44, 20, -4, 12);
        //            armRight.top = new Rectangle(48, 16, -4, 4);
        //            armRight.bottom = new Rectangle(52, 16, -4, 4);
        //        }
        //        return armRight;
        //    }
        //}

        //private static TexturePosInfo legLeft = null;
        //public static TexturePosInfo LegLeft
        //{
        //    get
        //    {
        //        if (legLeft == null)
        //        {
        //            legLeft = new TexturePosInfo();
        //            legLeft.front = new Rectangle(4, 20, 4, 12);
        //            legLeft.right = new Rectangle(8, 20, 4, 12);
        //            legLeft.rear = new Rectangle(12, 20, 4, 12);
        //            legLeft.left = new Rectangle(0, 20, 4, 12);
        //            legLeft.top = new Rectangle(4, 16, 4, 4);
        //            legLeft.bottom = new Rectangle(8, 16, 4, 4);
        //        }
        //        return legLeft;
        //    }
        //}

        //private static TexturePosInfo legRight = null;
        //public static TexturePosInfo LegRight
        //{
        //    get
        //    {
        //        if (legRight == null)
        //        {
        //            legRight = new TexturePosInfo();
        //            legRight.front = new Rectangle(8, 20, -4, 12);
        //            legRight.left = new Rectangle(12, 20, -4, 12);
        //            legRight.rear = new Rectangle(16, 20, -4, 12);
        //            legRight.right = new Rectangle(4, 20, -4, 12);
        //            legRight.top = new Rectangle(8, 16, -4, 4);
        //            legRight.bottom = new Rectangle(12, 16, -4, 4);
        //        }
        //        return legRight;
        //    }
        //}

        private static TexturePosInfo normalThing = null;
        public static TexturePosInfo FullImage
        {
            get
            {
                if (normalThing == null)
                {
                    normalThing = new TexturePosInfo();
                    normalThing.front = new Rectangle(0, 0, 64, 32);
                    normalThing.left = new Rectangle(0, 0, 64, 32);
                    normalThing.rear = new Rectangle(0, 0, 64, 32);
                    normalThing.right = new Rectangle(0, 0, 64, 32);
                    normalThing.top = new Rectangle(0, 0, 64, 32);
                    normalThing.bottom = new Rectangle(0, 0, 64, 32);
                }
                return normalThing;
            }
        }

        //public static TexturePosInfo FullImageWithSize(float sizeFactor)
        //{
        //    var normalThingTen = new TexturePosInfo();
        //    normalThingTen.front = new Rectangle(0, 0, (int)(64.0f / sizeFactor), (int)(32.0f / sizeFactor));
        //    normalThingTen.left = new Rectangle(0, 0, (int)(64.0f / sizeFactor), (int)(32.0f / sizeFactor));
        //    normalThingTen.rear = new Rectangle(0, 0, (int)(64.0f / sizeFactor), (int)(32.0f / sizeFactor));
        //    normalThingTen.right = new Rectangle(0, 0, (int)(64.0f / sizeFactor), (int)(32.0f / sizeFactor));
        //    normalThingTen.top = new Rectangle(0, 0, (int)(64.0f / sizeFactor), (int)(32.0f / sizeFactor));
        //    normalThingTen.bottom = new Rectangle(0, 0, (int)(64.0f / sizeFactor), (int)(32.0f / sizeFactor));
        //    return normalThingTen;
        //}
    }
}
