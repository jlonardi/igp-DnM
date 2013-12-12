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
		//set initial health for safe vale, will be corrected on difficulty settings
		ApplyDifficultySetting();
	}

	public void ApplyDifficultySetting(){
		EnemyManager enemies = EnemyManager.instance;

		switch(difficulty){
		case DifficultySetting.EASY:
			player.SetEnemyDamage(0.5f);
			player.SetFireDamage(0.3f);
			enemies.maxEnemies = 1;
			enemies.timeBetweenEnemyCountAddition = 30;
			dragon.SetHealth(8000);
			dragon.SetMaxHealth(8000);
			enemies.waveIntervalOnDragonFight = 2f;
			enemies.maxDragonFightEnemies = 5;
			weapons.guns[1].unlimited = true;
			weapons.guns[2].unlimited = true;
			weapons.guns[3].unlimited = true;
			weapons.guns[4].unlimited = true;
			break;
		case DifficultySetting.NORMAL:
			player.SetEnemyDamage(1f);
			player.SetFireDamage(0.5f);
			enemies.maxEnemies = 2;
			enemies.timeBetweenEnemyCountAddition = 20;
			dragon.SetHealth(10000);
			dragon.SetMaxHealth(10000);
			enemies.waveIntervalOnDragonFight = 1.5f;
			enemies.maxDragonFightEnemies = 10;
			weapons.guns[1].totalClips = 20;
			weapons.guns[2].totalClips = 10;
			weapons.guns[3].totalClips = 5;
			weapons.guns[4].totalClips = 20;
			break;	
		case DifficultySetting.HARD:
			player.SetEnemyDamage(1.2f);
			player.SetFireDamage(0.8f);
			enemies.maxEnemies = 10;
			enemies.timeBetweenEnemyCountAddition = 10;
			dragon.SetHealth(15000);
			dragon.SetMaxHealth(15000);
			enemies.waveIntervalOnDragonFight = 1.0f;
			enemies.maxDragonFightEnemies = 20;
			weapons.guns[1].totalClips = 15;
			weapons.guns[2].totalClips = 7;
			weapons.guns[3].totalClips = 4;
			weapons.guns[4].totalClips = 15;
			break;
		case DifficultySetting.EPIC:
			player.SetEnemyDamage(1.4f);
			player.SetFireDamage(1.2f);
			enemies.maxEnemies = 15;
			enemies.timeBetweenEnemyCountAddition = 5;
			dragon.SetHealth(20000);
			dragon.SetMaxHealth(20000);
			enemies.waveIntervalOnDragonFight = 1.0f;
			enemies.maxDragonFightEnemies = 40;
			weapons.guns[1].totalClips = 10;
			weapons.guns[2].totalClips = 5;
			weapons.guns[3].totalClips = 3;
			weapons.guns[4].totalClips = 10;
			break;
		}
	}
}
