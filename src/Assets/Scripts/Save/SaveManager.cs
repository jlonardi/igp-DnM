using UnityEngine;
using UnitySerialization;
using System;
using System.Collections;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour {
	//use singleton since only we need once instance of this class
	public static SaveManager instance;

	//savegame format version
	private static int formatVersion = 3;

	public LevelState levelState = LevelState.LOADED;
	// difficulty used to store new game difficulty + when showing highscore after game
	public DifficultySetting difficulty = DifficultySetting.NORMAL;

	[HideInInspector]
	public int maxSaveSlots = 5; //max amount of games saved
	public SaveInfo[] saveInfo;

	[HideInInspector]	
	public SaveContainer container = new SaveContainer();

	private SaveSerializer serializer = new SaveSerializer();
	
	private int skipUpdateFrames = 3;
		
	public void Awake()
	{
		// never destroy Save Manager on scene load
		DontDestroyOnLoad (this);

		SaveManager.instance = this;
		saveInfo = new SaveInfo[maxSaveSlots];
	}

	void Update(){
		//workaround for relaxing recently instantiated ragdolls for load game
		SkipUpdateFrames();
	}		

	// fill saveInfo array with headeredata from savegames
	public void GetSaveInfo(){
		for (int i=0; i<maxSaveSlots; i++){
			saveInfo[i] = serializer.GetSaveInfo(i);
		}
	}
	
	
	public void Save(){
		container.SaveValues();
		serializer.Save(container.saveSlot);
		GameManager.instance.gameState = GameState.PAUSE_MENU;
	}
	
	public void Load(){
		levelState = LevelState.LOADING_SAVE;

		// load container data
		serializer.Load(container.saveSlot);
		
		// allow 3 frames to be skipped after save to fix ragdoll bug
		skipUpdateFrames = 3;
		Time.timeScale = 1;
		//GameManager.instance.gameState = GameState.RUNNING;	
	}

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
				disableKinematic(go.transform);
			}
		}
		
		skipUpdateFrames = 0;
		Time.timeScale = 0;
		return false;
	}

	//routine to save screenshot
	public void SaveScreenshot(Texture2D source,int targetWidth,int targetHeight) {
		Texture2D result=new Texture2D(targetWidth,targetHeight, TextureFormat.RGB24 ,true);
		//Texture2D result=new Texture2D(targetWidth,targetHeight,source.format,true);
		Color[] rpixels=result.GetPixels(0);
    	float incX=(1.0f / (float)targetWidth);
    	float incY=(1.0f / (float)targetHeight);
		//scale texture into target width & height
    	for(int px=0; px<rpixels.Length; px++) {
        	rpixels[px] = source.GetPixelBilinear(incX*((float)px%targetWidth), incY*((float)Mathf.Floor(px/targetWidth)));
	    }
    	result.SetPixels(rpixels,0);
	    result.Apply();
    	container.screenshot = result.EncodeToPNG();
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

		//when screenshot has been taken, go to save dialog
		GameManager.instance.gameState = GameState.SAVE_DIALOG;

		SaveScreenshot(tex, 320, 180);
    	Destroy (tex);
    }
	
	//this should be on some generic class
	public void disableKinematic(Transform parent){			
		List<Transform> goTransforms = new List<Transform>();
		storeTransforms(parent, goTransforms);
		foreach(Transform t in goTransforms){
			if (t.rigidbody != null){						
				t.rigidbody.isKinematic = false;
			}
		}
	}

	// and this too..
	// this builds a Transform list of all transforms attached to the gameobject
	private void storeTransforms(Transform t_parent, List<Transform> list){
		if (t_parent==null){
			return;
		}
		
		list.Add(t_parent);
		foreach(Transform t in t_parent){
			storeTransforms(t,list);
		}
	}	

	public int GetFormatVersion(){
		return formatVersion;
	}
	
}
