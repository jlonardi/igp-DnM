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

		helpPosition = CalculateGUIRect(700, 40, 0, 400);
		//health & armor bar position and size
		healthPosition = new Rect(20,20,369,55);
		armorPosition = new Rect(26,61,352,8);
	}

	// Show() gets called from OnGuiManager
	public void Show() {
		if (game == null){
			Initialize();
		}

		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		if (!game.treasure.OnGround()){
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
		currentHealth = currentHealth*3.52f;
		currentArmor = game.statistics.playerArmor*7.04f;

		GUI.BeginGroup(new Rect(26,25,currentHealth,32)); 
		GUI.DrawTexture(new Rect(0,0,352,32), health);
		GUI.EndGroup();
		GUI.BeginGroup(new Rect(26,66,currentArmor,4));
		GUI.DrawTexture(new Rect(0,0,352,4), armor);
		GUI.EndGroup();

		GUILayout.BeginArea(new Rect(gui.GetWidth()-330,5,330,500));		                             
		GUILayout.Label("Treasure: " + game.statistics.treasureAmount + " %", "hud_label");
		GUILayout.Label ("Score: " + game.statistics.score, "hud_score");

		Gun gun = game.weapons.currentGun;

		if (gun.enabled){
			GUILayout.Label("Grenades: " + game.weapons.grenadeCount, "hud_label");
			GUILayout.Label("Gun: " + gun.name, "hud_label");
			GUILayout.Label("Ammo: " + gun.currentRounds, "hud_label");
			string clips;
			if (gun.unlimited){
				clips = "unlimited";
			} else {
				clips = "" + gun.totalClips;
			}
			GUILayout.Label("Clips: " + clips, "hud_label");
			if (gun.reloading){
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
