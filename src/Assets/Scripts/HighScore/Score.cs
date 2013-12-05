using UnityEngine;
using System;
using System.Collections;
using System.Runtime.Serialization;

[Serializable()] public class
Score : IComparable<Score>{
	private int score;
	private string playerName;

	public Score(int scores, string name){
		this.score=scores;
		this.playerName=name;
	}
	
	public int getScore(){
		return this.score;
	}
	
	public string getName(){
		return this.playerName;
	}


	public int CompareTo(Score score1) {
		if(this.score==score1.getScore()){
			return 0;
		}
		else if(this.score>score1.getScore()){
			return 1;
		}
		else {
			return -1;
		}
	}
}
