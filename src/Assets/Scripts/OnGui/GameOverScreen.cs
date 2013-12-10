using UnityEngine;
using System.Collections;

public class GameOverScreen {
	private GameManager game;
	private OnGuiManager gui;
	private HighScoreManager scoreManager;
	private int centerX;
	private int centerY;

	public void Initialize(){
		game = GameManager.instance;
		gui = OnGuiManager.instance;
		scoreManager = HighScoreManager.instance;
		centerX = gui.GetCenterX();
		centerY = gui.GetCenterY();
	}

	// Show() gets called from OnGuiManager
	public void Show(){
		if (game == null){
			Initialize();
		}
		GUI.skin.GetStyle("window");
		GUI.Box(new Rect(centerX-400, 100,800,850),"", "window");

		GUILayout.BeginArea(new Rect(centerX-220, 210,440,600));
		GUILayout.Label("Game Over", "textfield");
		GUILayout.EndArea();

		GUILayout.BeginArea(new Rect(centerX-100, 400,300, 400));
		GUILayout.Label("Treasure: " + game.treasure.GetTreasureAmount(), "plaintext");
		GUILayout.Label ("Enemies: " + game.statistics.bodycount, "plaintext");
		if (game.statistics.dragonSlayed){
			GUILayout.Label ("Dragon was killed!", "plaintext");
		} else {
			GUILayout.Label ("Dragon is still alive", "plaintext");
		}
		GUILayout.Label ("Total score: " + game.statistics.score, "plaintext");
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
