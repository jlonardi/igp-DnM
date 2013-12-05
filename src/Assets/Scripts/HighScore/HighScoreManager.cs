using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary; 

public class HighScoreManager : MonoBehaviour {


	private List<Score> scores;
	private string saveDirectory;

	void Start(){

		scores = new List<Score>();
		findSaveDirectory();
		loadScoresFromFile();
	
	}
	//sorts the score array
	private void sort(){
		scores.Sort();
	}
	//returns the smallest score if there is less than 10 scores (max amount of scores) it returns 0
	public int getSmallestScore() {
		if (scores.Count<10) {
			return 0;
		}
		else {
			return scores[9].getScore();
		}
	}
	//returns the score list
	public List<Score> getScores() {
		return scores;
	}




	//tries to find the directory where to save, creates new one if it doesn't excist
	private void findSaveDirectory(){
		string docsDirectory;
		try {
			docsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		} catch (Exception){
			//if can't get My Documents-folder, put saves in <current folder>/highScore/
			docsDirectory = "highScore"; 
		}
		
		saveDirectory = docsDirectory + "\\Dragons and Miniguns";
		
		if (!Directory.Exists(saveDirectory))
			Directory.CreateDirectory(saveDirectory);
	}


	//loads the old scores from the score file
	private void loadScoresFromFile(){
		string filePath = saveDirectory + "\\highScore"+".dat";
		Stream stream = File.Open(filePath, FileMode.OpenOrCreate);
		BinaryFormatter bformatter = new BinaryFormatter();
		scores=(List<Score>)bformatter.Deserialize(stream);
		stream.Close();
	}
	//uppdates the score file
	private void uppdateScoreFile(){
		string filePath = saveDirectory + "\\highScore"+".dat";
		Stream stream = File.Open(filePath, FileMode.Create);
		BinaryFormatter bformatter = new BinaryFormatter();
		bformatter.Serialize(stream, scores);
		stream.Close();
	}

	


}
