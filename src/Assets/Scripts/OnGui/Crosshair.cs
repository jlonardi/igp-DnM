using UnityEngine;

[System.Serializable]
public class Crosshair {
	public Texture2D crosshairVertical;
	public Texture2D crosshairHorizontal;
	private Rect crosshairPosition;
	private float crosshairSize;
	private int centerX;
	private int centerY;
	private OnGuiManager gui;

	public void Initialize(){
		gui = OnGuiManager.instance;

		// get screen center coordinates
		centerX = gui.nativeWidth/2 + gui.padWidth/2;
		centerY = gui.nativeHeight/2;
	}

	// Show() gets called from OnGuiManager
	public void Show() {
		if (gui == null){
			Initialize();
		}

		Gun gun = GunManager.instance.currentGun;
		if (gun!=null && gun.enabled){ // if gun in use, draw crosshair
			DrawCrosshair((int)gun.currentAccuracy);
		}
	}

	private void DrawCrosshair(int size){
		GUI.DrawTexture(new Rect(centerX - 4, centerY - 30 - size, 8, 30), crosshairVertical);
		GUI.DrawTexture(new Rect(centerX - 4, centerY + size, 8, 30), crosshairVertical);
		GUI.DrawTexture(new Rect(centerX - 30 - size, centerY - 4, 30, 8), crosshairHorizontal);
		GUI.DrawTexture(new Rect(centerX + size, centerY -4, 30, 8), crosshairHorizontal);
	}
}
