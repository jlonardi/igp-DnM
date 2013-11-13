﻿using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary; 

public class LoadMenu {	

	// this get's called from game manager when GameState.LOAD_MENU_MAIN or GameState.LOAD_LEVEL_PAUSE	
	public void Show(){
		SaveInfo[] saveInfo = SaveManager.instance.saveInfo;
		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize=30;
		GUI.Box(new Rect((Screen.width *0.5f)-300, (Screen.height*0.5f)-100,600,250),"Load Game", myStyle);
		
		GUILayout.BeginArea(new Rect((Screen.width *0.5f)-250, (Screen.height*0.5f)-50,500,200));

		for (int i = 0; i < SaveManager.instance.maxSaveSlots; i++){
			if (saveInfo[i].name == null){
				//disable button if no savegame selected
				GUI.enabled = false;
				if (GUILayout.Button("Empty")){					
					Debug.Log("Empty save");
				}
				GUI.enabled = true;				
			}
			if (saveInfo[i].name != null){
				if(GUILayout.Button(saveInfo[i].name))
				{
					SaveManager.instance.container.saveSlot = i;
					SaveManager.instance.Load();
				}
				if (Event.current.type == EventType.Repaint && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition)){
					GUILayout.Window(i, new Rect(Screen.width *0.5f,10,330,290), ShowDetails, "");
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
	private void ShowDetails(int windowID) {
		SaveInfo[] saveInfo = SaveManager.instance.saveInfo;
		if (saveInfo[windowID].name == null || saveInfo[windowID].dateTime == null || saveInfo[windowID].screenshot == null){
			return;
		}
		GUI.DrawTexture(new Rect(10,10,320,180), saveInfo[windowID].screenshot);
		GUI.Label(new Rect(40,200,200,40), ConvertLevelName(saveInfo[windowID].level));
		GUI.Label(new Rect(40,220,200,40), ConvertPlayTime(saveInfo[windowID].playTime));
		GUI.Label(new Rect(40,240,200,40), saveInfo[windowID].dateTime);
	}

	public string ConvertPlayTime(float playTime){
		TimeSpan t = TimeSpan.FromSeconds(playTime);
		return string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds);
	}

	public string ConvertLevelName(int levelNumber) {
		switch(levelNumber){
		case 1:
			return "Level 1 - Desert";
		case 2:
			return "Level 2 - Forest";
		case 3:
			return "Level 3 - Mountain";
		default:
			return levelNumber.ToString();
		}
	}

}
