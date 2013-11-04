using UnityEngine;

using System.Collections;

public class GunManager : MonoBehaviour {
	public GunKeyBinder[] guns;	
	public int currentGunIndex;
	public Gun currentGun;
	public HitParticles hitParticles = new HitParticles();
	private Hud hud;
	private Treasure treasure;
	
	void Start () {	
		hud = GameObject.FindObjectOfType(typeof(Hud)) as Hud;
		treasure = GameObject.FindObjectOfType(typeof(Treasure)) as Treasure;
		
		
		for (int i=0; i<guns.Length; i++){
			guns[i].gun.enabled = false;
		}
		ChangeToGun(0);
		
		// set to false until treasure on ground
		currentGun.enabled = false;
	}
	
	void Update () {		
		for (int i=0; i<guns.Length; i++){
			if (Input.GetKeyDown(guns[i].keyToActivate)){
				ChangeToGun(i);
			}
		}				
		if (treasure.OnGround()){
			currentGun.enabled = true;
		}
	}
	
	private void ChangeToGun(int gunIndex){
		guns[currentGunIndex].gun.enabled = false;		
		currentGun = guns[gunIndex].gun;
		currentGunIndex = gunIndex;
		hud.setGun(currentGun);
	}	
}

