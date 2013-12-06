using UnityEngine;

[System.Serializable]
public class OnGuiManager : MonoBehaviour {
	//use singleton since only we need one instance of this class
	public static OnGuiManager instance;

	// skin for all menu items
  	public GUISkin guiSkin;

	public GameOverScreen gameOverScreen = new GameOverScreen();
	public StoryScreen storyScreen = new StoryScreen();
	public PauseMenu pauseMenu = new PauseMenu();
	public SaveMenu saveMenu = new SaveMenu();
	public SaveDialog saveDialog = new SaveDialog();
	public LoadMenu loadMenu = new LoadMenu();
	public Hud hud = new Hud();
	public Crosshair crosshair = new Crosshair();
	public BloodSplatter bloodSplatter = new BloodSplatter();
	public HighScoreScreen highScoreScreen = new HighScoreScreen();
	public GameManager game;

	// resolition which OnGui elements use as target
	private int nativeWidth = 1920;
	private int nativeHeight = 1080;

	// if screen aspect differs from 16:9, use marginal to fix locations
	private int padWidth = 0;

	public void Awake(){
		OnGuiManager.instance = this;
	}	

	private Matrix4x4 GetScalingMatrix(){
		float scale = 1.0f * Screen.height / nativeHeight;
		padWidth = (int)(Screen.width/scale) - nativeWidth;
		return Matrix4x4.TRS (new Vector3(0, 0, 0), Quaternion.identity, new Vector3 (scale, scale, 1f));
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
		case GameState.STORY:
		case GameState.HIGHSCORE_MAIN:
			Time.timeScale=1;
			Screen.showCursor=true;
			Screen.lockCursor=false;
			break;

		case GameState.GAME_OVER:
		case GameState.HIGHSCORE_GAME:
			Time.timeScale=0;
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
			
		case GameState.SAVE_SCREENSHOT:
			break;

		default:
			break;
		}			
	}	

	// select which gui items are shown by game state
	void OnGUI(){
		// if game manager not ready, do nothing
		if (game == null){
			return;
		}
		//set up scaling for OnGui elements
		GUI.matrix = GetScalingMatrix();

		GUI.skin = guiSkin;

		switch (game.gameState)
		{		
		case GameState.PAUSE_MENU:
			bloodSplatter.Show();
			pauseMenu.Show();
			break;
		case GameState.STORY:
			storyScreen.Show();
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
			crosshair.Show();
			break;
		case GameState.HIGHSCORE_GAME:
		case GameState.HIGHSCORE_MAIN:
			highScoreScreen.Show();
			break;
		default:
			break;
		}
	}	
	
	public int GetLeft(){
		return padWidth;
	}
	public int GetCenterX(){
		return (nativeWidth + padWidth)/2;
	}
	public int GetWidth(){
		return nativeWidth + padWidth;
	}
	public int GetTop(){
		return 0;
	}
	public int GetCenterY(){
		return nativeHeight/2;
	}
	public int GetHeight(){
		return nativeHeight;
	}

}
