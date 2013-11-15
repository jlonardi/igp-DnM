using UnityEngine;
using System.Collections;

[System.Serializable]
public class OnGuiManager : MonoBehaviour {
	//use singleton since only we need one instance of this class
	public static OnGuiManager instance;

	// skin for all menu items
  	public GUISkin guiSkin;

	public GameOverScreen gameOverScreen = new GameOverScreen();
	public PauseMenu pauseMenu = new PauseMenu();
	public SaveMenu saveMenu = new SaveMenu();
	public SaveDialog saveDialog = new SaveDialog();
	public LoadMenu loadMenu = new LoadMenu();
	public Hud hud = new Hud();
	public BloodSplatter bloodSplatter = new BloodSplatter();
	public GameManager game;

	public void Awake(){
		OnGuiManager.instance = this;
	}	

	// select which gui items are shown by game state
	void OnGUI(){
		// if game manager not ready, do nothing
		if (game == null){
			return;
		}

		switch (GameManager.instance.gameState)
		{		
		case GameState.PAUSE_MENU:
			bloodSplatter.Show();
			pauseMenu.Show();
			break;
		case GameState.LOAD_MENU_MAIN:
			loadMenu.Show();
			break;
		case GameState.LOAD_MENU_PAUSE:
			bloodSplatter.Show();
			loadMenu.Show();
			break;
		case GameState.SAVE_MENU:
			bloodSplatter.Show();
			saveMenu.Show();
			break;
		case GameState.SAVE_DIALOG:
			bloodSplatter.Show();
			saveDialog.Show();
			break;
		case GameState.SAVE_SCREENSHOT:
			bloodSplatter.Show();
			//hud.Show();
			break;
		case GameState.MAIN_MENU:
			break;
		case GameState.GAME_OVER:
			bloodSplatter.Show();
			gameOverScreen.Show();
			break;
		case GameState.RUNNING:
			bloodSplatter.Show();
			hud.Show();
			break;
		default:
			break;
		}
	}

	void Update(){
		// if game manager not set, do it
		if (game == null){
			game = GameManager.instance;
		}

		switch (game.gameState)
		{
		case GameState.RUNNING:
  			//when game is not paused time runs normally and the cursor is hidden and locked
			Time.timeScale=1;
			Screen.showCursor=false;
			Screen.lockCursor=true;
			// if menu key pressed, open pause menu
			if (Input.GetButtonDown("Menu")){
				game.gameState = GameState.PAUSE_MENU;
			}
			break;
			
		case GameState.PAUSE_MENU:
			//when game is paused time stops and the cursour shows
			Time.timeScale=0;
			Screen.showCursor=true;
			Screen.lockCursor=false;
			if (Input.GetButtonDown("Menu")){
				game.gameState = GameState.RUNNING;
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
				game.gameState = GameState.MAIN_MENU;
			}			
			break;
			
		case GameState.LOAD_MENU_PAUSE:
		case GameState.SAVE_MENU:
			// if menu key pressed, return to pause menu
			if (Input.GetButtonDown("Menu")){
				game.gameState = GameState.PAUSE_MENU;
			}
			break;

		case GameState.SAVE_DIALOG:
			// if menu key pressed, return to save menu
			if (Input.GetButtonDown("Menu")){
				game.gameState = GameState.SAVE_MENU;
			}			
			break;
			
		case GameState.GAME_OVER:
			//when game is paused time stops and the cursour shows
			Time.timeScale=0;
			Screen.showCursor=true;
			Screen.lockCursor=false;
			break;
			
		case GameState.SAVE_SCREENSHOT:
			break;

		default:
			break;
		}			
	}	
}
