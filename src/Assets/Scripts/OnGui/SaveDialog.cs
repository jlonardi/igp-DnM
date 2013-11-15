using UnityEngine;
using System.Collections;

public class SaveDialog {	
		
	// Show() gets called from OnGuiManager
	public void Show(){
		string saveName;
		if (SaveManager.instance.container.name != null){
			saveName = SaveManager.instance.container.name;
		} else {
			saveName = "Savegame " + (SaveManager.instance.container.saveSlot + 1);
		}

		GUI.Box(new Rect((Screen.width *0.5f)-125, (Screen.height*0.5f)-25,250,100), "Give the name of your game");
		SaveManager.instance.container.name = GUI.TextArea (new Rect ((Screen.width *0.5f)-100, (Screen.height*0.5f), 200, 20),
		                                                    saveName, 20);
		
		if (GUI.Button(new Rect ((Screen.width *0.5f)-50, (Screen.height*0.5f)+25, 100, 20),"save game"))
		{			
			SaveManager.instance.Save();
		}
	}
}
