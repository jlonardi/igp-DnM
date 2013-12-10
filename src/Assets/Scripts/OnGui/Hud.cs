using UnityEngine;
using System.Collections;

[System.Serializable]
public class Hud {
	public Texture2D healthBarTexture;
	public Texture2D healthBackgroundTexture;
	public Texture2D armorBarTexture;
	public Texture2D armorBackgroundTexture;
	public Texture2D dragonHealthBarTexture;

	public Texture2D gun_bg;
	public Texture2D gun_pistol;
	public Texture2D gun_m4;
	public Texture2D gun_minigun;
	public Texture2D gun_scar;

	private Rect helpPosition;
	private Rect notePosition;
	private Rect treasureNotePosition;
	private Rect gunInfoPosition;
	private Rect grenadeInfoPosition;

	private Rect healtBackPosition;
	private Rect armorBackPosition;
	private Rect dragonBackPosition;
	private GameManager game;
	private OnGuiManager gui;
	private int guiCenterX;
	private int guiCenterY;
	private int guiWidth;

	public void Initialize(){
		game = GameManager.instance;
		gui = OnGuiManager.instance;
		guiCenterX = gui.GetCenterX();
		guiCenterY = gui.GetCenterY();
		guiWidth = gui.GetWidth();

		helpPosition = new Rect(guiCenterX-350, 920, 650, 40);
		notePosition = new Rect(guiCenterX-350, guiCenterY-60, 700, 40);
		treasureNotePosition = new Rect(guiCenterX-350, 300, 700, 40);
		gunInfoPosition = new Rect(20,800,330,500);
		grenadeInfoPosition = new Rect(guiWidth-300,1010,300,20);

		//health & armor bar position and size
		healtBackPosition = new Rect(20,20,364,58);
		armorBackPosition = new Rect(26,63,352,8);
		dragonBackPosition = new Rect(guiCenterX-185,20,369,55);
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

		if(game.player.GetArmor() > 0) {
			healtBackPosition.height = 58; 
		} else {
			healtBackPosition.height = 45; 
		}
		GUI.DrawTexture(healtBackPosition, healthBackgroundTexture);

		playerHealth = playerHealth*3.52f;
		playerArmor = game.player.GetArmor()*7.04f;

		dragonHealth = game.dragon.GetHealth() / game.dragon.GetMaxHealth() * 100 * 3.52f;

		GUI.BeginGroup(new Rect(26,25, playerHealth,32)); 
		GUI.DrawTexture(new Rect(0,0,352,32), healthBarTexture);
		GUI.EndGroup();

		if(game.player.GetArmor() > 0) {
			GUI.DrawTexture (armorBackPosition, armorBackgroundTexture);
			GUI.BeginGroup(new Rect(26,64, playerArmor,5));
			GUI.DrawTexture(new Rect(0,0,352,4), armorBarTexture);
			GUI.EndGroup();
		}

		if(game.dragon.GetFighting() && dragonHealth > 0) {
			GUI.DrawTexture (dragonBackPosition, healthBackgroundTexture);
			GUI.BeginGroup(new Rect(guiCenterX-180, 30, dragonHealth, 32));
			GUI.DrawTexture(new Rect(0,0,352,32), dragonHealthBarTexture);
			GUI.EndGroup();
		}
		//display score and treasure amount
		GUI.Label(new Rect(guiWidth-330,5,320,500), "Score: " + game.statistics.score, "hud_score_treasure");
		GUI.Label(new Rect(guiWidth-330,55,320,500), "Treasure: " + game.treasure.GetTreasureAmount() + " %", "hud_score_treasure");

		//display gun data here
		GUILayout.BeginArea(gunInfoPosition);
		if (gun.enabled){
			GUI.DrawTexture(new Rect(0,0,300,100), gun_bg);
			if (gun.name == "M4" || gun.name == "M203") {
				GUI.DrawTexture(new Rect(0,0,300,100), gun_m4);
			}else if (gun.name == "Minigun") {
				GUI.DrawTexture(new Rect(0,0,300,100), gun_minigun);
			}else if (gun.name == "Scar-L") {
				GUI.DrawTexture(new Rect(0,0,300,100), gun_scar);
			}else {
				GUI.DrawTexture(new Rect(0,0,300,100), gun_pistol);
			}
			GUILayout.BeginArea(new Rect(0,110,330,200));
				GUILayout.Label("Gun: " + gun.name, "hud_gun");
				if (gun.currentRounds>0){
					GUILayout.Label("Ammo: " + gun.currentRounds, "hud_gun");
				} else {
					GUILayout.Label("Ammo: -", "hud_gun");
				}
				if (!gun.unlimited){
					GUILayout.Label("Clips: " + gun.totalClips, "hud_gun");
				}
			GUILayout.EndArea();
		}
		GUILayout.EndArea();

		if (gun.enabled){
			GUI.Label(grenadeInfoPosition, "Grenades: " + game.weapons.grenadeCount, "hud_right");
		}

		if (game.treasure.isEmpty()){
			GUI.Label(treasureNotePosition, "You failed to protect the treasure!", "hud_note");
		}

		if (gun.enabled && gun.reloading){
			GUI.Label(notePosition, "Reloading...", "hud_note");
		}

		// align next text elements to the center
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		if (!game.treasure.OnGround()){
			GUI.Label(helpPosition, "Press 'E' to drop the treasure");
		} else if(game.pickupState == PickupState.TREASURE){
			GUI.Label(helpPosition, "Press 'E' to pick up the treasure");
		} else if(game.pickupState == PickupState.ARMOR) {
			GUI.Label(helpPosition, "Press 'E' to pick up the armor");
		} else if (game.pickupState == PickupState.GRENADE_BOX) {
			GUI.Label(helpPosition, "Press 'E' to pick up the grenades");
		} else if (game.pickupState == PickupState.MINIGUN) {
			GUI.Label(helpPosition, "Press 'E' to pick up the minigun");
		} else if (game.pickupState == PickupState.SCAR_L) {
			GUI.Label(helpPosition, "Press 'E' to pick up the Scar-L");
		}

		// restore text alignment to the left for other OnGui-elements
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
	}	
}
