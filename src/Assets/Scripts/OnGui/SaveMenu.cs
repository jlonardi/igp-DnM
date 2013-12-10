using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary; 


public class SaveMenu {
	private SaveInfo[] saveInfo;
	private OnGuiManager gui;
	private GameManager game;
	private int centerX;
	private int centerY;

	public void Initialize(){
		gui = OnGuiManager.instance;
		game = GameManager.instance;
		centerX = gui.GetCenterX();
		centerY = gui.GetCenterY();
		saveInfo = SaveManager.instance.saveInfo;
	}

	// Show() gets called from OnGuiManager
	public void Show(){
		if (gui == null){
			Initialize();
		}

		GUI.skin.GetStyle("window");
		GUI.Box(new Rect(centerX-500, 100,1000,850),"", "window");
		
		GUILayout.BeginArea(new Rect(centerX-220, 210,440,600));
		GUILayout.Label("Select Save Slot", "textfield");
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(centerX-375, 330,350,600));

		for (int i = 0; i < game.saves.maxSaveSlots; i++){
			string saveName;
			if (saveInfo[i].name != null){
				saveName = saveInfo[i].name;
			} else {
				saveName = "Empty";
			}

			if(GUILayout.Button(saveName))
			{				
				game.gameState = GameState.SAVE_SCREENSHOT;
				// take screenshot now as it takes additional frame to complete
				game.saves.GrabScreenShot();
				
				game.saves.container.saveSlot = i;
				game.saves.container.name = saveInfo[i].name;
			}

			if (saveInfo[i].name != null && Event.current.type == EventType.Repaint 
			    	&& GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition)){
				GUILayout.Window(i, new Rect(centerX, 300,430,550), ShowDetails, "");
			}

		}					

		GUILayout.Space(40);

		if(GUILayout.Button ("Back"))	{
			game.gameState = GameState.PAUSE_MENU;
		}
		GUILayout.EndArea();		
	}

	private void ShowDetails(int windowID) {
		SaveInfo[] saveInfo = game.saves.saveInfo;
		if (saveInfo[windowID].name == null || saveInfo[windowID].dateTime == null || saveInfo[windowID].screenshot == null){
			return;
		}
		string difficultyStr;
		switch(saveInfo[windowID].difficulty){
		case DifficultySetting.EASY:
			difficultyStr = "Easy";
			break;
		case DifficultySetting.HARD:
			difficultyStr = "Hard";
			break;
		case DifficultySetting.EPIC:
			difficultyStr = "Epic";
			break;
		default:
			difficultyStr = "Normal";
			break;
		}
		
		//GUI.Label(new Rect(40,275,350,50), ConvertLevelName(saveInfo[windowID].level));
		GUI.DrawTexture(new Rect(55,110,320,180), saveInfo[windowID].screenshot);
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(40,320,350,20), saveInfo[windowID].dateTime, "plaintext");
		GUI.Label(new Rect(40,380,350,20), ConvertPlayTime(saveInfo[windowID].playTime), "plaintext");
		GUI.Label(new Rect(40,440,350,20), difficultyStr, "plaintext");
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
	}	

	//get actual playing time from float value
	private string ConvertPlayTime(float playTime){
		TimeSpan t = TimeSpan.FromSeconds(playTime);
		return string.Format("{0:D2}:{1:D2}.{2:D2}", t.Hours, t.Minutes, t.Seconds);
	}
}
