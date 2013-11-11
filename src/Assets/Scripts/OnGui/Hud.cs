using UnityEngine;
using System.Collections;

[System.Serializable]
public class Hud {
	public Texture2D crosshairTexture;	

	private Rect crosshairPosition;
	private Rect helpPosition;
	private int crosshairSize;
	
	public Hud() {		
		//size and alignment for gun crosshair
		crosshairSize = Screen.height/30;
	    crosshairPosition = CalculateGUIRect(crosshairSize, crosshairSize, 0, 0);
	    helpPosition = CalculateGUIRect(500, 40, 0, -40);
	}
	
	// this get's called from game manager when GameState.RUNNING
    public void Show() {
		// failsafe check	
		if (!Treasure.instance || !GunManager.instance){
			return;
		}
		
		if (!Treasure.instance.onGround){
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label(helpPosition, "Press 'E' to drop the treasure");
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		}
			
		Gun gun = GunManager.instance.currentGun;
		if (gun!=null && gun.enabled){ // if gun in use, draw crosshair
    		GUI.DrawTexture(crosshairPosition, crosshairTexture);
		}
		GUI.Box(new Rect(5,5,105,125),"");	
		GUI.Label(new Rect(10,10,100,20), "Health: " + PlayerHealth.instance.health);
		GUI.Label(new Rect(10,25,100,20), "Treasure: " + Treasure.instance.treasureAmount);
		GUI.Label (new Rect(10,40,100,20),"Score: " + GameManager.instance.statistics.score);
		
		if (gun.enabled){
			GUI.Label(new Rect(10,55,100,20), "Gun: " + gun.gunName);			
			GUI.Label(new Rect(10,70,100,20), "Ammo: " + gun.currentRounds);
			string clips = gun.totalClips.ToString();
			if (gun.unlimited){
				clips = "unlimited";
			}
			GUI.Label(new Rect(10,85,100,20), "Clips: " + clips);
			if (gun.reloading){
				GUI.Label(new Rect(10,100,100,20), "Reloading...");
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
