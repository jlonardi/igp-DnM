using UnityEngine;
using System;
using System.Collections;
using System.Runtime.Serialization;

[Serializable()] 
public class Score : IComparable<Score>{
	private int score;
	private int bodyCount;
	private int treasureValue;
	private bool dragonSlayed;
	private string playerName;

	public Score(int score, int bodyCount, int treasureValue, bool dragonSlayed, string name){
		this.score = score;
		this.bodyCount = bodyCount;
		this.treasureValue = treasureValue;
		this.dragonSlayed = dragonSlayed;
		this.playerName = name;
	}
	
	public int getScore(){
		return this.score;
	}
	
	public string getName(){
		return this.playerName;
	}

	public int getBodyCount(){
		return this.bodyCount;
	}

	public int getTreasureValue(){
		return this.treasureValue;
	}

	public bool getDragonSlayed(){
		return this.dragonSlayed;
	}

	public int CompareTo(Score score1) {
		if(this.score==score1.getScore()){
			return 0;
		}
		else if(this.score<score1.getScore()){
			return 1;
		}
		else {
			return -1;
		}
	}
}
