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
	public float prevJoyTrigger;

	[HideInInspector]
	public string currentWeaponName;

	//time variable to help hit the corner direction on gamepad
	private	float timeOfUpLeft;

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
		if(game.gameState != GameState.RUNNING || game.player.GetAliveStatus() == false){
			return;
		}

		//get joystick values
		float JoyAxis3 = Input.GetAxis("Fire/Aim Down Sight");
		float JoyPadX = Input.GetAxis("Inventory Horizontal");
		float JoyPadY = Input.GetAxis("Inventory Vertical");
		
		bool JoyFire = false;
		bool JoyFireDown = false;
		bool JoyFireUp = false;
		bool JoyAimDown = false;
		bool JoyPadUp = false;
		bool JoyPadDown = false;
		bool JoyPadLeft = false;
		bool JoyPadRight = false;
		bool JoyPadUpLeft = false;
		
		if (JoyAxis3 > 0.5f){
			JoyFire = true;
		} else if (JoyAxis3 < -0.5f){
			JoyAimDown = true;
		}

		if (prevJoyTrigger > 0.5f && JoyAxis3 < 0.5f){
			JoyFire = false;
			JoyFireUp = true;
		}
		if (prevJoyTrigger < 0.5f && JoyAxis3 > 0.5f){
			JoyFire = true;
			JoyFireDown = true;
		}

		prevJoyTrigger = JoyAxis3;
		if (JoyPadX > 0.5f && JoyPadY < -0.5f){
			JoyPadUpLeft = true;
			timeOfUpLeft = Time.timeSinceLevelLoad;

		} else if (JoyPadX > 0.5f && JoyPadY < 0.3f && timeOfUpLeft + 0.5f < Time.timeSinceLevelLoad){
			JoyPadUp = true;
		} else if (JoyPadX < -0.5f && JoyPadY < 0.3f && timeOfUpLeft + 0.5f < Time.timeSinceLevelLoad){
			JoyPadDown = true;
		}

		if (JoyPadY > 0.5f && JoyPadX < 0.3f){
			JoyPadRight = true;
		} else if (JoyPadY < -0.5f && JoyPadX < 0.3f){
			JoyPadLeft = true;
		}

		bool GetFireButton = Input.GetButton("Fire") || JoyFire;
		bool GetFireButtonDown = Input.GetButtonDown("Fire") || JoyFireDown;
		bool GetFireButtonUp = Input.GetButtonUp("Fire") || JoyFireUp;
		

		bool treasureOnGround = game.treasure.OnGround();

		// Get the input vector from kayboard
		Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

		//if no input, stop sprint
		if (directionVector == Vector3.zero) {
			motor.StopSprint();
		}

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

		Gun currentGun = game.weapons.currentGun;
		int currentGunIndex = game.weapons.currentGunIndex;

		if (Input.GetButton("Reload") && !currentGun.reloading){
			currentGun.Reload();
		}

		if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetButtonDown("Switch Weapon")){
			game.weapons.ChangeToNextGun();
		} else if (Input.GetAxis("Mouse ScrollWheel") > 0){
			game.weapons.ChangeToPreviousGun();
		}

		if (Input.GetButtonDown("Pistol") || JoyPadDown){
			game.weapons.ChangeToGun(0);
		}
		if (Input.GetButtonDown("Assault Rifle") || JoyPadUp){
			game.weapons.ChangeToGun(1);
		}
		if (Input.GetButtonDown("Grenade Launcher") || JoyPadRight){
			game.weapons.ChangeToGun(2);
		}
		if (Input.GetButtonDown("Minigun") || JoyPadLeft){
			game.weapons.ChangeToGun(3);
		}
		if (Input.GetButtonDown("Scar-L") || JoyPadUpLeft){
			game.weapons.ChangeToGun(4);
		}

		if (Input.GetButtonDown("Sprint") && treasureOnGround && GetFireButton == false){
			motor.StartSprint();
		}

		if (GetFireButtonDown && currentGun.currentRounds == 0 && !currentGun.reloading && currentGun.freeToShoot){		
			currentGun.PlayOutOfAmmoSound();
		}
		
		if(GetFireButtonUp && game.player.GetAliveStatus()){
			currentGun.freeToShoot = true;
		}

		//Check if the user if firing the weapon
		if (GetFireButton && treasureOnGround && game.weapons.currentGun.freeToShoot){
			fire = true;
		} else {
			fire = false;
		}

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
				game.pickupState = PickupState.NONE;
				game.treasure.CarryTreasure();

			} else if(game.pickupState == PickupState.MINIGUN){
				game.pickupState = PickupState.NONE;
				game.weapons.guns[3].PickUp();
				game.weapons.ChangeToGun(3);
				// hide minigun on scene
				GameObject minigun = GameObject.Find("minigunOnGround");
				minigun.SetActive(false);
				
			} else if(game.pickupState == PickupState.SCAR_L){
				game.pickupState = PickupState.NONE;
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
				game.statistics.armorPickedUp = true;
				// apply armor
				game.player.SetArmor(50);
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