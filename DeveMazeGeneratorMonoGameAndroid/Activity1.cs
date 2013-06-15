using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using DeveMazeGeneratorMonoGame;

namespace DeveMazeGeneratorMonoGameAndroid
{
    [Activity(Label = "DeveMazeGeneratorMonoGameAndroid"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        private Game1 game;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Game1.Activity = this;
            game = new Game1();
            SetContentView(game.Window);
            game.Run();
        }

        public override bool OnKeyDown(Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
        {
            if (keyCode == Android.Views.Keycode.Back)
            {
                game.shouldShutdown = true;
            }
            return base.OnKeyDown(keyCode, e);
        }
    }
}

