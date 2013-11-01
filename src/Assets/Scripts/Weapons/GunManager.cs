using UnityEngine;

using System.Collections;

public class GunManager : MonoBehaviour{
	public Texture2D crosshairTexture;
	private Rect rectPosition;
	private float rectSize;
	
	public GunKeyBinder[] guns;	
	public int currentGunIndex;
	public Gun currentGun;
	public HitParticles hitParticles = new HitParticles();
	
	void Start () {		
		//size and alignment for gun reticle
		rectSize = Screen.height/15;
	    rectPosition = new Rect((Screen.width-rectSize)/2, (Screen.height-rectSize)/2,rectSize,rectSize);	

		for (int i=0; i<guns.Length; i++){
			guns[i].gun.enabled = false;
		}
		currentGunIndex = 0;
		guns[0].gun.enabled = true;
		currentGun = guns[0].gun;
	}
	
	void Update () {
		for (int i=0; i<guns.Length; i++){
			if (Input.GetKeyDown(guns[i].keyToActivate)){
				ChangeToGun(i);
			}
		}		
		//hud.selectedGun = currentGun;
		//hud.ammoRemaining[currentGunIndex] = guns[currentGunIndex].gun.currentRounds;
	}
	
	private void ChangeToGun(int gunIndex){
		Gun cGun = guns[gunIndex].gun;
		cGun.enabled = true;
		currentGun = cGun;
		currentGunIndex = gunIndex;				
	}
	
    void OnGUI() {
    	if(Time.timeScale!=0 && currentGun.enabled) {
    		GUI.DrawTexture(rectPosition, crosshairTexture);
    	}
	}	
}

