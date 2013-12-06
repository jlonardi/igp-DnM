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

		string storyHead = "";
		string storyText = "";
		
		GUI.Box(new Rect(centerX-600, 25,1200,1000),"", "window");

		//if already at last slide, don't check the slide counter
		switch (storySlide){
		case 1:
			storyHead = "Old Man's House";
			storyText = "epic story here";
			break;
		case 2:
			storyHead = "The Journey Begins";
			storyText = "epic story here";
			break;
		case 3:
			storyHead = "Dragon's Lair";
			storyText = "epic story here";
			break;
		case 4:
			storyHead = "Orc's Get Involved";
			storyText = "epic story here";
			break;
		case 5:
			storyHead = "Trapped";
			storyText = "epic story here";
			break;
		}

		TellStory(storyHead, storyText);

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
			}
		}
		GUILayout.EndArea();	
	}

	private void TellStory(string head, string text){
		Rect imagePosition = new Rect(centerX-480, 250, 960, 540);
		Rect textPosition = new Rect(centerX-450, 820,300,600);

		GUI.DrawTexture(imagePosition, storyImages[storySlide-1]);
		GUILayout.BeginArea(new Rect(centerX-220, 135,440,600));
		GUILayout.Label("- " + head + " - ", "textfield");
		GUILayout.EndArea();
		GUILayout.BeginArea(textPosition);
		GUILayout.Label(text, "plaintext");
		GUILayout.EndArea();
	}
	
}
