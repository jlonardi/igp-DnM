using UnityEngine;

using System.Collections;

public class GunManager : MonoBehaviour {	
	//use singleton since only we need once instance of this class
	public static GunManager instance;

	public GameObject handGrenadePrefab;
	public float grenadeSpeed = 10f;
	public float grenadeThrowDelay = 1f;
	public int grenadeCount = 10;

	public GunKeyBinder[] guns;	
	public int currentGunIndex;
	public Gun currentGun;
	public HitParticles hitParticles = new HitParticles();
	[HideInInspector]
	public GameObject shootFrom;
	public GameManager game;

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
		if (game == null){
			game = GameManager.instance;
		}

		if (!currentGun.enabled && game.treasureState != TreasureState.CARRYING){
			currentGun.enabled = true;
		}

		for (int i=0; i<guns.Length; i++){
			if (Input.GetKeyDown(guns[i].keyToActivate)){
				ChangeToGun(i);
			}
		}

		game.statistics.gunEnabled = currentGun.enabled;
		game.statistics.gunName = currentGun.name;
		game.statistics.gunUnlimitedClips = currentGun.unlimited;
		game.statistics.gunClips = currentGun.totalClips;
		game.statistics.gunRounds = currentGun.currentRounds;
		game.statistics.gunReloading = currentGun.reloading;

	}
	
	public void ChangeToCurrentWeapon(){
		ChangeToGun(currentGunIndex);
	}
	
	public void ChangeToGun(int gunIndex){	
		bool oldEnabled = currentGun.enabled;
		currentGun.enabled = false;
		currentGun = guns[gunIndex].gun;
		currentGunIndex = gunIndex;
		if (oldEnabled){
			currentGun.enabled = true;
		}
	}	
}

