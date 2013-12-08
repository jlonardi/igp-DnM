using UnityEngine;
using System.Collections;

public class HighScoreDialog {
	private GameManager game;
	private OnGuiManager gui;
	private HighScoreManager scoreManager;
	private int centerX;
	private int centerY;
	private	string playerName;
	private TextEditor editor;

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

		GUILayout.BeginArea(new Rect(centerX-220, 210,440,600));
		GUILayout.Label("New High Score!", "textfield");
		GUILayout.Space(100);
		GUILayout.Label("Enter your name:", "plaintext");

		//if player hits Enter inside textarea, add highscore
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return) {
			AddHighscore();
		}

		GUI.SetNextControlName("PlayerNameInput");
		playerName = GUILayout.TextArea(playerName, 15);

		GUI.FocusControl("PlayerNameInput");
		editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
		editor.selectPos = playerName.Length + 1;
		editor.pos = playerName.Length + 1;


		GUILayout.EndArea();	

		GUILayout.BeginArea(new Rect(centerX-175, 700,350,600));

		//if player hits Next-button, add highscore
		if(GUILayout.Button ("Next")) {
			AddHighscore();
		}

		GUILayout.EndArea();	

	}

	private void AddHighscore(){
		scoreManager.addHighScore(game.statistics.score, game.statistics.bodycount, game.treasure.GetTreasureAmount(),
	                          game.statistics.dragonSlayed, playerName);
		SaveManager.instance.levelState = LevelState.LOADING_HIGHSCORE;
		// load Main Menu scene
		Application.LoadLevel("Main Meny");
	}
	
	
}
