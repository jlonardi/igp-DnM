using UnityEngine;
using System.Collections;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Character/FPS Input Controller")]
public class FPSInputController : MonoBehaviour {
	
	[HideInInspector]
	public bool fire;
	private GunManager weaponSystem;
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
	private Treasure treasure;
	private GameManager game;

	
	// Use this for initialization
	void Start () {
		motor = GetComponent<CharacterMotor>();
		weaponSystem = GameObject.FindObjectOfType(typeof(GunManager)) as GunManager;
		treasure = GameObject.FindObjectOfType(typeof(Treasure)) as Treasure;
		game = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
	}
	
	// Update is called once per frame
	void Update () {
		if(!game.gameRunning || game.paused){ //Mouse look do not work if game is paused or over
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
		}
		
		// Apply the direction to the CharacterMotor
		motor.inputMoveDirection = transform.rotation * directionVector;
		motor.inputJump = Input.GetButton("Jump");	
		
		bool treasureOnGround = treasure.onGround;
		
		//Check if the user if firing the weapon
		fire = Input.GetButton("Fire1") && treasureOnGround && weaponSystem.currentGun.freeToShoot;
			
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
		
//		firing = (firingTimer <= 0.0f && fire);
		
		if(weaponSystem != null && treasureOnGround)
		{
			weaponSystem.currentGun.fire = firing;
			reloading = weaponSystem.currentGun.reloading;
			currentWeaponName = weaponSystem.currentGun.gunName;
			currentWeapon = weaponSystem.currentGunIndex;
		}
		
		
		if (Input.GetButton("Use") && !treasureOnGround ){
			treasure.SetTreasureOnGround();
		}

	}
}