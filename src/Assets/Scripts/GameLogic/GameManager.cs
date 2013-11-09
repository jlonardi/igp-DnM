using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance;	

	// skin for all menu items
  	public GUISkin guiSkin;
	
	public Statistics statistics = new Statistics();
	
	// OnGUI screens & menus:
	public GameOverScreen gameOverScreen = new GameOverScreen();
	public PauseMenu pauseMenu = new PauseMenu();
	public SaveMenu saveMenu = new SaveMenu();
	public SaveDialog saveDialog = new SaveDialog();
	public LoadMenu loadMenu = new LoadMenu();
	public Hud hud = new Hud();

	public GameState gameState = GameState.MAIN_MENU;
		
	public int wave;

	public void Awake()
    {
		// never destroy Game Manager on scene load
		DontDestroyOnLoad (this);	
	
		//use singleton since only we need once instance of this class
       	GameManager.instance = this;
    }	
	
	void Start () {
	}
	
	public void NewGame(){
		// wait untill all gameobjects are loaded
		gameState = GameState.RUNNING;
		wave = 1;
		statistics.Reset();
	}
	
	public void GameOver(){
		gameState = GameState.GAME_OVER;
	}

	// select which gui items are shown by game state
	void OnGUI()
	{
		switch (gameState)
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
	
	void Update(){		
		switch (gameState)
		{
		case GameState.RUNNING:
  			//when game is not paused time runs normally and the cursor is hidden and locked
			Time.timeScale=1;
			Screen.showCursor=false;
			Screen.lockCursor=true;
		
			if (Input.GetButtonDown("Menu")){
				gameState = GameState.PAUSE_MENU;
			}
			break;
		case GameState.MAIN_MENU:
			Time.timeScale=1;
			Screen.showCursor=true;
			Screen.lockCursor=false;
			break;
		case GameState.PAUSE_MENU:
			//when game is paused time stops and the cursour shows
			Time.timeScale=0;
			Screen.showCursor=true;
			Screen.lockCursor=false;
			if (Input.GetButtonDown("Menu")){
				gameState = GameState.RUNNING;
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
}
