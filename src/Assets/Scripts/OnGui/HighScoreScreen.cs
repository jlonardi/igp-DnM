using UnityEngine;
using System.Collections;

public class HighScoreScreen {
	private GameManager game;
	private OnGuiManager gui;
	private int centerX;
	private int centerY;

	public void Initialize(){
		game = GameManager.instance;
		gui = OnGuiManager.instance;
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
		GUILayout.Label("High Scores", "textfield");
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(centerX-150, 350,300,600));
/*
		for (int i=0; i<scoreCount; i++){
			GUILayout.Label(i + ". " + scores[i].name, "plaintext");
		}
*/
		GUILayout.EndArea();

		// area for main menu button
		GUILayout.BeginArea(new Rect(centerX-175, 700,350,600));
		if(GUILayout.Button ("Main Menu"))
		{
			// load Main Menu scene
			Application.LoadLevel("Main Meny");
		}
		GUILayout.EndArea();
	}
	
}
