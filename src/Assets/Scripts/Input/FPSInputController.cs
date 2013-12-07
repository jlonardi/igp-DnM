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
	private GunManager gunManager;

	// Use this for initialization
	void Start () {
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

		bool treasureOnGround = game.treasure.OnGround();

		// Get the input vector from kayboard or analog stick
		Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
//		if (directionVector == Vector3.zero) {
//			directionVector = new Vector3(Input.GetAxis("Joy X"), 0f, Input.GetAxis("Joy Y"));
//		}


/*		if (Vector3.Distance(directionVector, Vector3.zero) > 0.1f){
			game.statistics.playerMoving = true;
		} else {
			game.statistics.playerMoving = false;
		}
*/
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

		int currentGunIndex = gunManager.currentGunIndex;

		if (Input.GetAxis("Mouse ScrollWheel") < 0){
			while (true){
				currentGunIndex++;
				if (currentGunIndex>4){
					currentGunIndex = 0;
				}
				if (gunManager.guns[currentGunIndex].picked_up){
					break;
				}
			}
			gunManager.ChangeToGun(currentGunIndex);
		} else if (Input.GetAxis("Mouse ScrollWheel") > 0){
			while (true){
				currentGunIndex--;
				if (currentGunIndex<0){
					currentGunIndex = 4;
				}
				if (gunManager.guns[currentGunIndex].picked_up){
					break;
				}
			}
			gunManager.ChangeToGun(currentGunIndex);
		}

		if (Input.GetButtonDown("Pistol")){
			gunManager.ChangeToGun(0);
		}
		if (Input.GetButtonDown("Assault Rifle")){
			gunManager.ChangeToGun(1);
		}
		if (Input.GetButtonDown("Grenade Launcher")){
			gunManager.ChangeToGun(2);
		}
		if (Input.GetButtonDown("Minigun")){
			gunManager.ChangeToGun(3);
		}
		if (Input.GetButtonDown("Scar-L")){
			gunManager.ChangeToGun(4);
		}

		if (Input.GetButton("Sprint") && !treasureOnGround){
			motor.StartSprint();
		}


		//Check if the user if firing the weapon
		fire = Input.GetButton("Fire") && treasureOnGround && GunManager.instance.currentGun.freeToShoot;

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
			gunManager.ThrowGrenade();
		}

//		firing = (firingTimer <= 0.0f && fire);
		
		if(treasureOnGround){
			GunManager.instance.currentGun.fire = firing;
			reloading = GunManager.instance.currentGun.reloading;
			currentWeaponName = GunManager.instance.currentGun.name;
			currentWeapon = GunManager.instance.currentGunIndex;
		}
		
		
		if (Input.GetButtonDown("Use")){
			if (!treasureOnGround){
				game.treasure.SetTreasureOnGround();
				game.pickupState = PickupState.TREASURE;

			} else if(game.pickupState == PickupState.TREASURE){
				game.treasure.CarryTreasure();
				game.pickupState = PickupState.NONE;

				// if sprinting, stop it while carrying the treasure
				if (motor.sprinting){
					motor.StopSprint();
				}

				// disable gun so we can carry treasure
				gunManager.currentGun.enabled = false;
				// find treasure positions from scene
				GameObject treasureBox = GameObject.Find("treasure_box");
				treasureBox.collider.isTrigger = true;
				GameObject treasureOnPlayer = GameObject.Find("Treasure");
				// change parent to players Treasure-object
				treasureBox.transform.parent = treasureOnPlayer.transform;
				// and change local position & rotation back to start values
				treasureBox.transform.localPosition = new Vector3(0,0,-1.28f);
				treasureBox.transform.localRotation = Quaternion.identity;

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

			} else if(game.pickupState == PickupState.GRENADE_BOX){
				game.pickupState = PickupState.NONE;
				// add grenades to gun manager
				gunManager.grenadeCount = 20;
				// hide grenades on scene
				GameObject grenadeBox = GameObject.Find("grenadeBoxOnGround");
				grenadeBox.SetActive(false);

			} else if(game.pickupState == PickupState.MINIGUN){
				game.pickupState = PickupState.NONE;
				// set minigun available
				gunManager.guns[3].picked_up = true;
				gunManager.ChangeToGun(3);
				PlayerSounds.instance.PlayGunPickupSound();
				// hide minigun on scene
				GameObject minigun = GameObject.Find("minigunOnGround");
				minigun.SetActive(false);

			} else if(game.pickupState == PickupState.SCAR_L){
				game.pickupState = PickupState.NONE;
				// set minigun available
				gunManager.guns[4].picked_up = true;
				gunManager.ChangeToGun(4);
				PlayerSounds.instance.PlayGunPickupSound();
				// hide scar-L on scene
				GameObject scarL = GameObject.Find("scarlOnGround");
				scarL.SetActive(false);

			}
			
		}

	}
}