using UnityEngine;

using System.Collections;

public class GunManager : MonoBehaviour {
	public GunKeyBinder[] guns;	
	public int currentGunIndex;
	public Gun currentGun;
	public HitParticles hitParticles = new HitParticles();
	private Hud hud;
	
	void Start () {	
		hud = GameObject.Find("Game Manager").GetComponent<Hud>();
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
		
		hud.setGun(currentGun);
	}
	
	private void ChangeToGun(int gunIndex){
		Gun cGun = guns[gunIndex].gun;
		cGun.enabled = true;
		currentGun = cGun;
		currentGunIndex = gunIndex;				
	}	
}

