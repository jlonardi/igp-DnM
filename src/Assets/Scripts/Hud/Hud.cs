﻿using UnityEngine;
using System.Collections;

public class Hud : MonoBehaviour {
	// no need for singleton as hud only checks other objects
	// hud is not used in any other gameobject
	
	public Texture2D crosshairTexture;
	
	private Rect crosshairPosition;
	private Rect helpPosition;
	private int crosshairSize;
		
	
	void Start () {		
		//size and alignment for gun crosshair
		crosshairSize = Screen.height/30;
	    crosshairPosition = CalculateGUIRect(crosshairSize, crosshairSize, 0, 0);
	    helpPosition = CalculateGUIRect(500, 40, 0, -40);
	}
	
    void OnGUI() {

		
    	if(Time.timeScale == 0 || !GameManager.instance.gameRunning || GameManager.instance.paused){
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
		GUI.Label(new Rect(10,25,100,20), "Treasure: " + Treasure.instance.amount);

		GUI.Label (new Rect(10,40,100,20),"Score: " +GameManager.instance.score);
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
