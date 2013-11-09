using UnityEngine;
using System.Collections;

public class PauseMenu {
	
	// this get's called from game manager when GameState.PAUSE_MENU
	public void Show()
	{
		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize = 30;
		GUI.Box(new Rect((Screen.width *0.5f)-138, (Screen.height*0.5f)-100,275,250),"Game Paused", myStyle);
		
		GUILayout.BeginArea(new Rect((Screen.width *0.5f)-50, (Screen.height*0.5f)-50,100,200));
		
		if(GUILayout.Button ("Resume"))
		{
			 // to resume game set pause to false
			GameManager.instance.gameState = GameState.RUNNING;
		}
		
		if(GUILayout.Button ("Main Menu"))
		{
			// load main menu scene
			GameManager.instance.gameState = GameState.MAIN_MENU;
			Application.LoadLevel("Main Meny");			
		}
		if(GUILayout.Button ("Save game"))
		{
			// show save game menu
			GameManager.instance.gameState = GameState.SAVE_MENU;
		}
		if(GUILayout.Button ("Load game"))
		{
			// show load game menu
			GameManager.instance.gameState = GameState.LOAD_MENU_PAUSE;
		}
		  
		if(GUILayout.Button ("Quit game"))
		{
			// quit game
			Application.Quit();
		}
		
		GUILayout.EndArea ();
	}	
}
