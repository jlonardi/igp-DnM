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
	private GameManager game;
	private Gun currentGun;

	public void Initialize(){
		gui = OnGuiManager.instance;
		game = GameManager.instance;
		currentGun = game.weapons.currentGun;

		// get screen center coordinates
		centerX = gui.GetCenterX();
		centerY = gui.GetCenterY();
	}

	// Show() gets called from OnGuiManager
	public void Show() {
		if (gui == null){
			Initialize();
		}

		if (currentGun!=null && currentGun.enabled){ // if gun in use, draw crosshair
			DrawCrosshair((int)(currentGun.currentAccuracy+10)*2);
		}
	}

	private void DrawCrosshair(int size){
		GUI.DrawTexture(new Rect(centerX - 4, centerY - 30 - size, 8, 30), crosshairVertical);
		GUI.DrawTexture(new Rect(centerX - 4, centerY + size, 8, 30), crosshairVertical);
		GUI.DrawTexture(new Rect(centerX - 30 - size, centerY - 4, 30, 8), crosshairHorizontal);
		GUI.DrawTexture(new Rect(centerX + size, centerY -4, 30, 8), crosshairHorizontal);
	}
}
