using UnityEngine;

using System.Collections;

public class GunManager : MonoBehaviour {	
	public GameObject handGrenadePrefab;
	public float grenadeSpeed = 12f;
	public float grenadeThrowDelay = 1f;
	public int grenadeCount = 10;
	private bool throwingGrenade = false;
	private float timeOfLastGrenade;

	public Gun[] guns;	
	public int currentGunIndex;
	public Gun currentGun;
	public HitParticles hitParticles = new HitParticles();
	private GameManager game;
	private Camera playerCam;
	private CharacterController controller;
	private GameObject throwFrom;

	void Start () {	
		throwFrom = transform.root.FindChild("ThrowGrenadeFrom").gameObject;
		playerCam = PlayerCamera.instance.camera;
		controller = transform.root.GetComponent<CharacterController>();

		// set all guns to false at start
		for (int i=0; i<guns.Length; i++){
			guns[i].enabled = false;
		}
	}
	
	void Update () {
		if (game == null){
			game = GameManager.instance;
		}

		if (!currentGun.enabled && game.treasure.OnGround()){
			currentGun.enabled = true;
		}

		if (throwingGrenade && (timeOfLastGrenade + grenadeThrowDelay) < Time.time){
			DelayedGrenadeThrow();
		}
	}
	
	public void ChangeToCurrentWeapon(){
		ChangeToGun(currentGunIndex);
	}
	
	public void ChangeToGun(int gunIndex){
		// if gun is not available yet, do nothing
		if (!guns[gunIndex].picked_up){
			return;
		}

		float accuracy = currentGun.currentAccuracy;
		bool oldEnabled = currentGun.enabled;
		currentGun.enabled = false;
		currentGun = guns[gunIndex];
		currentGunIndex = gunIndex;
		currentGun.currentAccuracy = accuracy;
		if (oldEnabled){
			currentGun.enabled = true;
		}
	}

	// method which is called when player wants to throw a grenade
	public void ThrowGrenade(){
		// if treasure is not on ground or already throwing a grenade, return
		if (!game.treasure.OnGround() || throwingGrenade){
			return;
		}
		if ( grenadeCount <= 0){
			// out of ammo, sound?
			return;
		}

		timeOfLastGrenade = Time.time;
		throwingGrenade = true;
		// play audio for removing the grenade socket here
	}

	// method with actually throws the grenade
	private void DelayedGrenadeThrow(){
		throwingGrenade = false;
		grenadeCount--;
		Vector3 startPosition = throwFrom.transform.position;
		GameObject grenade = (GameObject)Instantiate(handGrenadePrefab, startPosition, Quaternion.identity);
		
		Ray camRay = playerCam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));
		grenade.transform.rotation = Quaternion.LookRotation(camRay.direction);
		
		Vector3 grenadeDirection = (playerCam.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.7f, 10f)) 
		                            - this.transform.position).normalized;
		
		// add player movement to the grenadde throw velocity
		grenade.rigidbody.velocity = controller.velocity + grenadeDirection * grenadeSpeed;
	}

}

