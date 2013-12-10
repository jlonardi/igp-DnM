using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance;	

	// set this to true on editor, so we can start level on editor without going to main menu
	public bool gameLevel = false;

	public GameState gameState = GameState.MAIN_MENU;
	public PickupState pickupState = PickupState.NONE;
	public Statistics statistics;
	public GunManager weapons;
	public Dragon dragon;
	public Player player;
	public Treasure treasure;
	public DifficultySetting difficulty;
	public GameObject saveManagerPrefab;

	[HideInInspector]
	public SaveManager saves;

	void Awake()
	{
		//use singleton since only we need once instance of this class
       	GameManager.instance = this;
		statistics = new Statistics(this);
	}
	
	void Start(){
		if (saves == null){
			GetSaveManager();
		}
	}

	void Update(){
		// call updates on child managers
		statistics.Update();
	}

	public void GameOver(){
		#if UNITY_WEBPLAYER
		gameState = GameState.GAME_OVER;
		return;
		#endif

		// if score can be set into high score list, ask for player name
		if (HighScoreManager.instance.getSmallestScore() < statistics.score){
			gameState = GameState.HIGHSCORE_DIALOG;
		} else {
			gameState = GameState.GAME_OVER;
		}
	}

	private void GetSaveManager(){
		GameObject saveManagerObj = GameObject.Find("Save Manager");
		
		// if Save Manager not present already, create one
		if (!saveManagerObj){
			saveManagerObj = (GameObject)Instantiate(saveManagerPrefab, Vector3.zero, Quaternion.identity);
			saveManagerObj.name = "Save Manager";
			
			saves = saveManagerObj.GetComponent<SaveManager>();
			// this is only for testing level on editor
			if (gameLevel == true && saves.levelState != LevelState.LOADED){
				saves.levelState = LevelState.LOADED;
				NewGame();
			}
			
		} else {                             
			saves = saveManagerObj.GetComponent<SaveManager>();
		}
	}

	void OnLevelWasLoaded(int level) {
		if (saves == null){
			GetSaveManager();
		}
		switch (level){
		case 0: // main menu
			if (saves.levelState == LevelState.LOADING_HIGHSCORE){
				difficulty = saves.difficulty;
				gameState = GameState.HIGHSCORE;
			}
			saves.levelState = LevelState.LOADED;
			break;
		default:
			if (saves.levelState == LevelState.LOADING_SAVE){
				saves.container.RestoreValues();	
				ApplyDifficultySetting();
				saves.levelState = LevelState.LOADED;
			} else {
				NewGame();
			}
			break;
		}
	}

	public void NewGame(){
		// wait until all gameobjects are loaded
		gameState = GameState.RUNNING;
		statistics.Reset();
		OnGuiManager.instance.bloodSplatter.SetBloodAlpha(0f);
		difficulty = saves.difficulty;
		ApplyDifficultySetting();
	}

	public void ApplyDifficultySetting(){
		switch(difficulty){
		case DifficultySetting.EASY:
			break;
		case DifficultySetting.HARD:
			break;
		case DifficultySetting.NIGHTMARE:
			break;
		default: //normal settings
			break;	
		}
	}
}
