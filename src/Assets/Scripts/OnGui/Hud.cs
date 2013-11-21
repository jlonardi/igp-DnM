using UnityEngine;
using System.Collections;

[System.Serializable]
public class Hud {
	public Texture2D healthbar;
	public Texture2D health;	

	private Rect helpPosition;

	private Rect healthPosition;
	private float currentHealth;

	private GameManager game;
	private OnGuiManager gui;
	private int nativeWidth;
	private int nativeHeight;
	
	public void Initialize(){
		game = GameManager.instance;
		gui = OnGuiManager.instance;
		nativeWidth = gui.nativeWidth;
		nativeHeight = gui.nativeHeight;

		helpPosition = CalculateGUIRect(500, 40, 0, -40);
		//healthbar position and size
		healthPosition = new Rect(10,200,193,34);
	}

	// Show() gets called from OnGuiManager
	public void Show() {
		if (game == null){
			Initialize();
		}

		if (GameManager.instance.treasureState == TreasureState.CARRYING){
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label(helpPosition, "Press 'E' to drop the treasure");
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		}
			
		GUI.DrawTexture(healthPosition, healthbar);
		currentHealth = game.statistics.playerHealth;
		currentHealth = currentHealth*1.76f;
		GUI.BeginGroup(new Rect(18,207,currentHealth,20)); 
		GUI.DrawTexture(new Rect(0,0,176,20), health);
		GUI.EndGroup();


		GUI.Box(new Rect(5,5,105,125),"");	
		GUI.Label(new Rect(10,10,100,20), "Health: " + game.statistics.playerHealth);
		GUI.Label(new Rect(10,25,100,20), "Treasure: " + game.statistics.treasureAmount);
		GUI.Label (new Rect(10,40,100,20),"Score: " + game.statistics.score);
		
		if (game.statistics.gunEnabled){
			GUI.Label(new Rect(10,55,100,20), "Grenades: " + game.statistics.grenadeCount);
			GUI.Label(new Rect(10,70,100,20), "Gun: " + game.statistics.gunName);			
			GUI.Label(new Rect(10,85,100,20), "Ammo: " + game.statistics.gunRounds);
			string clips;
			if (game.statistics.gunUnlimitedClips){
				clips = "unlimited";
			} else {
				clips = game.statistics.gunClips.ToString();
			}
			GUI.Label(new Rect(10,100,100,20), "Clips: " + clips);
			if (game.statistics.gunReloading){
				GUI.Label(new Rect(10,115,100,20), "Reloading...");
			}
		}

	}	
	
	
	public Rect CalculateGUIRect(int width, int height){
		return CalculateGUIRect(width, height, 0, 0);
	}
	
	public Rect CalculateGUIRect(int width, int height, int xOffset, int yOffset){
		return new Rect((nativeWidth-width)/2 + xOffset, (nativeHeight-height)/2 + yOffset, width, height);
	}
}
