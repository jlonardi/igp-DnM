using UnityEngine;

using System.Collections;

public class GunManager : MonoBehaviour {	
	//use singleton since only we need once instance of this class
	public static GunManager instance;
    public void Awake()
    {
        GunManager.instance = this;
    }
	
	public GunKeyBinder[] guns;	
	public int currentGunIndex;
	public Gun currentGun;
	public bool canUseWeapons;
	public HitParticles hitParticles = new HitParticles();
	
	void Start () {	
		// set to false until weapons enabled
		for (int i=0; i<guns.Length; i++){
			guns[i].gun.enabled = false;
			guns[i].gun.gameObject.SetActive(false);
		}
		
		// temp fix
		if (Treasure.instance.onGround){
			EnableWeapons();
			ChangeToGun(currentGunIndex);
		}//
	}
	
	void Update () {
		//temp fix for savegame
		if (guns[0].gun.gameObject.activeSelf && guns[1].gun.gameObject.activeSelf)
		{
			if (canUseWeapons){
				if (currentGunIndex == 0){
					guns[1].gun.gameObject.SetActive(false);
					guns[1].gun.enabled = false;
				} else {
					guns[0].gun.gameObject.SetActive(false);
					guns[0].gun.enabled = false;
				}
			} else {
					guns[0].gun.enabled = false;
					guns[1].gun.enabled = false;
					guns[0].gun.gameObject.SetActive(false);
					guns[1].gun.gameObject.SetActive(false);
			}
		} //
		
		for (int i=0; i<guns.Length; i++){
			if (Input.GetKeyDown(guns[i].keyToActivate)){
				ChangeToGun(i);
			}
		}				
	}
	
	public void EnableWeapons(){
		canUseWeapons = true;
		ChangeToGun(0);
	}
	
	public void ChangeToGun(int gunIndex){	
		//temp fix
		guns[0].gun.enabled = false;
		guns[1].gun.enabled = false;
		//
		
		currentGun.enabled = false;
		currentGun.gameObject.SetActive(false);
		//guns[currentGunIndex].gun.enabled = false;
		currentGun = guns[gunIndex].gun;
		currentGunIndex = gunIndex;
		if (canUseWeapons){
			currentGun.enabled = true;
			currentGun.gameObject.SetActive(true);
		}
	}	
}

