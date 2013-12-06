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
		int pictureIndex = -1;
		
		GUI.Box(new Rect(centerX-600, 25,1200,1000),"", "window");

		//if already at last slide, don't check the slide counter
		switch (storySlide){
		case 1:
			pictureIndex = 1;
			storyHead = "Old Man's House";
			storyText = "Player hears from the elderly man that there is a hidden valley up high in the mountains " +
				"and that there's a treasure with significant amount of gold and riches.";
			break;
		case 2:
			pictureIndex = 1;
			storyHead = "Old Man's House";
			storyText = "Old man also warns about the mean and old dragon which has kept the treasure safe through centuries.";
			break;
		case 3:
			pictureIndex = 2;
			storyHead = "The Journey Begins";
			storyText = "As our player is afraid of no dragon, he packs up and heads up in to the mountains. " +
				"Finally, he finds the hidden valley by using the precise instructions given by the old man.";
			break;
		case 4:
			pictureIndex = 2;
			storyHead = "The Journey Begins";
			storyText =  "But alas, as he runs into the lizard guardians he quickly realizes that this isn't just any " +
				"walk in the park.";
			break;
		case 5:
			pictureIndex = 2;
			storyHead = "The Journey Begins";
			storyText =  "Luckily though, our player was prepared for the journey by packing his trustworthy Beretta pistol " +
				"and M4 assault rifle and with those he could take the guardians out with ease.";
			break;
		case 6:
			pictureIndex = 3;
			storyHead = "Dragon's Lair";
			storyText = "Old dragons do need to sleep at times so the player waits for his moment and steals the treasure while the dragon " + 
				"is taking it's daily nap.";
			break;
		case 7:
			pictureIndex = 3;
			storyHead = "Dragon's Lair";
			storyText = "This should have been simple enough, but as the player stumbles on some bones on the ground, the dragon wakes up. " +
				"Player quickly grabs the treasure and makes a run for it.";
			break;
		case 8:
			pictureIndex = 4;
			storyHead = "Orc's Get Involved";
			storyText = "epic story here";
			break;
		case 9:
			pictureIndex = 4;
			storyHead = "Orc's Get Involved";
			storyText = "epic story here";
			break;
		case 10:
			pictureIndex = 5;
			storyHead = "Trapped";
			storyText = "epic story here";
			break;
		case 11:
			pictureIndex = 5;
			storyHead = "Trapped";
			storyText = "epic story here";
			break;
		case 12:
			pictureIndex = 5;
			storyHead = "Loading Game";
			storyText = "Please wait..";
			break;
		}

		TellStory(pictureIndex,	storyHead, storyText);

		GUILayout.BeginArea(new Rect(gui.GetWidth()-200, 1000,100,200));
		if (storySlide<11 && GameManager.instance.levelState != LevelState.LOADING_NEWGAME){
			if(GUILayout.Button ("Next"))
			{
				storySlide++;
			}
		} else if(storySlide == 11){
			if(GUILayout.Button ("Start")) {
				storySlide++;
				//loads first level
				GameManager.instance.levelState = LevelState.LOADING_NEWGAME;
				Application.LoadLevel("GameLevel");
			}
		}
		GUILayout.EndArea();	
	}

	private void TellStory(int pictureIndex, string head, string text){
		Rect imagePosition = new Rect(centerX-480, 225, 960, 540);
		Rect textPosition = new Rect(centerX-500, 780, 1030, 300);

		GUI.DrawTexture(imagePosition, storyImages[pictureIndex-1]);

		GUILayout.BeginArea(new Rect(centerX-220, 135, 440, 600));
		GUILayout.Label("- " + head + " - ", "textfield");
		GUILayout.EndArea();
		GUILayout.BeginArea(textPosition);
		GUILayout.Label(text, "storytext");
		GUILayout.EndArea();
	}
	
}
