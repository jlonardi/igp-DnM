using UnityEngine;
using System.Collections;

[System.Serializable]
public class Statistics {
	public int level;
	public int score;
	public int wave;
	public int bodycount;
	public float playTime;
	public bool dragonSlayed;
	
	public float playerSpeed = 0f;

	//scores for different enemy types:
	private int orcKillScore = 100;
	
	private float timeOfLastPoint = 0f;
	private float pointIntervall=1f;

	// this gets called from GameManager Update()
	public void Update(){
		if((timeOfLastPoint + pointIntervall)<Time.time){
			score++;
			timeOfLastPoint = Time.time;
		}
	}

	public void Reset(){
		level = 1;
		score = 0;
		bodycount = 0;
		wave = 0;
	}

	public void AddKillStats(EnemyType et){
		bodycount++;
		switch (et){
		case(EnemyType.ORC):
		default:
			score += orcKillScore;
			break;		
		}
	}

}