using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	//use singleton since only we need once instance of this class
	public static GameManager instance;
    public void Awake()
    {
		
        	GameManager.instance = this;
		
    }	
	
	public int wave;
	public int score;
	public int bodyCount;
	
	//playTime updated only when game ends for statistics
	public float playTime;
	
	public bool gameRunning = true;
	public bool paused = false;
	[SerializeThis]
	private float timeOfLastPoint = 0f;
	private float pointIntervall=1f;
	
	void Start () {
		NewGame();
	}
	
	public void NewGame(){
		wave = 1;
		score = 0;
		bodyCount = 0;
		Time.timeScale = 1;
		gameRunning = true;
	}
	
	public void GameOver(){
		Time.timeScale = 0;
		playTime = Time.time;
		gameRunning = false;		
	}
	public void Update(){
	 	if(Treasure.instance.onGround){
			if((timeOfLastPoint + pointIntervall)<Time.time){
				score++;
				timeOfLastPoint=Time.time;
			}
		}
	}
}
