using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary; 

public class HighScoreManager : MonoBehaviour {
	//use singleton since only we need once instance of this class
	public static HighScoreManager instance;

	private List<Score> scores;
	private string saveDirectory;
	private int highestID;

	void Awake(){
		HighScoreManager.instance = this;
	}

	private void initialize(){
		scores = new List<Score>();
		findSaveDirectory();
		loadScoresFromFile();
		fingHighestID();
	}

	//sorts the score array
	private void sort(){
		scores.Sort();
	}
	//returns the smallest score if there is less than 10 scores (max amount of scores) it returns 0
	public int getSmallestScore() {
		initialize();
		if (scores.Count<10) {
			return 0;
		}
		else {
			return scores[9].getScore();
		}
	}
	//adds a new high score to list
	public void addHighScore(int score, int bodyCount, int treasureValue, bool dragonSlayed, string name){
		initialize();
		this.highestID++;
		scores.Add(new Score(score, bodyCount, treasureValue, dragonSlayed, name,highestID));
		sort ();
		if (scores.Count>10){
			scores.RemoveAt(10);
		}
		updateScoreFile();
	
	}
	//returns the score list
	public List<Score> getScores() {
		initialize();
		return scores;
	}

	//finds the highest id in the list
	public void fingHighestID(){
		int count=scores.Count;
		this.highestID=0;
		if(count==0){
				
			return;
		}
		foreach(Score score in scores){
			int iD=score.getID();
			if(iD>this.highestID){
				this.highestID=iD;
			}
		}
	}


	//tries to find the directory where to save, creates new one if it doesn't excist
	private void findSaveDirectory(){
		string docsDirectory;
		try {
			docsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		} catch (Exception){
			//if can't get My Documents-folder, put saves in <current folder>/highScore/
			docsDirectory = "highscore"; 
		}
		
		saveDirectory = docsDirectory + "\\Dragons and Miniguns";
		
		if (!Directory.Exists(saveDirectory))
			Directory.CreateDirectory(saveDirectory);
	}


	//loads the old scores from the score file
	private void loadScoresFromFile(){
		Stream stream;
		int difficulty = (int)GameManager.instance.difficulty;
		string filePath = saveDirectory + "\\highscore"+difficulty+".dat";
	
		if (!File.Exists(filePath)){
			return;
		}

		stream = File.Open(filePath, FileMode.Open);
		if(stream.Length==0){
			stream.Close();
			return;
		}
		BinaryFormatter bformatter = new BinaryFormatter();
		scores=(List<Score>)bformatter.Deserialize(stream);
		stream.Close();
	}

	//updates the score file
	private void updateScoreFile(){
		int difficulty = (int)GameManager.instance.difficulty;
		string filePath = saveDirectory + "\\highscore"+difficulty+".dat";

		Stream stream = File.Open(filePath, FileMode.OpenOrCreate);
		BinaryFormatter bformatter = new BinaryFormatter();
		bformatter.Serialize(stream, scores);
		stream.Close();
	}
}
