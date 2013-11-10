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

	[HideInInspector]
	public GameObject shootFrom;
	
	void Start () {	
		shootFrom = transform.FindChild("ShootFrom").gameObject;

		// set to false until weapons enabled
		for (int i=0; i<guns.Length; i++){
			guns[i].gun.enabled = false;
			guns[i].gun.gameObject.SetActive(false);
		}		
	}
	
	void Update () {		
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

