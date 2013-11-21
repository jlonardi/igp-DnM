using UnityEngine;
using System.Collections;

[System.Serializable]
public class Statistics {
	public int playerHealth;
	public float playerArmor = 0.0f; // armor scale 0-1.0f

	public int treasureAmount;

	//orig amount is used for calculation current percentage of the treasure	
	public int treasureFullAmount;

	public int level;
	public int score;
	public int wave;
	public int bodycount;
	public float playTime;

	public string gunName;
	public int gunRounds;
	public int gunClips;
	public bool gunUnlimitedClips;
	public bool gunReloading;
	public bool gunEnabled;
	public int grenadeCount;
	public float playerSpeed = 0f;

	//scores for different enemy types:
	private int orcKillScore = 100;
	
	private float timeOfLastPoint = 0f;
	private float pointIntervall=1f;

	//constructor
	public Statistics(){
		treasureFullAmount = treasureAmount;
	}	
	
	// this gets called from GameManager Update()
	public void Update(){
		// start adding score as treasure has been stolen
		if(GameManager.instance.treasureState != TreasureState.NOT_PICKED_UP){
			if((timeOfLastPoint + pointIntervall)<Time.time){
				score++;
				timeOfLastPoint = Time.time;
			}
		}
	}

	public void Reset(){
		playerHealth = 100;
		treasureAmount = 100;
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