﻿using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 

namespace UnitySerialization {	
	public class SaveSerializer {
		
		private string saveDirectory;
		
		public SaveSerializer(){
			string docsDirectory;
			try {
				docsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			} catch (Exception){
				//if can't get My Documents-folder, put saves in <current folder>/savegame/
				docsDirectory = "savegame"; 
			}
			
			saveDirectory = docsDirectory + "\\Dragons and Miniguns";
			
			if (!Directory.Exists(saveDirectory))
	    		Directory.CreateDirectory(saveDirectory);
		}
		
		// Call this to write data
		public void Save(int saveIndex)
		{
			string filePath = saveDirectory + "\\savegame" + (saveIndex + 1) + ".dat";
			
			SaveData data = new SaveData();
			Stream stream = File.Open(filePath, FileMode.Create);
			BinaryFormatter bformatter = new BinaryFormatter();
			bformatter.Binder = new VersionDeserializationBinder();
			bformatter.Serialize(stream, data);
			stream.Close();
		}
		 
		// Call this to restore filedata and load a saved game
		public void Load(int saveIndex) { 
			Load (saveIndex, true);
		}

		public void Load(int saveIndex, bool loadLevel) { 
				string filePath = saveDirectory + "\\savegame" + (saveIndex + 1) + ".dat";
			
			SaveData data = new SaveData();
			Stream stream = File.Open(filePath, FileMode.Open);
			BinaryFormatter bformatter = new BinaryFormatter();
			bformatter.Binder = new VersionDeserializationBinder();
			data = (SaveData)bformatter.Deserialize(stream);		
			stream.Close();

			if (loadLevel){
				// when data is restored into save container, load level
				GameManager.instance.levelState = LevelState.LOADING_SAVE;
				Application.LoadLevel(1); //levelNumber							
			}
		}
		
		// Call this to get the savegame name
		public SaveInfo GetSaveInfo(int saveIndex) { 
			SaveInfo saveInfo = new SaveInfo();	
			saveInfo.screenshot = new Texture2D(320, 180);

			string filePath = saveDirectory + "\\savegame" + (saveIndex + 1) + ".dat";
			if (!File.Exists(filePath)){
				saveInfo.name = null;
			} else {
				Load(saveIndex, false);
				if (SaveManager.instance.container.name != null){
					saveInfo.name = SaveManager.instance.container.name;
				} else {
					saveInfo.name = "Savegame " + (saveIndex + 1);
				}
				if (SaveManager.instance.container.dateTime != null){
					saveInfo.dateTime = SaveManager.instance.container.dateTime;
				} else {
					saveInfo.dateTime = "";
				}
				if (SaveManager.instance.container.playTime != null){
					saveInfo.playTime = SaveManager.instance.container.playTime;
				} else {
					saveInfo.playTime = 0f;
				}
				if (SaveManager.instance.container.screenshot != null){
					saveInfo.screenshot.LoadImage(SaveManager.instance.container.screenshot);
				}
				saveInfo.level = SaveManager.instance.container.level;
			}
			return saveInfo;
		}
	}
}

