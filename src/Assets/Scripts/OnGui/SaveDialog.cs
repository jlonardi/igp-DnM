using UnityEngine;
using System.Collections;

public class SaveDialog {	
		
	// this get's called from game manager when GameState.SAVE_DIALOG
	public void Show(){		
		GUI.Box(new Rect((Screen.width *0.5f)-125, (Screen.height*0.5f)-25,250,100), "Give the name of your game");
		SaveManager.instance.container.name = GUI.TextArea (new Rect ((Screen.width *0.5f)-100, (Screen.height*0.5f), 200, 20),
																		SaveManager.instance.container.name, 20);
		
		if (GUI.Button(new Rect ((Screen.width *0.5f)-50, (Screen.height*0.5f)+25, 100, 20),"save game"))
		{			
			SaveManager.instance.Save();
		}
	}
}
