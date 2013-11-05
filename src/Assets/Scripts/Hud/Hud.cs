using UnityEngine;
using System.Collections;

public class Hud : MonoBehaviour {
	public Texture2D crosshairTexture;

	private GameManager game;
	private GunManager gunManager;
	private Treasure treasure;
	private PlayerHealth vitals;
	
	private Rect crosshairPosition;
	private Rect helpPosition;
	private int crosshairSize;
		
	
	void Start () {
		game = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
		gunManager = GameObject.FindObjectOfType(typeof(GunManager)) as GunManager;
		treasure = GameObject.FindObjectOfType(typeof(Treasure)) as Treasure;
		vitals = GameObject.FindObjectOfType(typeof(PlayerHealth)) as PlayerHealth;
		
		//size and alignment for gun crosshair
		crosshairSize = Screen.height/30;
	    crosshairPosition = CalculateGUIRect(crosshairSize, crosshairSize, 0, 0);
	    helpPosition = CalculateGUIRect(500, 40, 0, -40);
	}
	
    void OnGUI() {
		if (!game.gameRunning && !game.paused){
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label(helpPosition, "GAME OVER!");
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		}
		
    	if(Time.timeScale == 0 || !game.gameRunning || game.paused){
			return;
		}
		
		if (!treasure.onGround){
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label(helpPosition, "Press 'E' to drop the treasure");
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		}
			
		Gun gun = gunManager.currentGun;
		if (gun!=null && gun.enabled){ // if gun in use, draw crosshair
    		GUI.DrawTexture(crosshairPosition, crosshairTexture);
		}
			
		GUI.Label(new Rect(10,10,100,20), "Health: " + vitals.health);
		GUI.Label(new Rect(10,25,100,20), "Treasure: " + treasure.amount);
		if (gun.enabled){
			GUI.Label(new Rect(10,40,100,20), "Gun: " + gun.gunName);			
			GUI.Label(new Rect(10,55,100,20), "Ammo: " + gun.currentRounds);
			string clips = gun.totalClips.ToString();
			if (gun.unlimited){
				clips = "unlimited";
			}
			GUI.Label(new Rect(10,70,100,20), "Clips: " + clips);
			if (gun.reloading){
				GUI.Label(new Rect(10,85,100,20), "Reloading...");
			}
		}
	}	
	
	
	public Rect CalculateGUIRect(int width, int height){
		return CalculateGUIRect(width, height, 0, 0);
	}
	
	public Rect CalculateGUIRect(int width, int height, int xOffset, int yOffset){
		return new Rect((Screen.width-width)/2 + xOffset, (Screen.height-height)/2 + yOffset, width, height);
	}
}
