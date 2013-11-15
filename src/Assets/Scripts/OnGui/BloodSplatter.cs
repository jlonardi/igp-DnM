﻿using UnityEngine;

[System.Serializable]
public class BloodSplatter {
	public Texture2D bloodSplatter;
	
	private Rect splatterPosition;
	private Color bloodColor;
	private Color originalColor;
	
	public float fadeSpeed = 0.05f;

	[HideInInspector]
	public float bloodAlpha;

	public float pulseTreshold = 0f;

	private float fadeDir;

	// constructor
	public BloodSplatter() {		
		splatterPosition = new Rect(0, 0, Screen.width, Screen.height);
		
		bloodAlpha = 0f;
		fadeDir = -1f;
	}


	// Show() gets called from OnGuiManager
	public void Show() {


			bloodColor = GUI.color;
			originalColor = GUI.color;
			bloodColor.a = bloodAlpha;
			
			
		if(bloodAlpha >= pulseTreshold) {
			bloodAlpha = bloodAlpha + fadeDir * fadeSpeed * Time.deltaTime;	
			bloodAlpha = Mathf.Clamp01(bloodAlpha);
			bloodColor.a = bloodAlpha;
		}
		GUI.color = bloodColor;
			GUI.DrawTexture(splatterPosition, bloodSplatter);
			GUI.color = originalColor;
		
	}
	

	public void setSplatterVisible(float a) {
		if(a < 0.35f) a = 0.35f;
		bloodAlpha = a;
		pulseTreshold = a - 0.1f;
		fadeBloodSplatterOut();
	}
	
	public void fadeBloodSplatterIn() {
		fadeDir = 1;	
	}
	
	public void fadeBloodSplatterOut() {
		fadeDir = -1;	
	}
}
