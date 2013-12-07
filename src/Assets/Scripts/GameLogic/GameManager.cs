using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance;	

	// set this to true on editor, so we can start level on editor without going to main menu
	public bool gameLevel = false;

	public GameState gameState = GameState.MAIN_MENU;
	public PickupState pickupState = PickupState.NONE;
	public Statistics statistics = new Statistics();
	public WaveManager waves = new WaveManager();
	public GunManager weapons;
	public Player player;
	public Treasure treasure;
	public GameObject saveManagerPrefab;

	[HideInInspector]
	public SaveManager saves;

	void Awake()
	{
		//use singleton since only we need once instance of this class
       	GameManager.instance = this;
	}

	void Start()
	{
		GameObject saveManagerObj = GameObject.Find("Save Manager");

		// if Save Manager not present already, create one
		if (!saveManagerObj){
			saveManagerObj = (GameObject)Instantiate(saveManagerPrefab, Vector3.zero, Quaternion.identity);
			saveManagerObj.name = "Save Manager";

			saves = saveManagerObj.GetComponent<SaveManager>();
			// this is only for testing level on editor
			if (gameLevel == true && saves.levelState == LevelState.LOADING_NEWGAME){
				saves.levelState = LevelState.LOADED;
				NewGame();
			}

		} else {                             
			saves = saveManagerObj.GetComponent<SaveManager>();
		}
	}	
	
	void Update(){
		// call updates on child managers
		statistics.Update();
		waves.Update();
	}

	public void NewGame(){
		// wait until all gameobjects are loaded
		gameState = GameState.RUNNING;
		statistics.Reset();
		OnGuiManager.instance.bloodSplatter.SetBloodAlpha(0f);
	}
	
	public void GameOver(){
		gameState = GameState.GAME_OVER;
	}
	
}
