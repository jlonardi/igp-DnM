using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance;	

	public LevelState levelState;	
	public GameState gameState = GameState.MAIN_MENU;
	public TreasureState treasureState = TreasureState.CARRYING;
	public PickupState pickupState = PickupState.NONE;
	public Statistics statistics = new Statistics();
	public WaveManager waves = new WaveManager();
	void Awake()
    {
		// never destroy Game Manager on scene load
		DontDestroyOnLoad (this);	
	
		//use singleton since only we need once instance of this class
       	GameManager.instance = this;
	
    }	
	
	void Update(){
		// call updates on child managers
		statistics.Update();
		waves.Update();
	}

	public void NewGame(){
		// wait until all gameobjects are loaded
		treasureState = TreasureState.CARRYING;
		gameState = GameState.RUNNING;
		statistics.Reset();
		OnGuiManager.instance.bloodSplatter.bloodAlpha = 0f;
	}
	
	public void GameOver(){
		gameState = GameState.GAME_OVER;
	}
	
}
