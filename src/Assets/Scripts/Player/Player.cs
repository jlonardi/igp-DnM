using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private float health = 100f;
	private float armor; // armor scale 0-50
	private float fireDamage = 1f;
	private float enemyDamage = 1f;
	private GameManager game;
	private OnGuiManager guiManager;
	private PlayerSounds sounds;
	private bool isAlive = true;
	private float timeSinceKilled;
	//how long it takes for player to die after final hit (s)
	private float deathDuration = 2.5f;
	public float speed;
	public FPSInputController inputController = new FPSInputController();


	[HideInInspector]
	public CharacterMotor motor;


    void Start() {
		game = GameManager.instance;
		guiManager = OnGuiManager.instance;
		motor = GetComponent<CharacterMotor>();
		sounds = PlayerSounds.instance;
	}	

	void Update(){
		if (!isAlive && timeSinceKilled + deathDuration < Time.timeSinceLevelLoad){
			game.GameOver();
		}
		//update inputs
		inputController.UpdateInput();
		inputController.UpdateRumble();
	}

	public void TakeDamage(float damageAmount, DamageType damageType){
		//if dead already, do nothing
		if (!isAlive){
			return;
		}

		damageAmount *= enemyDamage;

		float tempArmor = 0;
		float tempHealth = 0;

		//handle fire damage here
		if (damageType == DamageType.FIRE){
			//if player has an armor, firedamage is 50% less
			tempArmor = armor - (damageAmount*0.5f);
			if (tempArmor<0){
				armor = 0;
				// amount without armor is 100% of firedamage
				tempHealth = health + (tempArmor*2);
			} else {
				tempHealth = health;
				armor = tempArmor;
			}
		}

		//handle enemy hits and everything but firedamage here
		if (damageType != DamageType.FIRE){
			//if player has an armor, take less damage
			tempArmor = armor - damageAmount;
			if (tempArmor<0){
				armor = 0;
				tempHealth = health + tempArmor;
			} else {
				tempHealth = health;
				armor = tempArmor;
			}
		}

		// if enemy hits player, treasure drops
		if(!game.treasure.OnGround()){
			game.treasure.SetTreasureOnGround();
		}

		// visualize the pain
		guiManager.bloodSplatter.setSplatterVisible( (1f-(tempHealth/100f)));


		if (tempHealth <= 0){
			health = 0;
			isAlive = false;
			timeSinceKilled = Time.timeSinceLevelLoad;
			sounds.PlayDeathSound();

			//move player so that it doesn't drop through terrain
			transform.position += Vector3.up*10;
			RaycastHit hit = new RaycastHit();		
			if (Physics.Raycast(transform.position, -Vector3.up, out hit)){
				// align just above terrain		    
				transform.position = new Vector3(transform.position.x, transform.position.y - hit.distance + 0.05f, 
				                                 	transform.position.z);
			}
			//move camera so it's closer to ground and rotate it
			Transform cameraTransform = GameObject.Find("Main Camera").transform;
			cameraTransform.eulerAngles = new Vector3(20,0,90);
		} else {
			health = tempHealth;
			sounds.PlayPainSound();
		}
	}
	
	public float GetHealth(){
		return health;
	}
	
	public void SetHealth(float value){
		health = value;
	}
	
	public float GetArmor(){
		return armor;
	}
	
	public void SetArmor(float value){
		armor = value;
	}

	public void SetDeathDuration(float value){
		deathDuration = value;
	}

	public bool GetAliveStatus(){
		return isAlive;
	}
	
	public void SetAliveStatus(bool value){
		isAlive = value;
	}

	public float GetFireDamage(){
		return fireDamage;
	}
	
	public void SetFireDamage(float value){
		fireDamage = value;
	}

	public float GetEnemyDamage(){
		return enemyDamage;
	}
	
	public void SetEnemyDamage(float value){
		enemyDamage = value;
	}
}
