using UnityEngine;
using System.Collections;

[System.Serializable]
public class Statistics {
	public int level = 1;
	public int score = 0;
	public int bodycount = 0;
	public float playTime;

	//scores for different enemy types:
	private int orcKillScore = 100;
	
	private float timeOfLastPoint = 0f;
	private float pointIntervall=1f;
	
	//constructor
	public Statistics(){
	}	
	
	// this gets called from GameManager Update()
	public void Update(){
	 	if(Treasure.instance.onGround){
			if((timeOfLastPoint + pointIntervall)<Time.time){
				score++;
				timeOfLastPoint = Time.time;
			}
		}
	}
	
	public void Kill(EnemyType et){
		bodycount++;
		switch (et){
			case(EnemyType.ORC):
			default:
				score += orcKillScore;
				break;		
		}
	}
	
	public void Reset(){
		score = 0;
		bodycount = 0;
	}
}