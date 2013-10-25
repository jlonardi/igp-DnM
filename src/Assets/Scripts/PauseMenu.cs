using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GUISkin skin;
	public bool paused=false; //tells if game is paused
	private float screen_width=Screen.width;
	private float screen_height=Screen.height; //putting screen size here to optimaze code
	
	void Update(){
		
		if(paused){                       //when game is paused time stops and the cursour shows
			Time.timeScale=0;
			Screen.showCursor=true;
			Screen.lockCursor=false;
		}
		else{
			Time.timeScale=1;            //when game is not paused time runs normally and the cursor is hidden and lockt
			Screen.showCursor=false;
			Screen.lockCursor=true;
		}
		if(Input.GetKeyDown(KeyCode.Escape)){  //to get pause on and off pres ESC
			paused =!paused;

		}
	
	
	}
	void OnGUI()
	{
		if (paused)
		{
			PauseScreen();
		}
	}
	void PauseScreen()
	{
		GUILayout.BeginArea(new Rect((screen_width *0.5f)-50, (Screen.height*0.5f)-50,100,200));
		
		if(GUILayout.Button ("Resume"))
		{
			paused =!paused; //stop pause to resume game
		}
		
		if(GUILayout.Button ("Main Menu"))
		{
			Application.LoadLevel("Main Meny"); //load main meny level
		}
		if(GUILayout.Button ("Quit game"))
		{
			Application.Quit();  //quit game
		}
		
		GUILayout.EndArea ();
	}
	
}
