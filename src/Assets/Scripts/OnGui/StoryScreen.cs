using UnityEngine;
using System.Collections;

[System.Serializable]
public class StoryScreen {
	private GameManager game;
	private OnGuiManager gui;
	public Texture2D[] storyImages;

	private int centerX;
	private int centerY;
	private int storySlide = 1;
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

		GUIStyle myStyle = new GUIStyle("Box");
		myStyle.fontSize=30;
		switch (storySlide){
		case 1:
			GUI.Box(new Rect(centerX-138, centerY-100,275,250),"Slide 1", myStyle);
			GUI.Label(new Rect(centerX-100, centerY-150,275,250), "nothing yet");
			break;
		case 2:
			GUI.Box(new Rect(centerX-138, centerY-100,275,250),"Slide 2", myStyle);
			GUI.Label(new Rect(centerX-100, centerY-150,275,250), "nothing yet");
			break;
		case 3:
			GUI.Box(new Rect(centerX-138, centerY-100,275,250),"Slide 3", myStyle);
			GUI.Label(new Rect(centerX-100, centerY-150,275,250), "nothing yet");
			break;
		case 4:
			GUI.Box(new Rect(centerX-138, centerY-100,275,250),"Slide 4", myStyle);
			GUI.Label(new Rect(centerX-100, centerY-150,275,250), "nothing yet");
			break;
		case 5:
			GUI.Box(new Rect(centerX-138, centerY-100,275,250),"Slide 5", myStyle);
			GUI.Label(new Rect(centerX-100, centerY-150,275,250), "nothing yet");
			break;
		}
		imagePosition = new Rect(0, 0, 1920, 1080);
		GUI.DrawTexture(imagePosition, storyImages[storySlide-1]);

		GUILayout.BeginArea(new Rect(gui.GetWidth()-200, 1000,100,200));
		if (storySlide<5){
			if(GUILayout.Button ("Next"))
			{
				storySlide++;
			}
		} else if(GUILayout.Button ("Start game")) {
			storySlide=1;
			//loads first level
			GameManager.instance.levelState = LevelState.LOADING_NEWGAME;
			Application.LoadLevel("GameLevel");
		}
		GUILayout.EndArea();	
	}
	
	
}
