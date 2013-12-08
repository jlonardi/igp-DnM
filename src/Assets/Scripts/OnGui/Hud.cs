using UnityEngine;
using System.Collections;

[System.Serializable]
public class Hud {
	public Texture2D healthBarTexture;
	public Texture2D healthBackgroundTexture;
	public Texture2D armorBarTexture;
	public Texture2D armorBackgroundTexture;
	public Texture2D dragonHealthBarTexture;

	private Rect helpPosition;

	private Rect healtBackPosition;
	private Rect armorBackPosition;
	private Rect dragonBackPosition;
	private GameManager game;
	private OnGuiManager gui;
	private int centerX;
	private int centerY;

	public void Initialize(){
		game = GameManager.instance;
		gui = OnGuiManager.instance;
		centerX = gui.GetCenterX();
		centerY = gui.GetCenterY();

		helpPosition = new Rect(centerX-350, 920, 700, 40);
		//health & armor bar position and size
		healtBackPosition = new Rect(20,20,369,55);
		armorBackPosition = new Rect(26,61,352,8);
		dragonBackPosition = new Rect(centerX-185,20,369,55);
	}

	// Show() gets called from OnGuiManager
	public void Show() {
		if (game == null){
			Initialize();
		}

		float playerHealth = game.player.GetHealth();
		float playerArmor = game.player.GetArmor();
		float dragonHealth = game.dragon.GetHealth();
		Gun gun = game.weapons.currentGun;

		GUI.DrawTexture(healtBackPosition, healthBackgroundTexture);
		GUI.DrawTexture (armorBackPosition, armorBackgroundTexture);

		playerHealth = playerHealth*3.52f;
		playerArmor = game.player.GetArmor()*7.04f;

		dragonHealth = game.dragon.GetHealth() / game.dragon.GetMaxHealth() * 100 * 3.52f;

		GUI.BeginGroup(new Rect(26,25, playerHealth,32)); 
		GUI.DrawTexture(new Rect(0,0,352,32), healthBarTexture);
		GUI.EndGroup();
		GUI.BeginGroup(new Rect(26,66, playerArmor,4));
		GUI.DrawTexture(new Rect(0,0,352,4), armorBarTexture);
		GUI.EndGroup();

		if(game.dragon.GetFighting() && dragonHealth > 0) {
			GUI.DrawTexture (dragonBackPosition, healthBackgroundTexture);
			GUI.BeginGroup(new Rect(centerX-180, 30, dragonHealth, 32));
			GUI.DrawTexture(new Rect(0,0,352,32), dragonHealthBarTexture);
			GUI.EndGroup();
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

		GUILayout.BeginArea(new Rect(gui.GetWidth()-330,5,330,500));		                             
		GUILayout.Label("Treasure: " + game.treasure.GetTreasureAmount() + " %", "hud_label");
		GUILayout.Label ("Score: " + game.statistics.score, "hud_score");

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
}
