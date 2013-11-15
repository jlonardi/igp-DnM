using UnityEngine;

[System.Serializable]
public class BloodSplatter {
	public Texture2D bloodSplatter;
	
	private Rect splatterPosition;
	private Color bloodColor;
	private Color originalColor;
	
	public float fadeSpeed = 0.05f;
	
	private float bloodAlpha; 
	private float fadeDir;

	public BloodSplatter() {		

		splatterPosition = new Rect(0, 0, Screen.width, Screen.height);
		
		bloodAlpha = 0f;
		fadeDir = -1f;
	}


	// this get's called from game manager when GameState.RUNNING
	public void Show() {
		bloodColor = GUI.color;
		originalColor = GUI.color;
		bloodColor.a = bloodAlpha;
		
		bloodAlpha = bloodAlpha + fadeDir * fadeSpeed * Time.deltaTime;	
		bloodAlpha = Mathf.Clamp01(bloodAlpha);	
		bloodColor.a = bloodAlpha;
		
		GUI.color = bloodColor;
		GUI.DrawTexture(splatterPosition, bloodSplatter);
		GUI.color = originalColor;
	}	
	

	public void setSplatterVisible(float a) {
		if(a < 0.2f) a = 0.2f;
		if(a > 0.9f) a = 0.9f;
		bloodAlpha = a;
		fadeBloodSplatterOut();
	}
	
	public void fadeBloodSplatterIn() {
		fadeDir = 1;	
	}
	
	public void fadeBloodSplatterOut() {
		fadeDir = -1;	
	}
}
