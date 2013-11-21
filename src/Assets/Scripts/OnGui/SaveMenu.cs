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

		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize=30;
		
		GUI.Box(new Rect(centerX-138, centerY-100,275,250), "Save Game", myStyle);
		GUILayout.BeginArea(new Rect(centerX-125, centerY-50,250,200));

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
		if(GUILayout.Button ("Return"))	{
			GameManager.instance.gameState = GameState.PAUSE_MENU;
		}
		GUILayout.EndArea();
		
	}
}
