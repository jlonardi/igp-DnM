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

		GUI.skin.GetStyle("window");
		GUI.Box(new Rect(centerX-400, 100,800,850),"", "window");
		
		GUILayout.BeginArea(new Rect(centerX-220, 210,440,600));
		GUILayout.Label("Name the save slot", "textfield");
		GUILayout.EndArea();
		
		//if player hits Enter inside textarea, save game
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return) {
			SaveManager.instance.Save();
		}

		GUILayout.BeginArea(new Rect(centerX-175, 400,350,600));
		SaveManager.instance.container.name = GUILayout.TextArea(saveName, 20);

		GUILayout.Space(50);

		//if player hits Save-button, save game
		if (GUILayout.Button("Save")){
			SaveManager.instance.Save();
		}

		GUILayout.Space(150);

		//if player hits Back-button, return to previous menu
		if (GUILayout.Button("Back"))
		{			
			GameManager.instance.gameState = GameState.SAVE_MENU;
		}
		GUILayout.EndArea();
	}
}
