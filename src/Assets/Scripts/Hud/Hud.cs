using UnityEngine;
using System.Collections;

public class Hud : MonoBehaviour {
	public Texture2D crosshairTexture;

	private Rect crosshairPosition;
	private float crosshairSize;
	private int health = 100;
	private int treasure = 100;
	private Gun gun = null;
		
	
	void Start () {		
		//size and alignment for gun crosshair
		crosshairSize = Screen.height/30;
	    crosshairPosition = new Rect((Screen.width-crosshairSize)/2, (Screen.height-crosshairSize)/2, crosshairSize,crosshairSize);	
	}
	
    void OnGUI() {
    	if(Time.timeScale!=0){ // draw HUD only when game is not paused
			
			if (gun.enabled){ // if gun in use, draw crosshair
    			GUI.DrawTexture(crosshairPosition, crosshairTexture);
			}
			
			GUI.Label(new Rect(10,10,100,20), "Health: " + health);
			GUI.Label(new Rect(10,25,100,20), "Treasure: " + treasure);
    	}
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
}
