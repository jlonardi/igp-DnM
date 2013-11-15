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
	
	// Use this for initialization
	void Start () {
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
}