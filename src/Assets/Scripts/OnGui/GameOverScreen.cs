using UnityEngine;
using System.Collections;

public class GameOverScreen {
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

		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize=30;
		GUI.Box(new Rect(centerX-138, centerY-100,275,250),"Game Over", myStyle);
		GUI.Label(new Rect(centerX-100, centerY-150,275,250), "Health: " + game.statistics.playerHealth);
		GUI.Label(new Rect(centerX-100, centerY-125,275,250), "Treasure: " + game.statistics.treasureAmount);
		GUI.Label (new Rect(centerX-100, centerY-100,275,250), "Body Count: " + game.statistics.bodycount);
		GUI.Label (new Rect(centerX-100, centerY-75,275,250), "Score: " + game.statistics.score);
		GUILayout.BeginArea(new Rect(centerX-50, centerY+100,100,200));
		if(GUILayout.Button ("Main Menu"))
		{
			// load Main Menu scene
			Application.LoadLevel("Main Meny");
		}
		GUILayout.EndArea();	
	}
	
	
}
