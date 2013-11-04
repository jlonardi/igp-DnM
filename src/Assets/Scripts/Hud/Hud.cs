using UnityEngine;
using System.Collections;

public class Hud : MonoBehaviour {
	public Texture2D crosshairTexture;

	private Rect crosshairPosition;
	private Rect helpPosition;
	private int crosshairSize;
	private int health = 100;
	private int treasure = 100;
	private bool treasureOnGround = false;
	private Gun gun = null;
		
	
	void Start () {		
		//size and alignment for gun crosshair
		crosshairSize = Screen.height/30;
	    crosshairPosition = CalculateGUIRect(crosshairSize, crosshairSize, 0, 0);
	    helpPosition = CalculateGUIRect(500, 40, 0, -40);
	}
	
    void OnGUI() {
    	if(Time.timeScale==0){ // draw HUD only when game is not paused
			return;
		}
		
		if (!treasureOnGround){
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label(helpPosition, "Press 'E' to drop the treasure");
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		}
			
		if (gun!=null && gun.enabled){ // if gun in use, draw crosshair
    		GUI.DrawTexture(crosshairPosition, crosshairTexture);
		}
			
		GUI.Label(new Rect(10,10,100,20), "Health: " + health);
		GUI.Label(new Rect(10,25,100,20), "Treasure: " + treasure);
	}	

	public void setGun(Gun currentGun){
		this.gun = currentGun;		
	}
		
	public void setHealth(int health){
		this.health = health;
	}
	
	public void setTreasure(int treasure){		
		this.treasure = treasure;
	}
	
	public void setTreasureOnGround(){
		this.treasureOnGround = true;
	}
	
	
	public Rect CalculateGUIRect(int width, int height){
		return CalculateGUIRect(width, height, 0, 0);
	}
	
	public Rect CalculateGUIRect(int width, int height, int xOffset, int yOffset){
		return new Rect((Screen.width-width)/2 + xOffset, (Screen.height-height)/2 + yOffset, width, height);
	}
}
