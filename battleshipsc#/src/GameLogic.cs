using System;
using SwinGameSDK;
using static MenuController;
using static DeploymentController;
using static DiscoveryController;
using static UtilityFunctions;
using static EndingGameController;
using static GameController;
using static GameResources;

namespace Battleships
{
	public class GameMain
	{

		static class GameLogic
		{
			public static void Main ()
			{
				// Opens a new Graphics Window
				SwinGame.OpenGraphicsWindow ("Battle Ships", 800, 600);

				// Load Resources
				LoadResources ();

				SwinGame.PlayMusic (GameMusic ("Background"));

				// Game Loop
				do {
					HandleUserInput ();
					DrawScreen ();
				}
				while (!SwinGame.WindowCloseRequested () == true | CurrentState == GameState.Quitting);

				SwinGame.StopMusic ();

				// Free Resources and Close Audio, to end the program.
				FreeResources ();
		}
	}
}
}