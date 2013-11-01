using UnityEngine;
using System.Collections;

public class LoadLevelMenu : MonoBehaviour {
	
	public bool showLoadLevelMenu;
	
	private float screen_width=Screen.width;
	private float screen_height=Screen.height; //putting screen size here to optimaze code
	
	public Texture background_texture;

	
	void OnGUI(){
		if (showLoadLevelMenu)
		{
			GUI.DrawTexture(new Rect(0,0,screen_width,screen_height), background_texture);
			LoadScreen ();
		}
	}
	
	// LoadScreen() takes care of loadning the game
	void LoadScreen()
	{
		int i=0;
		GUILayout.BeginArea(new Rect((screen_width *0.5f)-125, (screen_height*0.5f)-50,250,200));
		foreach(var sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName]) 
		{
			string saveSlotText=sg.Name+" " + sg.When.Day +"."+ sg.When.Month +"."+sg.When.Year+"  "+sg.When.Hour+":" +sg.When.Minute;
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
			showLoadLevelMenu=false;
		}
		GUILayout.EndArea ();			
	}

}
