using UnityEngine;
using System.Collections;

public class SaveDialog {
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

		string saveName;
		if (SaveManager.instance.container.name != null){
			saveName = SaveManager.instance.container.name;
		} else {
			saveName = "Savegame " + (SaveManager.instance.container.saveSlot + 1);
		}

		GUI.Box(new Rect(centerX-125, centerY-25,250,100), "Give the name of your game");
		SaveManager.instance.container.name = GUI.TextArea (new Rect (centerX-100, centerY, 200, 20),
		                                                    saveName, 20);
		
		if (GUI.Button(new Rect (centerX-50, centerY+25, 100, 20),"save game"))
		{			
			SaveManager.instance.Save();
		}
	}
}
