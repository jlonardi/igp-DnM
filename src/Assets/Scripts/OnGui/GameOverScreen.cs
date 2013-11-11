using UnityEngine;
using System.Collections;

public class GameOverScreen {
	
	// this get's called from game manager when GameState.GAME_OVER
	public void Show(){
		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize=30;
		GUI.Box(new Rect((Screen.width *0.5f)-138, (Screen.height*0.5f)-100,275,250),"Game Over", myStyle);
		GUI.Label(new Rect((Screen.width *0.5f)-100, (Screen.height*0.5f)-150,275,250), "Health: " + PlayerHealth.instance.health);
		GUI.Label(new Rect((Screen.width *0.5f)-100, (Screen.height*0.5f)-125,275,250), "Treasure: " + Treasure.instance.treasureAmount);
		GUI.Label (new Rect((Screen.width *0.5f)-100, (Screen.height*0.5f)-100,275,250), "Body Count: " + GameManager.instance.statistics.bodycount);
		GUI.Label (new Rect((Screen.width *0.5f)-100, (Screen.height*0.5f)-75,275,250), "Score: " + GameManager.instance.statistics.score);
		GUILayout.BeginArea(new Rect((Screen.width *0.5f)-50, (Screen.height*0.5f)+100,100,200));
		if(GUILayout.Button ("Main Menu"))
		{
			// load Main Menu scene
			Application.LoadLevel("Main Meny");
		}
		GUILayout.EndArea();	
	}
	
	
}
