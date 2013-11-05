using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public int Wave = 0;
	public bool gameRunning = true;
	public bool paused = false;
	
	void Start () {
	
	}
			
	public void GameOver(){
		Time.timeScale = 0;
		gameRunning = false;		
	}
}
