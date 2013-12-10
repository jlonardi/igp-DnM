using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighScoreScreen {
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

		string difficultyStr;

		List<Score> scores = scoreManager.getScores();

		switch(game.difficulty){
		case DifficultySetting.EASY:
			difficultyStr = "Easy";
			break;
		case DifficultySetting.HARD:
			difficultyStr = "Hard";
			break;
		case DifficultySetting.EPIC:
			difficultyStr = "Epic";
			break;
		default:
			difficultyStr = "Normal";
			break;
		}

		GUI.Box(new Rect(centerX-600, 25,1200,1000),"", "window");

		GUILayout.BeginArea(new Rect(centerX-220, 135,440,600));
		GUILayout.Label("High Scores - "+difficultyStr, "textfield");
		GUILayout.EndArea();

		//draw column labels
		GUI.Label(new Rect(centerX-520, 250, 100,30), "Rank", "plaintext");
		GUI.Label(new Rect(centerX-390, 250, 100,30), "Name", "plaintext");
		GUI.Label(new Rect(centerX-90, 250, 100,30), "Treasure", "plaintext");
		GUI.Label(new Rect(centerX+60, 250, 100,30), "Enemies", "plaintext");
		GUI.Label(new Rect(centerX+210, 250, 100,30), "Dragon", "plaintext");
		GUI.Label(new Rect(centerX+350, 250, 100,30), "Score", "plaintext");

		// show numbers
		GUILayout.BeginArea(new Rect(centerX-480, 300,300,600));
		for (int i=0; i<scores.Count; i++){
			GUILayout.Label(""+ (i+1), "plaintext");
		}
		GUILayout.EndArea();

		// show names
		GUILayout.BeginArea(new Rect(centerX-375, 300,300,600));
		for (int i=0; i<scores.Count; i++){
			GUILayout.Label(""+scores[i].getName(), "plaintext");
		}
		GUILayout.EndArea();

		// show treasure value
		GUILayout.BeginArea(new Rect(centerX-60, 300,300,600));
		for (int i=0; i<scores.Count; i++){
			GUILayout.Label(""+scores[i].getTreasureValue() + " %", "plaintext");
		}
		GUILayout.EndArea();
		
		// show body count
		GUILayout.BeginArea(new Rect(centerX+90, 300,300,600));
		for (int i=0; i<scores.Count; i++){
			GUILayout.Label(""+scores[i].getBodyCount(), "plaintext");
		}
		GUILayout.EndArea();
		
		// show dragon slayed
		GUILayout.BeginArea(new Rect(centerX+250, 300,300,600));
		for (int i=0; i<scores.Count; i++){
			if (scores[i].getDragonSlayed()){
				GUILayout.Label("x", "plaintext");
			} else {
				GUILayout.Label("-", "plaintext");
			}
		}
		GUILayout.EndArea();

		// show scores
		GUILayout.BeginArea(new Rect(centerX+360, 300,300,600));
		for (int i=0; i<scores.Count; i++){
			GUILayout.Label(""+scores[i].getScore(), "plaintext");
		}
		GUILayout.EndArea();

		// area for bottom buttons
		GUILayout.BeginArea(new Rect(centerX-550, 870, 1100,100));
		GUILayout.BeginHorizontal();
		if(GUILayout.Button ("Back to Main Menu")) {
			game.gameState = GameState.MAIN_MENU;
		}

		if(GUILayout.Button ("Show Easy")){
			game.difficulty = DifficultySetting.EASY;
		}

		if(GUILayout.Button ("Show Normal")){
			game.difficulty = DifficultySetting.NORMAL;
		}

		if(GUILayout.Button ("Show Hard")){
			game.difficulty = DifficultySetting.HARD;
		}

		if(GUILayout.Button ("Show Epic")){
			game.difficulty = DifficultySetting.EPIC;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
	
}
