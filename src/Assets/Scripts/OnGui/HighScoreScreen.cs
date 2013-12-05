﻿using UnityEngine;
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

		List<Score> scores = scoreManager.getScores();

		GUI.Box(new Rect(centerX-600, 25,1200,1000),"", "window");

		GUILayout.BeginArea(new Rect(centerX-220, 135,440,600));
		GUILayout.Label("High Scores", "textfield");
		GUILayout.EndArea();

		//draw column labels
		GUI.Label(new Rect(220, 250, 100,30), "Rank", "plaintext");
		GUI.Label(new Rect(350, 250, 100,30), "Name", "plaintext");
		GUI.Label(new Rect(650, 250, 100,30), "Treasure", "plaintext");
		GUI.Label(new Rect(800, 250, 100,30), "Enemies", "plaintext");
		GUI.Label(new Rect(950, 250, 100,30), "Dragon", "plaintext");
		GUI.Label(new Rect(1090, 250, 100,30), "Score", "plaintext");

		// show numbers
		GUILayout.BeginArea(new Rect(260, 300,300,600));
		for (int i=0; i<scores.Count; i++){
			GUILayout.Label(""+ (i+1), "plaintext");
		}
		GUILayout.EndArea();

		// show names
		GUILayout.BeginArea(new Rect(365, 300,300,600));
		for (int i=0; i<scores.Count; i++){
			GUILayout.Label(""+scores[i].getName(), "plaintext");
		}
		GUILayout.EndArea();

		// show treasure value
		GUILayout.BeginArea(new Rect(680, 300,300,600));
		for (int i=0; i<scores.Count; i++){
			GUILayout.Label(""+scores[i].getTreasureValue(), "plaintext");
		}
		GUILayout.EndArea();
		
		// show body count
		GUILayout.BeginArea(new Rect(830, 300,300,600));
		for (int i=0; i<scores.Count; i++){
			GUILayout.Label(""+scores[i].getBodyCount(), "plaintext");
		}
		GUILayout.EndArea();
		
		// show dragon slayed
		GUILayout.BeginArea(new Rect(990, 300,300,600));
		for (int i=0; i<scores.Count; i++){
			if (scores[i].getDragonSlayed()){
				GUILayout.Label("x", "plaintext");
			} else {
				GUILayout.Label("-", "plaintext");
			}
		}
		GUILayout.EndArea();

		// show scores
		GUILayout.BeginArea(new Rect(1090, 300,300,600));
		for (int i=0; i<scores.Count; i++){
			GUILayout.Label(""+scores[i].getScore(), "plaintext");
		}
		GUILayout.EndArea();

		// area for main menu button
		GUILayout.BeginArea(new Rect(centerX-175, 870,350,600));
		if(GUILayout.Button ("Main Menu"))
		{
			// load Main Menu scene
			Application.LoadLevel("Main Meny");
		}
		GUILayout.EndArea();
	}
	
}
