using UnityEngine;
using System.Collections;

public class GameOverScreen {
	private GameManager game;
	private OnGuiManager gui;
	private HighScoreManager scoreManager;
	private int centerX;
	private int centerY;
	private	string playerName;

	public void Initialize(){
		game = GameManager.instance;
		gui = OnGuiManager.instance;
		scoreManager = HighScoreManager.instance;
		centerX = gui.GetCenterX();
		centerY = gui.GetCenterY();
		playerName = "";
	}

	// Show() gets called from OnGuiManager
	public void Show(){
		if (game == null){
			Initialize();
		}
		GUI.skin.GetStyle("window");
		GUI.Box(new Rect(centerX-400, 100,800,850),"", "window");

		// if score can be set into high score list, ask for player name
		if (scoreManager.getSmallestScore() < game.statistics.score){
			GUILayout.BeginArea(new Rect(centerX-220, 210,440,600));
			GUILayout.Label("New High Score!", "textfield");
			GUILayout.Space(100);
			GUILayout.Label("Enter your name:", "plaintext");

			//if player hits Enter inside textarea, add highscore
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return) {
				AddHighscore();
			}

			playerName = GUILayout.TextArea(playerName, 18);
			GUILayout.EndArea();	

			GUILayout.BeginArea(new Rect(centerX-175, 700,350,600));

			//if player hits Next-button, add highscore
			if(GUILayout.Button ("Next"))
			{
				AddHighscore();
			}
			GUILayout.EndArea();	

		// if wasn't good enough to make into high score list, just show statistics
		} else {
			GUILayout.BeginArea(new Rect(centerX-220, 210,440,600));
			GUILayout.Label("Game Over", "textfield");
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(centerX-100, 400,300, 400));
			GUILayout.Label("Treasure: " + game.statistics.treasureAmount, "plaintext");
			GUILayout.Label ("Enemies: " + game.statistics.bodycount, "plaintext");
			GUILayout.Label ("Score: " + game.statistics.score, "plaintext");
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(centerX-175, 700,350,600));
			if(GUILayout.Button ("Main Menu"))
			{
				// load Main Menu scene
				Application.LoadLevel("Main Meny");
			}
			GUILayout.EndArea();	
		}
	}

	private void AddHighscore(){
		scoreManager.addHighScore(game.statistics.score, game.statistics.bodycount, game.statistics.treasureAmount,
		                          game.statistics.dragonSlayed, playerName);
		game.gameState = GameState.HIGHSCORE_GAME;				

	}
	
	
}
