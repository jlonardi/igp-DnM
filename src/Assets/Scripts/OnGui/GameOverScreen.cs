using UnityEngine;
using System.Collections;

public class GameOverScreen {
	private GameManager game;

	// this get's called from game manager when GameState.GAME_OVER
	public void Show(){
		if (game == null){
			game = GameManager.instance;
		}

		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize=30;
		GUI.Box(new Rect((Screen.width *0.5f)-138, (Screen.height*0.5f)-100,275,250),"Game Over", myStyle);
		GUI.Label(new Rect((Screen.width *0.5f)-100, (Screen.height*0.5f)-150,275,250), "Health: " + game.statistics.playerHealth);
		GUI.Label(new Rect((Screen.width *0.5f)-100, (Screen.height*0.5f)-125,275,250), "Treasure: " + game.statistics.treasureAmount);
		GUI.Label (new Rect((Screen.width *0.5f)-100, (Screen.height*0.5f)-100,275,250), "Body Count: " + game.statistics.bodycount);
		GUI.Label (new Rect((Screen.width *0.5f)-100, (Screen.height*0.5f)-75,275,250), "Score: " + game.statistics.score);
		GUILayout.BeginArea(new Rect((Screen.width *0.5f)-50, (Screen.height*0.5f)+100,100,200));
		if(GUILayout.Button ("Main Menu"))
		{
			// load Main Menu scene
			Application.LoadLevel("Main Meny");
		}
		GUILayout.EndArea();	
	}
	
	
}
