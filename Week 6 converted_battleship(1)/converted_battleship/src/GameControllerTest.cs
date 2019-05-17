using System;
using NUnit.Framework;
using SwinGameSDK;
using System.Collections.Generic;
using NuGet;

namespace BattleShips
{
	[TestFixture()]
	public class GameControllerTest
	{
		[Test()]
		public void AiSettingsTest ()
		{
			AIPlayer _ai;
			BattleShipsGame _theGame;

			_theGame = new BattleShipsGame ();
			_ai = new AIEasyPlayer (_theGame);

			GameController.StartGame ();
			Assert.IsTrue (GameController.ComputerPlayer == _ai);
		}
	}
}
