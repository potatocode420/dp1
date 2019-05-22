using System;
using SwinGameSDK;

namespace MyGame
{
    public class GameMain
    {
        public static void Main()
        {
			//Draw shape
			Color randomRGB = SwinGame.ColorRed();
			float x = 0.0F;
			float y = 0.0F;
			Shape myShape = new Shape (randomRGB, x, y,50,50);
            //Open the game window
            SwinGame.OpenGraphicsWindow("GameMain", 800, 600);
            SwinGame.ShowSwinGameSplashScreen();
            
            //Run the game loop
            while(false == SwinGame.WindowCloseRequested())
            {
                //Fetch the next batch of UI interaction
                SwinGame.ProcessEvents();
				bool a = SwinGame.MouseClicked (SwinGameSDK.MouseButton.LeftButton);
				bool b = SwinGame.MouseClicked (SwinGameSDK.MouseButton.LeftButton);
				if (a == true && b == true) {
					myShape.X = SwinGame.MouseX ();
					myShape.Y = SwinGame.MouseY ();
				}

				Point2D position = SwinGame.MousePosition ();
				bool inShape = SwinGame.PointInRect(position, myShape.X, myShape.Y, 50, 50);
				if (inShape) {
					if (SwinGame.KeyTyped (KeyCode.vk_SPACE)) {
						myShape.color = SwinGame.RandomRGBColor (255);
					}
				}
                
                //Clear the screen and draw the framerate
                SwinGame.ClearScreen(Color.White);
                SwinGame.DrawFramerate(0,0);
                //Draw onto the screen
				myShape.Draw ();
                SwinGame.RefreshScreen(60);

            }
        }
    }
}