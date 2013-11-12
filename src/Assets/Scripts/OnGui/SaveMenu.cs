using UnityEngine;
using System;
using System.Collections;

public class SaveMenu {

	// this get's called from game manager when GameState.SAVE_MENU
	public void Show(){
		Texture2D[] screenshot = new Texture2D[SaveManager.instance.maxSaveSlots];
		string[] name = new string[SaveManager.instance.maxSaveSlots];
		DateTime[] date = new DateTime[SaveManager.instance.maxSaveSlots];

		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize=30;
		
		GUI.Box(new Rect((Screen.width *0.5f)-138, (Screen.height*0.5f)-100,275,250), "Save Game", myStyle);
				GUILayout.BeginArea(new Rect((Screen.width *0.5f)-125, (Screen.height*0.5f)-50,250,200));

		for (int i = 1; i <= SaveManager.instance.maxSaveSlots; i++){
			SaveInfo saveInfo = SaveManager.instance.GetSaveInfo(i, ref screenshot[i-1], ref date[i-1]);
			if(GUILayout.Button(saveInfo.name))
			{				
				GameManager.instance.gameState = GameState.SAVE_SCREENSHOT;
				// take screenshot now as it takes additional frame to complete
				SaveManager.instance.GrabScreenShot();
				
				SaveManager.instance.container.saveSlot = i;
				SaveManager.instance.container.name = saveInfo.name;
			}
		}					        
		if(GUILayout.Button ("Return"))	{
			GameManager.instance.gameState = GameState.PAUSE_MENU;
		}
		GUILayout.EndArea();
		
	}
}
