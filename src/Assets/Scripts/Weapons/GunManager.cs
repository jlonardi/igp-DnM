using UnityEngine;

using System.Collections;

public class GunManager : MonoBehaviour {	
	//use singleton since only we need once instance of this class
	public static GunManager instance;
	
	public GunKeyBinder[] guns;	
	public int currentGunIndex;
	public Gun currentGun;
	public HitParticles hitParticles = new HitParticles();
	[HideInInspector]
	public GameObject shootFrom;

	public void Awake(){
        GunManager.instance = this;
    }	
	
	void Start () {	
		shootFrom = transform.FindChild("ShootFrom").gameObject;
		// set to false at start
		for (int i=0; i<guns.Length; i++){
			guns[i].gun.enabled = false;
		}
	}
	
	void Update () {
		if (!currentGun.enabled){
			ChangeToGun(currentGunIndex);
		}
		for (int i=0; i<guns.Length; i++){
			if (Input.GetKeyDown(guns[i].keyToActivate)){
				ChangeToGun(i);
			}
		}				
	}
	
	public void ChangeToCurrentWeapon(){
		ChangeToGun(currentGunIndex);
	}
	
	public void ChangeToGun(int gunIndex){	
		currentGun.enabled = false;
		currentGun = guns[gunIndex].gun;
		currentGunIndex = gunIndex;
		if (Treasure.instance.onGround){
			currentGun.enabled = true;
		}
	}	
}

