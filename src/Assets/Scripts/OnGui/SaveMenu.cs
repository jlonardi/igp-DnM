using UnityEngine;


public class SaveMenu {
	private SaveInfo[] saveInfo;
	private OnGuiManager gui;
	private int centerX;
	private int centerY;

	public void Initialize(){
		gui = OnGuiManager.instance;
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
		GUI.Box(new Rect(centerX-400, 100,800,850),"", "window");
		
		GUILayout.BeginArea(new Rect(centerX-220, 210,440,600));
		GUILayout.Label("Select Save Slot", "textfield");
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(centerX-175, 330,350,600));

		for (int i = 0; i < SaveManager.instance.maxSaveSlots; i++){
			string saveName;
			if (saveInfo[i].name != null){
				saveName = saveInfo[i].name;
			} else {
				saveName = "Empty";
			}

			if(GUILayout.Button(saveName))
			{				
				GameManager.instance.gameState = GameState.SAVE_SCREENSHOT;
				// take screenshot now as it takes additional frame to complete
				SaveManager.instance.GrabScreenShot();
				
				SaveManager.instance.container.saveSlot = i;
				SaveManager.instance.container.name = saveInfo[i].name;
			}
		}					

		GUILayout.Space(40);

		if(GUILayout.Button ("Back"))	{
			GameManager.instance.gameState = GameState.PAUSE_MENU;
		}
		GUILayout.EndArea();
		
	}
}
