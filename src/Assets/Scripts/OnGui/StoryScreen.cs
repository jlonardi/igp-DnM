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
		
		GUI.Box(new Rect(centerX-600, 5,1200,1070),"", "window");

		//if already at last slide, don't check the slide counter
		switch (storySlide){
		case 1:
			pictureIndex = 1;
			storyHead = "Old Man's House";
			storyText = "Player learns from an elderly man that there is a hidden valley up high in the mountains, " +
				"and in that valley there's a treasure with a significant amount of gold and riches.";
			break;
		case 2:
			pictureIndex = 1;
			storyHead = "Old Man's House";
			storyText = "The old man also warns about the mean and old dragon which has kept the treasure safe through the centuries.";
			break;
		case 3:
			pictureIndex = 2;
			storyHead = "The Journey Begins";
			storyText = "As our player is afraid of no dragon, he packs up and heads up in to the mountains. " +
				"Finally, he finds his way into the hidden valley by using the old man's instructions.";
			break;
		case 4:
			pictureIndex = 2;
			storyHead = "The Journey Begins";
			storyText =  "But alas, as he runs into two lizard guardians, he quickly realizes that this isn't just a " +
				"walk in the park.";
			break;
		case 5:
			pictureIndex = 2;
			storyHead = "The Journey Begins";
			storyText =  "Luckily enough, our player prepared for the journey by packing his trustworthy Beretta pistol " +
				"and M4 assault rifle with him. With those, taking out the guardians was easy.";
			break;
		case 6:
			pictureIndex = 3;
			storyHead = "Dragon's Lair";
			storyText = "Old dragons do need to sleep at times, so the player waits for his moment and steals the treasure while the dragon " + 
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
			storyHead = "The Chase Begins";
			storyText = "As the dragon's roar echoes through the valley, orcs nearby figure out what's happening and " +
				"decide to seize the treasure from the player.";
			break;
		case 9:
			pictureIndex = 4;
			storyHead = "The Chase Begins";
			storyText = "... and they also want to impale the player's head on a stick so they can decorate their lovely home with it.";
			break;
		case 10:
			pictureIndex = 5;
			storyHead = "Trapped";
			storyText = "As the player heads towards the only exit from the valley, the orcs initiate their cunning plan by " + 
				"blocking the player's path with boulders and attacking the player.";
			break;
		case 11:
			pictureIndex = 5;
			storyHead = "Trapped";
			storyText = "Their suprise attack doesn't go all that well as the player is, after all, heavily armed. " +
				"But can the player hold back rest of the surfacing enemies?";
			break;
		case 12:
			pictureIndex = 5;
			storyHead = "Loading Game";
			storyText = "Please wait..";
			break;
		}

		TellStory(pictureIndex,	storyHead, storyText);

		if(GUI.Button(new Rect(centerX-500, 935, 300, 50), "Back to Main Menu"))
		{
			GameManager.instance.gameState = GameState.MAIN_MENU;
		}

		if (storySlide>1 && game.saves.levelState != LevelState.LOADING_NEWGAME){
			if(GUI.Button(new Rect(centerX+100, 935, 200, 50), "Previous"))
			{
				storySlide--;
			}
		}

		if (storySlide<11 && game.saves.levelState != LevelState.LOADING_NEWGAME){
			if(GUI.Button(new Rect(centerX+300, 935, 200, 50), "Next"))
			{
				storySlide++;
			}
		} else if(storySlide == 11){
			if(GUI.Button(new Rect(centerX+300, 935, 250, 50), "Start Game")){
				storySlide++;
				//loads first level
				game.saves.levelState = LevelState.LOADING_NEWGAME;
				Application.LoadLevel("GameLevel");
			}
		}
	}

	private void TellStory(int pictureIndex, string head, string text){
		Rect imagePosition = new Rect(centerX-400, 190, 800, 540);
		Rect textPosition = new Rect(centerX-500, 740, 1030, 300);

		GUI.DrawTexture(imagePosition, storyImages[pictureIndex-1]);

		GUILayout.BeginArea(new Rect(centerX-220, 105, 440, 600));
		GUILayout.Label("- " + head + " - ", "textfield");
		GUILayout.EndArea();
		GUILayout.BeginArea(textPosition);
		GUILayout.Label(text, "storytext");
		GUILayout.EndArea();
	}
	
}
