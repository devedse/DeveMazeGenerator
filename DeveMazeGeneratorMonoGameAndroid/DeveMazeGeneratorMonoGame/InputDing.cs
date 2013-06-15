using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeveMazeGeneratorMonoGame
{
    public static class InputDing
    {
        public static MouseState CurMouse = Mouse.GetState();
        public static KeyboardState CurKey = Keyboard.GetState();
        public static MouseState PreMouse = Mouse.GetState();
        public static KeyboardState PreKey = Keyboard.GetState();

        public static void PreUpdate()
        {
            CurMouse = Mouse.GetState();
            CurKey = Keyboard.GetState();
        }

        public static void AfterUpdate()
        {
            PreMouse = Mouse.GetState();
            PreKey = CurKey;
        }

        public static bool KeyDownUp(Keys key)
        {
            return CurKey.IsKeyDown(key) && PreKey.IsKeyUp(key);
        }
    }
}
