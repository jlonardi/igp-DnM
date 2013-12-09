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

		GUI.skin.GetStyle("window");
		GUI.Box(new Rect(centerX-400, 100,800,850),"", "window");

		GUILayout.BeginArea(new Rect(centerX-220, 210,440,600));
		GUILayout.Label("Game Paused", "textfield");
		GUILayout.EndArea();

		GUILayout.BeginArea(new Rect(centerX-175, 330,350,600));
		
		if(GUILayout.Button ("Resume"))	{
			 // to resume game set pause to false
			GameManager.instance.gameState = GameState.RUNNING;
		}

		GUILayout.Space(30);

		#if UNITY_WEBPLAYER
		// don't show load & save on webplayer
		#else
		if(GUILayout.Button ("Load game")) {
			// load previous save details
			SaveManager.instance.GetSaveInfo();
			// show load game menu
			GameManager.instance.gameState = GameState.LOAD_MENU_PAUSE;
		}

		if(GUILayout.Button ("Save game")) {
			// load previous save details
			SaveManager.instance.GetSaveInfo();
			// show save game menu
			GameManager.instance.gameState = GameState.SAVE_MENU;
		}
		  
		GUILayout.Space(30);
		#endif

		if(GUILayout.Button ("Main Menu")) {
			// load main menu scene
			//GameManager.instance.gameState = GameState.MAIN_MENU;
			Application.LoadLevel("Main Meny");			
		}

		#if UNITY_WEBPLAYER
		// don't show quit on webplayer
		#else
		GUILayout.Space(50);

		if(GUILayout.Button ("Quit game"))	{
			// quit game
			Application.Quit();
		}
		#endif
		GUILayout.EndArea();
	}	
}
