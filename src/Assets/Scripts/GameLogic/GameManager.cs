using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance;	

	public LevelState levelState;	
	public GameState gameState = GameState.MAIN_MENU;	
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
		gameState = GameState.RUNNING;
		waves.wave = 1;
		statistics.Reset();
	}
	
	public void GameOver(){
		gameState = GameState.GAME_OVER;
	}
	
}
