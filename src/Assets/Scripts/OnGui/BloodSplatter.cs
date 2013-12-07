using UnityEngine;

[System.Serializable]
public class BloodSplatter {
	public Texture2D bloodSplatter;
	public float fadeSpeed = 0.05f;
	public float pulseTreshold = 0f;

	private Rect splatterPosition;
	private Color bloodColor;
	private Color originalColor;
	private float bloodAlpha;
	private float fadeDir;
	private OnGuiManager gui;

	public void Initialize(){
		gui = OnGuiManager.instance;
		splatterPosition = new Rect(0, 0, gui.GetWidth(), gui.GetHeight());
		bloodAlpha = 0f;
		fadeDir = -1f;
	}

	// Show() gets called from OnGuiManager
	public void Show() {
		if (gui == null){
			Initialize();
		}

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
		pulseTreshold = a - 0.3f;
		fadeBloodSplatterOut();
	}

	public void fadeBloodSplatterIn() {
		fadeDir = 1;	
	}
	
	public void fadeBloodSplatterOut() {
		fadeDir = -1;	
	}

	public void SetBloodAlpha(float value){
		bloodAlpha = value;
	}

	public float GetBloodAlpha(){
		return bloodAlpha;
	}

}
