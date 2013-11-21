using UnityEngine;
using System.Collections;

public class GameOverScreen {
	private GameManager game;
	private OnGuiManager gui;
	private int nativeWidth;
	private int nativeHeight;
	private int padWidth;

	public void Initialize(){
		game = GameManager.instance;
		gui = OnGuiManager.instance;
		nativeWidth = gui.nativeWidth;
		nativeHeight = gui.nativeHeight;
		padWidth = gui.padWidth;
	}

	// Show() gets called from OnGuiManager
	public void Show(){
		if (game == null){
			Initialize();
		}

		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize=30;
		GUI.Box(new Rect(((nativeWidth+padWidth) *0.5f)-138, (nativeHeight*0.5f)-100,275,250),"Game Over", myStyle);
		GUI.Label(new Rect(((nativeWidth+padWidth) *0.5f)-100, (nativeHeight*0.5f)-150,275,250), "Health: " + game.statistics.playerHealth);
		GUI.Label(new Rect(((nativeWidth+padWidth) *0.5f)-100, (nativeHeight*0.5f)-125,275,250), "Treasure: " + game.statistics.treasureAmount);
		GUI.Label (new Rect(((nativeWidth+padWidth) *0.5f)-100, (nativeHeight*0.5f)-100,275,250), "Body Count: " + game.statistics.bodycount);
		GUI.Label (new Rect(((nativeWidth+padWidth) *0.5f)-100, (nativeHeight*0.5f)-75,275,250), "Score: " + game.statistics.score);
		GUILayout.BeginArea(new Rect(((nativeWidth+padWidth) *0.5f)-50, (nativeHeight*0.5f)+100,100,200));
		if(GUILayout.Button ("Main Menu"))
		{
			// load Main Menu scene
			Application.LoadLevel("Main Meny");
		}
		GUILayout.EndArea();	
	}
	
	
}
