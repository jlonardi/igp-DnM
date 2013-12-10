using UnityEngine;
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
				Application.LoadLevel(1); //levelNumber							
			}
		}
		
		// Call this to get the savegame name
		public SaveInfo GetSaveInfo(int saveIndex) { 
			SaveInfo saveInfo = new SaveInfo();	
			SaveContainer container = SaveManager.instance.container;

			//reset value so game works with savegames made with older versions
			container.difficulty = DifficultySetting.NORMAL;
			
			saveInfo.screenshot = new Texture2D(320, 180);

			string filePath = saveDirectory + "\\savegame" + (saveIndex + 1) + ".dat";
			if (!File.Exists(filePath)){
				saveInfo.name = null;
			} else {
				container.formatVersion = 0;
				Load(saveIndex, false);
				if (container.formatVersion<3){ //don't show saved games from older versions
					saveInfo.name = null;
				} else if  (container.name != null){
					saveInfo.name = container.name;
				} else {
					saveInfo.name = "Savegame " + (saveIndex + 1);
				}
				if (container.dateTime != null){
					saveInfo.dateTime = container.dateTime;
				} else {
					saveInfo.dateTime = "";
				}
				saveInfo.playTime = container.playTime;
				saveInfo.difficulty = container.difficulty;
				if (container.screenshot != null){
					saveInfo.screenshot.LoadImage(container.screenshot);
				}
				saveInfo.level = container.level;
			}
			return saveInfo;
		}
	}
}

