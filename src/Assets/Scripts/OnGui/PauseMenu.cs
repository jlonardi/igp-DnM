using UnityEngine;
using System.Collections;

public class PauseMenu {
	private OnGuiManager gui;
	private int nativeWidth;
	private int nativeHeight;
	private int padWidth;

	public void Initialize(){
		gui = OnGuiManager.instance;
		nativeWidth = gui.nativeWidth;
		nativeHeight = gui.nativeHeight;
		padWidth = gui.padWidth;
	}

	// Show() gets called from OnGuiManager
	public void Show()
	{
		if (gui == null){
			Initialize();
		}

		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize = 30;
		GUI.Box(new Rect(((nativeWidth+padWidth)*0.5f)-138, (nativeHeight*0.5f)-100,275,250),"Game Paused", myStyle);
		
		GUILayout.BeginArea(new Rect(((nativeWidth+padWidth) *0.5f)-50, (nativeHeight*0.5f)-50,100,200));
		
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
