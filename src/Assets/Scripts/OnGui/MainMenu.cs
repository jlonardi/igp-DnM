using UnityEngine;
using System.Collections;

public class MainMenu {
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
	public void Show()
	{
		if (game == null){
			Initialize();
		}

		GUILayout.BeginArea(new Rect(centerX-250, 270,500,600));
		if(GUILayout.Button ("Epic Story")){
			//show story
			game.gameState = GameState.STORY;
		}
		if(GUILayout.Button ("New Game")){
			//show difficulty dialog
			game.gameState = GameState.DIFFICULTY;
		}
		#if UNITY_WEBPLAYER
		// don't show load & save on webplayer
		#else
		if(GUILayout.Button ("Load Game")){
			// load previous save details
			game.saves.GetSaveInfo();
			
			//show load menu
			game.gameState = GameState.LOAD_MENU_MAIN;
		}
		if(GUILayout.Button ("High Scores")){
			//show highscore
			game.gameState = GameState.HIGHSCORE;
		}
		if(GUILayout.Button ("Quit")){
			//if quit game button is pressed game quits
			Application.Quit();
		}
		#endif
		GUILayout.EndArea();
	}		
}

