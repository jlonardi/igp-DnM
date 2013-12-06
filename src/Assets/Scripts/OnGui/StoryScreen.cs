using UnityEngine;
using System.Collections;

[System.Serializable]
public class StoryScreen {
	private GameManager game;
	private OnGuiManager gui;
	public Texture2D[] storyImages;

	private int centerX;
	private int centerY;
	[HideInInspector]
	public int storySlide = 1;
	private Rect imagePosition;

	public void Initialize(){
		game = GameManager.instance;
		gui = OnGuiManager.instance;
		centerX = gui.GetCenterX();
		centerY = gui.GetCenterY();

	}

	// Show() gets called from OnGuiManager
	public void Show(){
		if (game == null){
			Initialize();
		}

		imagePosition = new Rect(centerX-960, 0, 1920, 1080);
		
		//if already at last slide, don't check the slide counter
		if (GameManager.instance.levelState == LevelState.LOADING_NEWGAME){
			GUI.DrawTexture(imagePosition, storyImages[4]);
		} else {
			GUI.DrawTexture(imagePosition, storyImages[storySlide-1]);
			switch (storySlide){
			case 1:
				GUI.Box(new Rect(centerX-138, centerY-100,275,250),"Slide 1");
				GUI.Label(new Rect(centerX-100, centerY-150,275,250), "nothing yet");
				break;
			case 2:
				GUI.Box(new Rect(centerX-138, centerY-100,275,250),"Slide 2");
				GUI.Label(new Rect(centerX-100, centerY-150,275,250), "nothing yet");
				break;
			case 3:
				GUI.Box(new Rect(centerX-138, centerY-100,275,250),"Slide 3");
				GUI.Label(new Rect(centerX-100, centerY-150,275,250), "nothing yet");
				break;
			case 4:
				GUI.Box(new Rect(centerX-138, centerY-100,275,250),"Slide 4");
				GUI.Label(new Rect(centerX-100, centerY-150,275,250), "nothing yet");
				break;
			case 5:
				GUI.Box(new Rect(centerX-138, centerY-100,275,250),"Slide 5");
				GUI.Label(new Rect(centerX-100, centerY-150,275,250), "nothing yet");
				break;
			}
		}

		GUILayout.BeginArea(new Rect(gui.GetWidth()-200, 1000,100,200));
		if (storySlide<5 && GameManager.instance.levelState != LevelState.LOADING_NEWGAME){
			if(GUILayout.Button ("Next"))
			{
				storySlide++;
			}
		} else if(storySlide == 5){
			if(GUILayout.Button ("Start")) {
				//loads first level
				GameManager.instance.levelState = LevelState.LOADING_NEWGAME;
				Application.LoadLevel("GameLevel");
				storySlide = 1;
			}
		}
		GUILayout.EndArea();	
	}
	
	
}
