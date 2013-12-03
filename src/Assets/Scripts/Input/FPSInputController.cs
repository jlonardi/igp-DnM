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
		
		// Get the input vector from kayboard or analog stick
		Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

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
		if (Input.GetButton("Grenade")){
			gunManager.ThrowGrenade();
		}

//		firing = (firingTimer <= 0.0f && fire);
		
		if(game.treasureState == TreasureState.SET_ON_GROUND){
			GunManager.instance.currentGun.fire = firing;
			reloading = GunManager.instance.currentGun.reloading;
			currentWeaponName = GunManager.instance.currentGun.name;
			currentWeapon = GunManager.instance.currentGunIndex;
		}
		
		
		if (Input.GetButton("Use")){
			if (game.treasureState == TreasureState.CARRYING){
				game.treasureState = TreasureState.SET_ON_GROUND;
				game.pickupState = PickupState.TREASURE;

			} else if(game.pickupState == PickupState.ARMOR){
				game.pickupState = PickupState.NONE;
				// apply armor
				game.statistics.playerArmor = 0.5f;
				// hide armor on scene
				GameObject armor = GameObject.Find("armorOnGround");
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
				gunManager.guns[3].gun.picked_up = true;
				gunManager.ChangeToGun(3);
				// hide minigun on scene
				GameObject minigun = GameObject.Find("minigunOnGround");
				minigun.SetActive(false);

			} else if(game.pickupState == PickupState.SCAR_L){
				game.pickupState = PickupState.NONE;
				// set minigun available
				gunManager.guns[4].gun.picked_up = true;
				gunManager.ChangeToGun(4);
				// hide scar-L on scene
				GameObject scarL = GameObject.Find("scarlOnGround");
				scarL.SetActive(false);

			}
			
		}

	}
}