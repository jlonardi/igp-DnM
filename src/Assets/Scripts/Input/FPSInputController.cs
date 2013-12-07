using UnityEngine;
using System.Collections;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Character/FPS Input Controller")]
public class FPSInputController : MonoBehaviour {
	
	[HideInInspector]
	public bool fire;
	[HideInInspector]
	public bool reloading;
	[HideInInspector]
	public int currentWeapon;

	private bool firing;
	private float firingTimer;
	public float idleTimer;	

	[HideInInspector]
	public string currentWeaponName;
	

	private CharacterMotor motor;
	private GameManager game;
	private PlayerSounds sounds;

	// Use this for initialization
	void Start () {
		game = GameManager.instance;
		motor = GetComponent<CharacterMotor>();
		sounds = PlayerSounds.instance;
	}
	
	// Update is called once per frame
	void Update () {
		if(game.gameState != GameState.RUNNING){
			return;
		}

		bool treasureOnGround = game.treasure.OnGround();

		// Get the input vector from kayboard
		Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

		if (directionVector != Vector3.zero) {
			// Get the length of the directon vector and then normalize it
			// Dividing by the length is cheaper than normalizing when we already have the length anyway
			float directionLength = directionVector.magnitude;

			// if player stops while sprinting, stop sprint
			if (motor.sprinting && directionLength<0.5f){
				motor.StopSprint();
			}

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

		int currentGunIndex = game.weapons.currentGunIndex;

		if (Input.GetAxis("Mouse ScrollWheel") < 0){
			game.weapons.ChangeToNextGun();
		} else if (Input.GetAxis("Mouse ScrollWheel") > 0){
			game.weapons.ChangeToPreviousGun();
		}

		if (Input.GetButtonDown("Pistol")){
			game.weapons.ChangeToGun(0);
		}
		if (Input.GetButtonDown("Assault Rifle")){
			game.weapons.ChangeToGun(1);
		}
		if (Input.GetButtonDown("Grenade Launcher")){
			game.weapons.ChangeToGun(2);
		}
		if (Input.GetButtonDown("Minigun")){
			game.weapons.ChangeToGun(3);
		}
		if (Input.GetButtonDown("Scar-L")){
			game.weapons.ChangeToGun(4);
		}

		if (Input.GetButton("Sprint") && treasureOnGround){
			motor.StartSprint();
		}


		//Check if the user if firing the weapon
		fire = Input.GetButton("Fire") && treasureOnGround && game.weapons.currentGun.freeToShoot;

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
		if (Input.GetButton("Grenade")){
			game.weapons.ThrowGrenade();
		}

//		firing = (firingTimer <= 0.0f && fire);
		
		if(treasureOnGround){
			game.weapons.currentGun.fire = firing;
			reloading = game.weapons.currentGun.reloading;
			currentWeaponName = game.weapons.currentGun.name;
			currentWeapon = game.weapons.currentGunIndex;
		}
		
		
		if (Input.GetButtonDown("Use")){
			if (!treasureOnGround){
				game.treasure.SetTreasureOnGround();

			} else if(game.pickupState == PickupState.TREASURE){
				game.treasure.CarryTreasure();

			} else if(game.pickupState == PickupState.MINIGUN){
				game.weapons.guns[3].PickUp();
				game.weapons.ChangeToGun(3);
				// hide minigun on scene
				GameObject minigun = GameObject.Find("minigunOnGround");
				minigun.SetActive(false);
				
			} else if(game.pickupState == PickupState.SCAR_L){
				game.weapons.guns[4].PickUp();
				game.weapons.ChangeToGun(4);
				// hide scar-L on scene
				GameObject scarL = GameObject.Find("scarlOnGround");
				scarL.SetActive(false);

			} else if(game.pickupState == PickupState.GRENADE_BOX){
				game.pickupState = PickupState.NONE;
				// add grenades to gun manager
				game.weapons.grenadeCount = 20;
				// hide grenades on scene
				GameObject grenadeBox = GameObject.Find("grenadeBoxOnGround");
				grenadeBox.SetActive(false);

			} else if(game.pickupState == PickupState.ARMOR){
				game.pickupState = PickupState.NONE;
				// apply armor
				game.statistics.playerArmor = 50;
				// hide armor on scene
				GameObject armor = GameObject.Find("armorOnGround");
				PlayerSounds.instance.PlayArmorPickupSound();
				armor.SetActive(false);
				// increase scar-L collider so it's easier to pick up
				GameObject scarL = GameObject.Find("pickup_scarl");
				BoxCollider scarCollider = scarL.GetComponent<BoxCollider>();
				scarCollider.size = new Vector3(2,2,2);
			}
		}
	}
	
}