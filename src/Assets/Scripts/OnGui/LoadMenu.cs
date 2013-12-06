using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary; 

public class LoadMenu {	
	private OnGuiManager gui;
	private int centerX;
	private int centerY;

	public void Initialize(){
		gui = OnGuiManager.instance;
		centerX = gui.GetCenterX();
		centerY = gui.GetCenterY();
	}

	// Show() gets called from OnGuiManager
	public void Show(){
		if (gui == null){
			Initialize();
		}

		SaveInfo[] saveInfo = SaveManager.instance.saveInfo;

		GUI.skin.GetStyle("window");
		GUI.Box(new Rect(centerX-500, 100,1000,850),"", "window");
		
		GUILayout.BeginArea(new Rect(centerX-220, 210,440,600));
		GUILayout.Label("Select slot to load", "textfield");
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(centerX-375, 330,350,600));

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
					GUILayout.Window(i, new Rect(centerX, 300,430,500), ShowDetails, "");
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

	//get actual playing time from float value
	private string ConvertPlayTime(float playTime){
		TimeSpan t = TimeSpan.FromSeconds(playTime);
		return string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds);
	}

	//get actual level name from level number
	private string ConvertLevelName(int levelNumber) {
		switch(levelNumber){
		case 1:
			return "Hidden Valley";
		case 2:
			return "Forest";
		case 3:
			return "Mountain";
		default:
			return levelNumber.ToString();
		}
	}

	private void ShowDetails(int windowID) {
		SaveInfo[] saveInfo = SaveManager.instance.saveInfo;
		if (saveInfo[windowID].name == null || saveInfo[windowID].dateTime == null || saveInfo[windowID].screenshot == null){
			return;
		}

		GUI.Label(new Rect(0,25,450,90), ConvertLevelName(saveInfo[windowID].level));
		GUI.DrawTexture(new Rect(55,100,320,180), saveInfo[windowID].screenshot);
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(40,310,335,20), saveInfo[windowID].dateTime, "plaintext");
		GUI.Label(new Rect(40,360,350,20), ConvertPlayTime(saveInfo[windowID].playTime), "plaintext");
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
	}	

}
