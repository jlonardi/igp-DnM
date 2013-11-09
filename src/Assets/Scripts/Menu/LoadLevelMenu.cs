using UnityEngine;
using System.Collections;

public class LoadLevelMenu : MonoBehaviour {
	
	public bool showLoadLevelMenu;
	private float screen_width=Screen.width;
	private float screen_height=Screen.height; //putting screen size here to optimaze code
	private SaveSerializer serializer;
	
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
		serializer = new SaveSerializer();
		GUILayout.BeginArea(new Rect((screen_width *0.5f)-125, (screen_height*0.5f)-50,250,200));
		
		for (int i = 1; i <= 5; i++){
			string saveSlotText= serializer.GetSavename(i);
			if (saveSlotText.Equals("Empty")){
				if (GUILayout.Button("Empty")){
					Debug.Log("Empty save");
				}
			}
			if (!saveSlotText.Equals("Empty")){
				if(GUILayout.Button(saveSlotText))
				{
//					serializer.Load(i);
					
					//workaround for relaxing recently instantiated ragdolls for load game
//					Time.timeScale = 1;	//allow TimeScale = 1 for next two frames
//					skipUpdateFrames = 2;	
				}
			}
		}			
		
		if(GUILayout.Button ("Return"))
		{
			showLoadLevelMenu=false;
		}
		GUILayout.EndArea ();			
	}

}
