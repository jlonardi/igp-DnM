using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	
	
  	public GUISkin guiSkin; //public GUISkin skin
	
	private delegate void GUIMethod(); //takes care of desiding what menu screen i shown
    private GUIMethod currentGUIMethod;
	
//	public bool paused=false; //tells if game is paused
	
	private float screen_width=Screen.width;
	private float screen_height=Screen.height; //putting screen size here to optimaze code
	
	private string save_name="Name your game"; //name of the saved game
	private int max_saved_games =5; //max amount of games saved
	
	void Awake()
	{
		LevelSerializer.MaxGames = max_saved_games;
		
	}
	
	void Update(){
		
		if(GameManager.instance.paused){                       //when game is paused time stops and the cursour shows
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
			GameManager.instance.paused =!GameManager.instance.paused;
			this.currentGUIMethod=PauseScreen;
			
		}
	
	
	}
	//OnGUI calls the right GUI screen metod when the game is paused
	void OnGUI()
	{
		if (Event.current.keyCode == KeyCode.Return && this.currentGUIMethod==SaveGame) //in save game screen if enter is pressed game is saved
		{
    		LevelSerializer.SaveGame(save_name);
			this.currentGUIMethod=PauseScreen;
   		}
		if (GameManager.instance.paused)
		{
			this.currentGUIMethod();
		}
	}

	//PauseScreen() is the main pause screen
	void PauseScreen()
	{
		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize=30;
		GUI.Box(new Rect((screen_width *0.5f)-138, (Screen.height*0.5f)-100,275,250),"Game Paused", myStyle);
		
		GUILayout.BeginArea(new Rect((screen_width *0.5f)-50, (Screen.height*0.5f)-50,100,200));
		
		if(GUILayout.Button ("Resume"))
		{
			GameManager.instance.paused =!GameManager.instance.paused; //stop pause to resume game
		}
		
		if(GUILayout.Button ("Main Menu"))
		{
			Application.LoadLevel("Main Meny"); //load main menu level
		}
		if(GUILayout.Button ("Save game"))
		{
			this.currentGUIMethod=SaveGameScreen;	//opens the save game screen
		}
		if(GUILayout.Button ("Load game"))
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
		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize=30;
		GUI.Box(new Rect((screen_width *0.5f)-138, (Screen.height*0.5f)-100,275,250),"Load Game", myStyle);
		
		int i=0;
		GUILayout.BeginArea(new Rect((screen_width *0.5f)-125, (screen_height*0.5f)-50,250,200));
		foreach(var sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName]) 
		{
			string saveSlotText= getSaveSlotText(sg);
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
		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize=30;
		
		int i=0;
		GUI.Box(new Rect((screen_width *0.5f)-138, (Screen.height*0.5f)-100,275,250), "Save Game", myStyle);
				GUILayout.BeginArea(new Rect((screen_width *0.5f)-125, (screen_height*0.5f)-50,250,200));
		foreach(var sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName]) 
		{
			string saveSlotText= getSaveSlotText(sg);
			if(GUILayout.Button(saveSlotText))
			{
				save_name=sg.Name;
				sg.Delete();
				this.currentGUIMethod = SaveGame;
				break;
			}
			i++;
				
			
		}
		while(i<5)
		{
			if(GUILayout.Button ("Empty"))
			{
				save_name="Empty";
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
		GUI.Box(new Rect((screen_width *0.5f)-125, (screen_height*0.5f)-25,250,100), "Give the name of your game");
		save_name = GUI.TextArea (new Rect ((screen_width *0.5f)-100, (screen_height*0.5f), 200, 20), save_name, 20);
		
		if (GUI.Button(new Rect ((screen_width *0.5f)-50, (screen_height*0.5f)+25, 100, 20),"save game"))
		{
			LevelSerializer.SaveGame(save_name);
			this.currentGUIMethod=PauseScreen;
		}
	}
	
	//Get saveslot text with formatted date and time
	private string getSaveSlotText(LevelSerializer.SaveEntry se){	
		return 	se.Name + "  (" +
				string.Format("{0:00}", se.When.Day) + "." +
				string.Format("{0:00}", se.When.Month) + "." +
				se.When.Year + ", " +
				string.Format("{0:00}", se.When.Hour) + ":" + 
				string.Format("{0:00}", se.When.Minute) + ")";
	}
}
