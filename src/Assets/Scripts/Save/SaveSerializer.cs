using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 

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
	public void Save(int saveIndex, string saveName)
	{
		string filePath = saveDirectory + "\\savegame" + saveIndex + ".dat";
		
		SaveData data = new SaveData(saveName, EditorApplication.currentScene, null);	
		Stream stream = File.Open(filePath, FileMode.Create);
		BinaryFormatter bformatter = new BinaryFormatter();
		bformatter.Binder = new VersionDeserializationBinder();
		bformatter.Serialize(stream, data);
		stream.Close();
	}
	 
	// Call this to load from a file into "data"
	public void Load(int saveIndex) { 
		string filePath = saveDirectory + "\\savegame" + saveIndex + ".dat";
		
		SaveData data = new SaveData();
		Stream stream = File.Open(filePath, FileMode.Open);
		BinaryFormatter bformatter = new BinaryFormatter();
		bformatter.Binder = new VersionDeserializationBinder();
		data = (SaveData)bformatter.Deserialize(stream);		
		stream.Close();	 
	}
	
	// Call this to get the savegame name
	public SaveInfo GetSaveInfo(int saveIndex, ref Texture2D screenshot, ref DateTime dateTime) { 
		SaveInfo saveInfo = new SaveInfo();

		string filePath = saveDirectory + "\\savegame" + saveIndex + ".dat";
		if (!File.Exists(filePath)){
			saveInfo.name = "Empty";
			saveInfo.dateTime = new DateTime();
			saveInfo.screenshot = null;					
		} else {
			saveInfo.name = "Savegame " + saveIndex;
			saveInfo.dateTime = new DateTime();
			saveInfo.screenshot = null;		
		}
		return saveInfo;
	}
}
	 
public class SaveInfo{
	public string name;
	public DateTime dateTime;
	public Texture2D screenshot;
}
