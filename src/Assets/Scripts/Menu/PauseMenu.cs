using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	
	
  	public GUISkin guiSkin; //public GUISkin skin
	
	private delegate void GUIMethod(); //takes care of desiding what menu screen i shown
    private GUIMethod currentGUIMethod;
	
	public bool paused=false; //tells if game is paused
	
	private float screen_width=Screen.width;
	private float screen_height=Screen.height; //putting screen size here to optimaze code
	
	private string save_name="Dragons & Miniguns"; //name of the saved game
	private int max_saved_games =5; //max amount of games saved
	

	void Awake()
	{
		LevelSerializer.MaxGames = max_saved_games;
		
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
			this.currentGUIMethod=PauseScreen;
			
		}
	
	
	}
	//OnGUI calls the right GUI screen metod when the game is paused
	void OnGUI()
	{
		if (paused)
		{
			this.currentGUIMethod();
		}
	}

	//PauseScreen() is the main pause screen
	void PauseScreen()
	{
		GUILayout.BeginArea(new Rect((screen_width *0.5f)-50, (Screen.height*0.5f)-50,100,200));
		
		if(GUILayout.Button ("Resume"))
		{
			paused =!paused; //stop pause to resume game
		}
		
		if(GUILayout.Button ("Main Menu"))
		{
			Application.LoadLevel("Main Meny"); //load main menu level
		}
		if(GUILayout.Button ("Save game"))
		{
			this.currentGUIMethod=SaveGameScreen;	//opens the save game screen
		}
		if(GUILayout.Button ("Load level"))
		{
			this.currentGUIMethod=LoadScreen; //opens load level screen
		}
		  
		if(GUILayout.Button ("Quit game"))
		{
			Application.Quit();  //quit game
		}
		
		GUILayout.EndArea ();
	}
	
	// LoadScreen() takes care of loadning the game
	void LoadScreen()
	{
		int i=0;
		GUILayout.BeginArea(new Rect((screen_width *0.5f)-125, (Screen.height*0.5f)-50,250,200));
		foreach(var sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName]) 
		{
			string saveSlotText=sg.Name+" " + sg.When.Day +"."+ sg.When.Month +"  "+sg.When.Hour+":" +sg.When.Minute;
			if(GUILayout.Button(saveSlotText))
			{
				sg.Load();
				Time.timeScale = 1;
				    
			}
			i++;
        }
		while(i<5)
		{
			GUILayout.Button ("Empty");
			i++;
		}
		
		if(GUILayout.Button ("Return"))
		{
			this.currentGUIMethod=PauseScreen;
		}
		GUILayout.EndArea ();			
	}
	
	// SaveGame() takes care of saving the game
	void SaveGameScreen()
	{
		int i=0;
		GUI.Box(new Rect((screen_width *0.5f)-138, (Screen.height*0.5f)-100,275,250), "Choose where to save your game");
				GUILayout.BeginArea(new Rect((screen_width *0.5f)-125, (Screen.height*0.5f)-50,250,200));
		foreach(var sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName]) 
		{
			string saveSlotText=sg.Name+" " + sg.When.Day +"."+ sg.When.Month +"  "+sg.When.Hour+":" +sg.When.Minute;
			if(GUILayout.Button(saveSlotText))
			{
				sg.Delete();	
				this.currentGUIMethod= SaveGame;
				break;
			}
			i++;
				
			
		}
		while(i<5)
		{
			if(GUILayout.Button ("Empty"))
			{
				this.currentGUIMethod= SaveGame;
			}
			
			i++;
		}
        
		if(GUILayout.Button ("Return"))
		{
			this.currentGUIMethod=PauseScreen;
		}
		
		GUILayout.EndArea ();	

	}
	
	
	//Save Game ask for the name of the new game and saves the game
	void SaveGame()
	{
		GUI.Box(new Rect((screen_width *0.5f)-100, (Screen.height*0.5f)-25,200,100), "Give the name of your game");
		save_name = GUI.TextArea (new Rect ((screen_width *0.5f)-100, (Screen.height*0.5f), 200, 20), save_name, 20);
		
		if (GUI.Button(new Rect ((screen_width *0.5f)-50, (Screen.height*0.5f)+25, 100, 20),"save game"))
		{
			LevelSerializer.SaveGame(save_name);
			this.currentGUIMethod=PauseScreen;
		}
	}
	
}
