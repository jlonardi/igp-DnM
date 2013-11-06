using UnityEngine;
using System.Collections;

public class gameOverMenu : MonoBehaviour {
	
	private float screen_width=Screen.width;
	private float screen_height=Screen.height; //putting screen size here to optimaze code
	void Update(){
		if(!GameManager.instance.gameRunning){
			Time.timeScale=0;
			Screen.showCursor=true;
			Screen.lockCursor=false;
		}
	}
	
	void OnGUI() {
		if (!GameManager.instance.gameRunning)
		{

			this.gameOverScreen();
		}
	}
	
	private void gameOverScreen(){
		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize=30;
		GUI.Box(new Rect((screen_width *0.5f)-138, (Screen.height*0.5f)-100,275,250),"Game Over", myStyle);
		GUI.Label(new Rect((screen_width *0.5f)-100, (Screen.height*0.5f)-150,275,250), "Health: " + PlayerHealth.instance.health);
		GUI.Label(new Rect((screen_width *0.5f)-100, (Screen.height*0.5f)-125,275,250), "Treasure: " + Treasure.instance.amount);
		GUI.Label (new Rect((screen_width *0.5f)-100, (Screen.height*0.5f)-100,275,250), "Body Count " + GameManager.instance.bodyCount);
		GUILayout.BeginArea(new Rect((screen_width *0.5f)-50, (Screen.height*0.5f)+100,100,200));
		if(GUILayout.Button ("Main Menu"))
		{
			Application.LoadLevel("Main Meny"); //load main menu level
		}
		GUILayout.EndArea ();
	
	}
	
	
}
