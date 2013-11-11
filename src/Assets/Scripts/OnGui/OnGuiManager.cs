using UnityEngine;
using System.Collections;

[System.Serializable]
public class OnGuiManager {
	// skin for all menu items
  	public GUISkin guiSkin;

	public GameOverScreen gameOverScreen = new GameOverScreen();
	public PauseMenu pauseMenu = new PauseMenu();
	public SaveMenu saveMenu = new SaveMenu();
	public SaveDialog saveDialog = new SaveDialog();
	public LoadMenu loadMenu = new LoadMenu();
	public Hud hud = new Hud();
	
	public void MenuUpdate(){
		switch (GameManager.instance.gameState)
		{
		case GameState.RUNNING:
  			//when game is not paused time runs normally and the cursor is hidden and locked
			Time.timeScale=1;
			Screen.showCursor=false;
			Screen.lockCursor=true;
			// if menu key pressed, open pause menu
			if (Input.GetButtonDown("Menu")){
				GameManager.instance.gameState = GameState.PAUSE_MENU;
			}
			break;
			
		case GameState.PAUSE_MENU:
			//when game is paused time stops and the cursour shows
			Time.timeScale=0;
			Screen.showCursor=true;
			Screen.lockCursor=false;
			if (Input.GetButtonDown("Menu")){
				GameManager.instance.gameState = GameState.RUNNING;
			}
			break;
			
		case GameState.MAIN_MENU:
			Time.timeScale=1;
			Screen.showCursor=true;
			Screen.lockCursor=false;
			break;
			
		case GameState.LOAD_MENU_MAIN:					
			// if menu key pressed, return to main menu
			if (Input.GetButtonDown("Menu")){
				GameManager.instance.gameState = GameState.MAIN_MENU;
			}			
			break;
			
		case GameState.LOAD_MENU_PAUSE:
		case GameState.SAVE_MENU:
			// if menu key pressed, return to pause menu
			if (Input.GetButtonDown("Menu")){
				GameManager.instance.gameState = GameState.PAUSE_MENU;
			}
			break;
			
		case GameState.SAVE_DIALOG:
			// if menu key pressed, return to save menu
			if (Input.GetButtonDown("Menu")){
				GameManager.instance.gameState = GameState.SAVE_MENU;
			}			
			break;
			
		case GameState.GAME_OVER:
			//when game is paused time stops and the cursour shows
			Time.timeScale=0;
			Screen.showCursor=true;
			Screen.lockCursor=false;
			break;
			
		default:
			break;
		}			
	}
	
	// select which gui items are shown by game state
	public void Show(){
		switch (GameManager.instance.gameState)
		{		
		case GameState.PAUSE_MENU:
			pauseMenu.Show();
			break;
		case GameState.LOAD_MENU_MAIN:
		case GameState.LOAD_MENU_PAUSE:
			loadMenu.Show();
			break;
		case GameState.SAVE_MENU:
			saveMenu.Show();
			break;
		case GameState.SAVE_DIALOG:
			saveDialog.Show();
			break;
		case GameState.MAIN_MENU:
			break;
		case GameState.GAME_OVER:
			gameOverScreen.Show();
			break;
		case GameState.RUNNING:
			hud.Show();
			break;
		default:
			break;
		}
	}
}
