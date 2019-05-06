using System;
using System.Collections.Generic;
using System.IO;
using SwinGameSDK;


/// <summary>
/// Controls displaying and collecting high score data.
/// </summary>
/// <remarks>
/// Data is saved to a file.
/// </remarks>
static class HighScoreController
{
	private const int NAME_WIDTH = 3;

	private const int SCORES_LEFT = 490;
	private const int HOME_BUTTON_LEFT = 50;
	private const int HOME_BUTTON_TOP = 400;
	private const int HOME_BUTTON_WIDTH = 200;
	private const int HOME_BUTTON_HEIGHT = 200;
	private static string _aiSettingScores = "easyhighscores.txt";
	private const int SCORES_HEADING = 40;
	private const int SCORES_TOP = 80;
	private const int SCORE_GAP = 30;
	private const int SCORE_BUTTONS_LEFT = 200;
	private const int SCORE_BUTTONS_TOP = 80;
	private const int SCORE_BUTTONS_WIDTH = 230;
	private const int SCORE_BUTTONS_HEIGHT = 70;
	private const int EASY_BUTTON_TOP = 116;
	private const int MEDIUM_BUTTON_TOP = 206;
	private const int HARD_BUTTON_TOP = 292;
	/// <summary>
	/// The score structure is used to keep the name and
	/// score of the top players together.
	/// </summary>

	private struct Score : IComparable
	{
		public string Name;

		public int Value;
		/// <summary>
		/// Allows scores to be compared to facilitate sorting
		/// </summary>
		/// <param name="obj">the object to compare to</param>
		/// <returns>a value that indicates the sort order</returns>
		public int CompareTo(object obj)
		{
			if (obj is Score) {
				Score other = (Score)obj;

				return other.Value - this.Value;
			} else {
				return 0;
			}
		}
	}


	private static List<Score> _Scores = new List<Score>();
	/// <summary>
	/// Loads the scores from the highscores text file.
	/// </summary>
	/// <remarks>
	/// The format is
	/// # of scores
	/// NNNSSS
	/// 
	/// Where NNN is the name and SSS is the score
	/// </remarks>
	private static void LoadScores(string filename)
	{
		string loadfile = SwinGame.PathToResource (filename);
		StreamReader input = default(StreamReader);
		input = new StreamReader(loadfile);

		//Read in the # of scores
		int numScores = 0;
		numScores = Convert.ToInt32(input.ReadLine());

		int i = 0;

		for (i = 1; i <= numScores; i++) {
			Score s = default(Score);
			string line = null;

			line = input.ReadLine();

			s.Name = line.Substring(0, NAME_WIDTH);
			s.Value = Convert.ToInt32(line.Substring(NAME_WIDTH));
			_Scores.Add(s);
		}
		input.Close();
	}

	/// <summary>
	/// Saves the scores back to the highscores text file.
	/// </summary>
	/// <remarks>
	/// The format is
	/// # of scores
	/// NNNSSS
	/// 
	/// Where NNN is the name and SSS is the score
	/// </remarks>
	private static void SaveScores(string filename)
	{
		string savefile = SwinGame.PathToResource (filename);
		StreamWriter output = default(StreamWriter);
		output = new StreamWriter(savefile);

		output.WriteLine(_Scores.Count);

		foreach (Score s in _Scores) {
			output.WriteLine(s.Name + s.Value);
		}

		output.Close();
	}

	/// <summary>
	/// Draws the high scores to the screen.
	/// </summary>
	public static void DrawIndicator (int indicatorPos)
	{
		SwinGame.DrawBitmap (GameResources.GameImage ("Indicator"), SCORE_BUTTONS_LEFT-50, indicatorPos-30);
	}

	public static int ChooseHighScore ()
	{
		int BUTTON_TOP = EASY_BUTTON_TOP;
		if (UtilityFunctions.IsMouseInRectangle (SCORE_BUTTONS_LEFT, EASY_BUTTON_TOP, SCORE_BUTTONS_WIDTH, SCORE_BUTTONS_HEIGHT)) {
			_Scores.Clear ();
			_aiSettingScores = "easyhighscores.txt";
			LoadScores (_aiSettingScores);
			BUTTON_TOP = EASY_BUTTON_TOP;
			DrawIndicator (BUTTON_TOP);
			return BUTTON_TOP;
		} else if (UtilityFunctions.IsMouseInRectangle (SCORE_BUTTONS_LEFT, MEDIUM_BUTTON_TOP, SCORE_BUTTONS_WIDTH, SCORE_BUTTONS_HEIGHT)) {
			_Scores.Clear ();
			_aiSettingScores = "mediumhighscores.txt";
			LoadScores (_aiSettingScores);
			BUTTON_TOP = MEDIUM_BUTTON_TOP;
            DrawIndicator (BUTTON_TOP);
			return BUTTON_TOP;
		} else if (UtilityFunctions.IsMouseInRectangle (SCORE_BUTTONS_LEFT, HARD_BUTTON_TOP, SCORE_BUTTONS_WIDTH, SCORE_BUTTONS_HEIGHT)) {
			_Scores.Clear ();
			_aiSettingScores = "hardhighscores.txt";
			LoadScores (_aiSettingScores);
			BUTTON_TOP = HARD_BUTTON_TOP;
            DrawIndicator (BUTTON_TOP);
			return BUTTON_TOP;
		}
		return BUTTON_TOP;
	}

	public static void DrawHighScores()
	{
		int indicatorPos;

		SwinGame.DrawBitmap(GameResources.GameImage("ScoreButtons"), SCORE_BUTTONS_LEFT, SCORE_BUTTONS_TOP);
		SwinGame.DrawBitmap(GameResources.GameImage("HomeButton"), HOME_BUTTON_LEFT, HOME_BUTTON_TOP);

		if (_Scores.Count == 0) {
			LoadScores (GameController.filename);
		}

			indicatorPos = ChooseHighScore ();

		SwinGame.DrawText("   High Scores   ", Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, SCORES_HEADING);
		//For all of the scores
		int i = 0;
		for (i = 0; i <= _Scores.Count - 1; i++) {
			Score s = default(Score);

			s = _Scores[i];

			//for scores 1 - 9 use 01 - 09
			if (i < 9) {
				SwinGame.DrawText(" " + (i + 1) + ":   " + s.Name + "   " + s.Value, Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
			} else {
				SwinGame.DrawText(i + 1 + ":   " + s.Name + "   " + s.Value, Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
			}
		}

		DeploymentController.DeploySounds ();
	}

	public static void AddHighScore ()
	{
		if (_Scores.Count == 0) {
			LoadScores (GameController.filename);
		}

		SwinGame.DrawText ("   High Scores   ", Color.White, GameResources.GameFont ("Courier"), SCORES_LEFT, SCORES_HEADING);
		//For all of the scores
		int i = 0;
		for (i = 0; i <= _Scores.Count - 1; i++) {
			Score s = default (Score);

			s = _Scores [i];

			//for scores 1 - 9 use 01 - 09
			if (i < 9) {
				SwinGame.DrawText (" " + (i + 1) + ":   " + s.Name + "   " + s.Value, Color.White, GameResources.GameFont ("Courier"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
			} else {
				SwinGame.DrawText (i + 1 + ":   " + s.Name + "   " + s.Value, Color.White, GameResources.GameFont ("Courier"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
			}
		}

		DeploymentController.DeploySounds ();	}

	/// <summary>
	/// Handles the user input during the top score screen.
	/// </summary>
	/// <remarks></remarks>
	public static void HandleHighScoreInput()
	{
		if ((UtilityFunctions.IsMouseInRectangle (HOME_BUTTON_LEFT, HOME_BUTTON_TOP, HOME_BUTTON_WIDTH, HOME_BUTTON_HEIGHT)) && (SwinGame.MouseClicked (MouseButton.LeftButton))) {
			GameController.EndCurrentState();
		}
	}

	/// <summary>
	/// Read the user's name for their highsSwinGame.
	/// </summary>
	/// <param name="value">the player's sSwinGame.</param>
	/// <remarks>
	/// This verifies if the score is a highsSwinGame.
	/// </remarks>
	public static void ReadHighScore(int value)
	{
		const int ENTRY_TOP = 500;

        if (_Scores.Count == 0)
        {
			LoadScores(GameController.filename);
        }
			

		//is it a high score
		if ((value > _Scores[_Scores.Count - 1].Value)) {
			Score s = new Score();
			s.Value = value;

			GameController.AddNewState(GameState.AddingHighScores);

			int x = 0;
			x = SCORES_LEFT + SwinGame.TextWidth(GameResources.GameFont("Courier"), "Name: ");

			SwinGame.StartReadingText(Color.White, NAME_WIDTH, GameResources.GameFont("Courier"), x, ENTRY_TOP);

			//Read the text from the user
			while (SwinGame.ReadingText()) {
				SwinGame.ProcessEvents();

				UtilityFunctions.DrawBackground();
				HighScoreController.AddHighScore();
				SwinGame.DrawText("Name: ", Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, ENTRY_TOP);
				SwinGame.RefreshScreen();
			}

			s.Name = SwinGame.TextReadAsASCII();

			if (s.Name.Length < 3) {
				s.Name = s.Name + new string(Convert.ToChar(" "), 3 - s.Name.Length);
			}

			_Scores.RemoveAt(_Scores.Count - 1);
			_Scores.Add(s);
			_Scores.Sort();

			SaveScores(GameController.filename);

			GameController.EndCurrentState();
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
