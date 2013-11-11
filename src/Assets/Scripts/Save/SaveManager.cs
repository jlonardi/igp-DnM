using UnityEngine;
using System;
using System.Collections;

public class SaveManager : MonoBehaviour {
	//use singleton since only we need once instance of this class
	public static SaveManager instance;

	[HideInInspector]	
	public SaveContainer container = new SaveContainer();
	
    public void Awake()
    {
		// never destroy Save Manager on scene load
		DontDestroyOnLoad (this);	
        SaveManager.instance = this;
    }

	private SaveSerializer serializer = new SaveSerializer();
	
	private int skipUpdateFrames = 3;
	public int maxSaveSlots = 5; //max amount of games saved
		
	void Update(){
		//workaround for relaxing recently instantiated ragdolls for load game
		SkipUpdateFrames();
	}		

	public SaveInfo GetSaveInfo(int saveSlot, ref Texture2D screenshot, ref DateTime dateTime){
		return serializer.GetSaveInfo(saveSlot, ref screenshot, ref dateTime);
	}
	
	
	public void Save(){
		container.SaveValues();
		serializer.Save(SaveManager.instance.container.saveSlot);
		GameManager.instance.gameState = GameState.PAUSE_MENU;
	}
	
	public void Load(){		
		serializer.Load(SaveManager.instance.container.saveSlot);
		
		// allow 3 frames to be skipped after save to fix ragdoll bug
		skipUpdateFrames = 3;
		Time.timeScale = 1;
		GameManager.instance.gameState = GameState.RUNNING;	
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
	
	
	//workaround for relaxing recently instantiated ragdolls for load game
	private bool SkipUpdateFrames(){
		if (skipUpdateFrames==0){
			return false;
		}
		
		if (skipUpdateFrames>1){
			skipUpdateFrames--;
			return true;
		} 

		foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]){
	   		if(go.name.Equals("orc ragdoll(Clone)")){	
				go.Serializable().disableKinematic(go.transform);
			}
		}
		
		skipUpdateFrames = 0;
		Time.timeScale = 0;
		return false;
	}
	
	public void GrabScreenShot(){
		StartCoroutine(rdp());
	}		
	
	IEnumerator rdp() {
		Texture2D tex = new Texture2D(Screen.width, Screen.height);
		yield return new WaitForEndOfFrame();
		tex.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0);
    	yield return null;
		tex.Apply();
		container.SaveTexture(tex);
    	//var bytes = tex.EncodeToPNG();
    	Destroy (tex);
    }
	
}
