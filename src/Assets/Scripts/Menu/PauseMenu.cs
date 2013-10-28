using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
//  	public GUISkin skin;
	public bool paused=false; //tells if game is paused
	private float screen_width=Screen.width;
	private float screen_height=Screen.height; //putting screen size here to optimaze code
	private string save_name="empy";
	private bool seeLoadMenu= false;

	
	void Awake()
	{
		LevelSerializer.MaxGames = 5;
	}
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
			if(seeLoadMenu)
			{
				LoadScreen ();
			}
			else{
				PauseScreen();
			}
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
		if(GUILayout.Button ("Save game"))
		{
			save_name="saved game";
			LevelSerializer.SaveGame(save_name);	
		}
		if(GUILayout.Button ("Load level"))
		{
			seeLoadMenu = !seeLoadMenu;
		}
		  
		if(GUILayout.Button ("Quit game"))
		{
			Application.Quit();  //quit game
		}
		
		GUILayout.EndArea ();
	}

	void LoadScreen()
	{
		GUILayout.BeginArea(new Rect((screen_width *0.5f)-50, (Screen.height*0.5f)-50,100,200));
		foreach(var sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName]) 
			{
				if(GUILayout.Button(sg.Name))
				{
					LevelSerializer.LoadSavedLevel(sg.Data);
					Time.timeScale = 1;
				}
			
        }
		if(GUILayout.Button ("Return"))
		{
			seeLoadMenu = !seeLoadMenu;
		}
		GUILayout.EndArea ();			
	}	
}
