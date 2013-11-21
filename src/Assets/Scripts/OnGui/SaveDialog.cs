using UnityEngine;
using System.Collections;

public class SaveDialog {
	private OnGuiManager gui;
	private int nativeWidth;
	private int nativeHeight;
	private int padWidth;
	
	public void Initialize(){
		gui = OnGuiManager.instance;
		nativeWidth = gui.nativeWidth;
		nativeHeight = gui.nativeHeight;
		padWidth = gui.padWidth;
	}

	// Show() gets called from OnGuiManager
	public void Show(){
		if (gui == null){
			Initialize();
		}

		string saveName;
		if (SaveManager.instance.container.name != null){
			saveName = SaveManager.instance.container.name;
		} else {
			saveName = "Savegame " + (SaveManager.instance.container.saveSlot + 1);
		}

		GUI.Box(new Rect(((nativeWidth+padWidth) *0.5f)-125, (nativeHeight*0.5f)-25,250,100), "Give the name of your game");
		SaveManager.instance.container.name = GUI.TextArea (new Rect (((nativeWidth+padWidth) *0.5f)-100, (nativeHeight*0.5f), 200, 20),
		                                                    saveName, 20);
		
		if (GUI.Button(new Rect (((nativeWidth+padWidth) *0.5f)-50, (nativeHeight*0.5f)+25, 100, 20),"save game"))
		{			
			SaveManager.instance.Save();
		}
	}
}
