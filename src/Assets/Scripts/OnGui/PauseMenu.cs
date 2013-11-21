using UnityEngine;
using System.Collections;

public class PauseMenu {
	private OnGuiManager gui;
	private int centerX;
	private int centerY;

	public void Initialize(){
		gui = OnGuiManager.instance;
		centerX = gui.GetCenterX();
		centerY = gui.GetCenterY();
	}

	// Show() gets called from OnGuiManager
	public void Show()
	{
		if (gui == null){
			Initialize();
		}

		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize = 30;
		GUI.Box(new Rect(centerX-138, centerY-100,275,250),"Game Paused", myStyle);
		
		GUILayout.BeginArea(new Rect(centerX-50, centerY-50,100,200));
		
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
			// load previous save details
			SaveManager.instance.GetSaveInfo();
			// show save game menu
			GameManager.instance.gameState = GameState.SAVE_MENU;
		}
		if(GUILayout.Button ("Load game"))
		{
			// load previous save details
			SaveManager.instance.GetSaveInfo();
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
