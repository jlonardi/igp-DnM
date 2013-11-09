using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	
	private SaveSerializer serializer;
  	public GUISkin guiSkin; //public GUISkin skin
	
	private delegate void GUIMethod(); //takes care of desiding what menu screen i shown
    private GUIMethod currentGUIMethod;
	
	private float screen_width=Screen.width;
	private float screen_height=Screen.height; //putting screen size here to optimaze code
	
	private string save_name="Name your game"; //name of the saved game
	private int maxSaveSlots = 5; //max amount of games saved
	private int currentSaveSlot = 0;
	private int skipUpdateFrames = 0;
	
	void Awake(){
		serializer = new SaveSerializer();		
	}
	
	void Update(){
		//workaround for relaxing recently instantiated ragdolls for load game
		SkipUpdateFrames();
		
		if(GameManager.instance.paused){	//when game is paused time stops and the cursour shows
			Time.timeScale=0;
			Screen.showCursor=true;
			Screen.lockCursor=false;
		}
		else if (GameManager.instance.gameRunning){
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
		// if savegame selected, return will save
		if (this.currentGUIMethod == SaveGame && Event.current.keyCode == KeyCode.Return)
		{
			serializer.Save(currentSaveSlot, save_name);
			this.currentGUIMethod=PauseScreen;
   		}
		if (GameManager.instance.paused && GameManager.instance.gameRunning)
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
	
	// LoadScreen() takes care of loading the game
	void LoadScreen()
	{
		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize=30;
		GUI.Box(new Rect((screen_width *0.5f)-138, (Screen.height*0.5f)-100,275,250),"Load Game", myStyle);
		
		GUILayout.BeginArea(new Rect((screen_width *0.5f)-125, (screen_height*0.5f)-50,250,200));
		for (int i = 1; i <= maxSaveSlots; i++){
			string saveSlotText= serializer.GetSavename(i);
			if (saveSlotText.Equals("Empty")){
				if (GUILayout.Button("Empty")){
					Debug.Log("Empty save");
				}
			}
			if (!saveSlotText.Equals("Empty")){
				if(GUILayout.Button(saveSlotText))
				{
					serializer.Load(i);
					this.currentGUIMethod=PauseScreen;
					
					//workaround for relaxing recently instantiated ragdolls for load game
					Time.timeScale = 1;	//allow TimeScale = 1 for next two frames
					skipUpdateFrames = 2;	
				}
			}
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
		
		GUI.Box(new Rect((screen_width *0.5f)-138, (Screen.height*0.5f)-100,275,250), "Save Game", myStyle);
				GUILayout.BeginArea(new Rect((screen_width *0.5f)-125, (screen_height*0.5f)-50,250,200));

		for (int i = 1; i <= maxSaveSlots; i++){
			string saveSlotText= serializer.GetSavename(i);
			if(GUILayout.Button(saveSlotText))
			{				
				currentSaveSlot = i;
				this.currentGUIMethod = SaveGame;
			}
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
			serializer.Save(currentSaveSlot, save_name);
			this.currentGUIMethod=PauseScreen;
		}
	}
	
	
	//workaround for relaxing recently instantiated ragdolls for load game
	private void SkipUpdateFrames(){
		if (skipUpdateFrames>1){
			skipUpdateFrames--;
		} else if (skipUpdateFrames==1){
			foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]){
	   			if(go.name.Equals("orc ragdoll(Clone)")){	
					go.Serializable().disableKinematic(go.transform);
				}
			}
			skipUpdateFrames = 0;
			Time.timeScale = 0;
		}		
	}
	
/*	//Get saveslot text with formatted date and time
	private string getSaveSlotText(LevelSerializer.SaveEntry se){	
		return 	se.Name + "  (" +
				string.Format("{0:00}", se.When.Day) + "." +
				string.Format("{0:00}", se.When.Month) + "." +
				se.When.Year + ", " +
				string.Format("{0:00}", se.When.Hour) + ":" + 
				string.Format("{0:00}", se.When.Minute) + ")";
	}*/
}
