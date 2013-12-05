using UnityEngine;
using System.Collections;

[System.Serializable]
public class Hud {
	public Texture2D healthbar;
	public Texture2D health;
	public Texture2D armorbar;
	public Texture2D armor;

	private Rect helpPosition;

	private Rect healthPosition;
	private Rect armorPosition;
	private float currentHealth;
	private float currentArmor;

	private GameManager game;
	private OnGuiManager gui;
	private int centerX;
	private int centerY;

	public void Initialize(){
		game = GameManager.instance;
		gui = OnGuiManager.instance;

		centerX = gui.GetCenterX();
		centerY = gui.GetCenterY();

		helpPosition = CalculateGUIRect(700, 40, 0, -80);
		//health & armor bar position and size
		healthPosition = new Rect(10,200,193,34);
		armorPosition = new Rect(16,225,176,2);
	}

	// Show() gets called from OnGuiManager
	public void Show() {
		if (game == null){
			Initialize();
		}

		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		if (game.treasureState == TreasureState.CARRYING){
			GUI.Label(helpPosition, "Press 'E' to drop the treasure");
		} else if(game.pickupState == PickupState.TREASURE){
			GUI.Label(helpPosition, "Press 'E' to pick up the treasure");
		} else if(game.pickupState == PickupState.ARMOR) {
			GUI.Label(helpPosition, "Press 'E' to pick up the EPIC armor");
		} else if (game.pickupState == PickupState.GRENADE_BOX) {
			GUI.Label(helpPosition, "Press 'E' to pick up the grenades");
		} else if (game.pickupState == PickupState.MINIGUN) {
			GUI.Label(helpPosition, "Press 'E' to pick up the minigun");
		} else if (game.pickupState == PickupState.SCAR_L) {
			GUI.Label(helpPosition, "Press 'E' to pick up the Scar-L");
		}
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;

		GUI.DrawTexture(healthPosition, healthbar);
		GUI.DrawTexture (armorPosition, armorbar);
		currentHealth = game.statistics.playerHealth;
		currentHealth = currentHealth*1.76f;
		currentArmor = game.statistics.playerArmor*1.76f*2;

		GUI.BeginGroup(new Rect(16,205,currentHealth,16)); 
		GUI.DrawTexture(new Rect(0,0,176,16), health);
		GUI.EndGroup();
		GUI.BeginGroup(new Rect(16,225,currentArmor,2));
		GUI.DrawTexture(new Rect(0,0,176,2), armor);
		GUI.EndGroup();

		GUILayout.BeginArea(new Rect(gui.GetWidth()-330,5,330,500));		                             
		GUILayout.Label("Treasure: " + game.statistics.treasureAmount, "hud_label");
		GUILayout.Label ("Score: " + game.statistics.score, "hud_score");
		
		if (game.statistics.gunEnabled){
			GUILayout.Label("Grenades: " + game.statistics.grenadeCount, "hud_label");
			GUILayout.Label("Gun: " + game.statistics.gunName, "hud_label");
			GUILayout.Label("Ammo: " + game.statistics.gunRounds, "hud_label");
			string clips;
			if (game.statistics.gunUnlimitedClips){
				clips = "unlimited";
			} else {
				clips = game.statistics.gunClips.ToString();
			}
			GUILayout.Label("Clips: " + clips, "hud_label");
			if (game.statistics.gunReloading){
				GUILayout.Label("Reloading...", "hud_label");
			}
		}

		GUILayout.EndArea();

	}	
	
	
	public Rect CalculateGUIRect(int width, int height){
		return CalculateGUIRect(width, height, 0, 0);
	}
	
	public Rect CalculateGUIRect(int width, int height, int xOffset, int yOffset){
		return new Rect(centerX-(width/2) + xOffset, centerY-(height/2) + yOffset, width, height);
	}
}
