using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary; 

public class LoadMenu {	
	
	// this get's called from game manager when GameState.LOAD_MENU_MAIN or GameState.LOAD_LEVEL_PAUSE	
	public void Show(){		
		Texture2D[] screenshot = new Texture2D[SaveManager.instance.maxSaveSlots];
		string[] name = new string[SaveManager.instance.maxSaveSlots];
		DateTime[] date = new DateTime[SaveManager.instance.maxSaveSlots];
		
		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize=30;
		GUI.Box(new Rect((Screen.width *0.5f)-138, (Screen.height*0.5f)-100,275,250),"Load Game", myStyle);
	
		GUILayout.BeginArea(new Rect((Screen.width *0.5f)-125, (Screen.height*0.5f)-50,250,200));
		for (int i = 1; i <= SaveManager.instance.maxSaveSlots; i++){
			SaveInfo saveInfo = SaveManager.instance.GetSaveInfo(i, ref screenshot[i-1], ref date[i-1]);
			if (saveInfo.name.Equals("Empty")){
				if (GUILayout.Button("Empty")){
					Debug.Log("Empty save");
				}
			}
			if (!saveInfo.name.Equals("Empty")){
				if(GUILayout.Button(saveInfo.name))
				{
					SaveManager.instance.container.saveSlot = i;
					SaveManager.instance.Load();
				}
			}
		}	
		if(GUILayout.Button ("Return"))	{
			if (GameManager.instance.gameState == GameState.LOAD_MENU_MAIN){
				GameManager.instance.gameState = GameState.MAIN_MENU;
			}
			if (GameManager.instance.gameState == GameState.LOAD_MENU_PAUSE){
				GameManager.instance.gameState = GameState.PAUSE_MENU;
			}
		}
		GUILayout.EndArea();		
	}

}
