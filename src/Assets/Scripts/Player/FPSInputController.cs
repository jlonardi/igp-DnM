using UnityEngine;
using System.Collections;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Character/FPS Input Controller")]
public class FPSInputController : MonoBehaviour {
	
	[HideInInspector]
	public bool fire;
	private bool firing;
	private float firingTimer;
	public float idleTimer;	
	[HideInInspector]
	public bool reloading;
	
	[HideInInspector]
	public string currentWeaponName;
	
	[HideInInspector]
	public int currentWeapon;
	
	private CharacterMotor motor;
	private GameManager game;
	private PlayerSounds sounds;
	private Camera playerCam;
	private GunManager gunManager;
	private CharacterController controller;
	private float timeOfLastGrenade;

	// Use this for initialization
	void Start () {
		playerCam = PlayerCamera.instance.camera;
		controller = GetComponent<CharacterController>();
		gunManager = GunManager.instance;
		motor = GetComponent<CharacterMotor>();
		sounds = PlayerSounds.instance;
	}
	
	// Update is called once per frame
	void Update () {
		if (game == null){
			game = GameManager.instance;
		}
		if(game.gameState != GameState.RUNNING){
			return;
		}
		
		// Get the input vector from kayboard or analog stick
		Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
		
		if (directionVector != Vector3.zero) {
			// Get the length of the directon vector and then normalize it
			// Dividing by the length is cheaper than normalizing when we already have the length anyway
			float directionLength = directionVector.magnitude;
			directionVector = directionVector / directionLength;
			
			// Make sure the length is no bigger than 1
			directionLength = Mathf.Min(1, directionLength);
			
			// Make the input vector more sensitive towards the extremes and less sensitive in the middle
			// This makes it easier to control slow speeds when using analog sticks
			directionLength = directionLength * directionLength;
			
			// Multiply the normalized direction vector by the modified length
			directionVector = directionVector * directionLength;

			sounds.PlayWalkSound();
		}
		
		// Apply the direction to the CharacterMotor
		motor.inputMoveDirection = transform.rotation * directionVector;
		motor.inputJump = Input.GetButton("Jump");	
				
		//Check if the user if firing the weapon
		fire = Input.GetButton("Fire1") && GameManager.instance.treasureState == TreasureState.SET_ON_GROUND &&
			GunManager.instance.currentGun.freeToShoot;

		idleTimer += Time.deltaTime;
		
		if(fire)
		{
			firing = true;
			firingTimer -= Time.deltaTime;
			idleTimer = 0.0f;
		}
		else
		{
			firing = false;
			firingTimer = 0.3f;
		}
		if (Input.GetButton("Grenade") && (timeOfLastGrenade + gunManager.grenadeThrowDelay) < Time.time && gunManager.grenadeCount > 0){
			timeOfLastGrenade = Time.time;
			gunManager.grenadeCount--;
			ThrowGrenade();
		}

//		firing = (firingTimer <= 0.0f && fire);
		
		if(game.treasureState == TreasureState.SET_ON_GROUND){
			GunManager.instance.currentGun.fire = firing;
			reloading = GunManager.instance.currentGun.reloading;
			currentWeaponName = GunManager.instance.currentGun.name;
			currentWeapon = GunManager.instance.currentGunIndex;
		}
		
		
		if (Input.GetButton("Use") && game.treasureState == TreasureState.CARRYING){
			game.treasureState = TreasureState.SET_ON_GROUND;
		}

	}

	public void ThrowGrenade(){
		Ray camRay = playerCam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.6f, 0f));
		Vector3 startPosition = playerCam.ScreenToWorldPoint(new Vector3 (Screen.width * 0.5f, Screen.height * 0.6f, 0.8f));
		GameObject grenade = (GameObject)Instantiate(gunManager.handGrenadePrefab, startPosition, Quaternion.identity);
		grenade.transform.rotation = Quaternion.LookRotation(camRay.direction);
		Rigidbody grenadeRigidbody = grenade.rigidbody;

		Vector3 grenadeDirection = (playerCam.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.3f, 1f)) 
			- this.transform.position).normalized;

		// add player movement to the grenadde throw velocity
		grenadeRigidbody.velocity = controller.velocity + grenadeDirection * gunManager.grenadeSpeed;
	}
}