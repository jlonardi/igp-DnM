using UnityEngine;


public class SaveMenu {
	private SaveInfo[] saveInfo;
	private OnGuiManager gui;
	private int nativeWidth;
	private int nativeHeight;
	private int padWidth;

	public void Initialize(){
		gui = OnGuiManager.instance;
		nativeWidth = gui.nativeWidth;
		nativeHeight = gui.nativeHeight;
		padWidth = gui.padWidth;
		saveInfo = SaveManager.instance.saveInfo;
	}

	// Show() gets called from OnGuiManager
	public void Show(){
		if (gui == null){
			Initialize();
		}

		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize=30;
		
		GUI.Box(new Rect(((nativeWidth+padWidth) *0.5f)-138, (nativeHeight*0.5f)-100,275,250), "Save Game", myStyle);
		GUILayout.BeginArea(new Rect(((nativeWidth+padWidth) *0.5f)-125, (nativeHeight*0.5f)-50,250,200));

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
