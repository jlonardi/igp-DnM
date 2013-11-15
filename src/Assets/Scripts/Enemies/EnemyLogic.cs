using UnityEngine;
using System.Collections;

public class EnemyLogic : MonoBehaviour {
	public EnemyType enemyType = EnemyType.ORC;
	
	public int health = 100;
	
	//how long enemy chases player if player shoots (s)
	private float focusTime = 20f;
	
	//how long enemy chases player if seen nearby
	private float nearbyTime = 10f;
	
	//distance where enemy detects player
	private float detectDistance = 6f;
	
	//distance where other enemies may join the chase for hostile player
	private float joinAttackDistance = 20f;
	
	//how greedy enemy is will determine the chance of other enemies to join the chase for hostile player (0-1.0)
	private float greedyness = 0.4f; 

	public bool attacking = false;
	public bool looting = false;
	public float attackInterval = 2f;
	public float lootInterval = 2f;
	private float attackDistance = 2.4f;
	private float lootDistance = 2.7f;
	private float timeFromAttack;
	private float timeFromLoot;
		
	private EnemyMovement enemyMovement;

	private Treasure treasure;
	private Transform playerTransform;
	private Transform treasureTransform;

	private focusTarget target;
	private float timeWhenFocusedPlayer = 0f;
	
	private bool treasureAvailable = false;
	private GameManager game;
	private RagdollManager ragdolls;
	private EnemyManager enemyManager;

	private PlayerHealth playerVitals;

	public AudioClip[] painSounds;
	
	public void Start() {
		enemyMovement = GetComponent<EnemyMovement>();
		game = GameManager.instance;
		playerVitals = PlayerHealth.instance;
		ragdolls = RagdollManager.instance;
		treasure = Treasure.instance;
		enemyManager = EnemyManager.instance;

		GameObject player = GameObject.Find("Player");	
		GameObject focusPoint = treasure.gameObject.transform.FindChild("focusPoint").gameObject;

		playerTransform = player.transform;
		treasureTransform = focusPoint.transform;

		target = focusTarget.PLAYER;
		enemyMovement.init(playerTransform);
	}
	
	public void Update() {
		
		if(Input.GetKeyDown(KeyCode.T)) {
			swapTarget();
		}
		
		
		checkFocus();
		checkActions();
	}
		
	
	private float getPlayerDistance(){
		return Vector3.Distance(playerTransform.position, transform.position);
	}
	private float getTreasureDistance(){
		return Vector3.Distance(treasureTransform.position, transform.position);
	}
	
	private void checkActions() {
		//The object is at target and ready to do some actions
		if(enemyMovement.atTarget) {
			if(target == focusTarget.PLAYER) {
				if(!attacking && getPlayerDistance() <= attackDistance) {
					attacking = true;
					Debug.Log("attacking set to true");
				}
 				if (getPlayerDistance() > attackDistance){
					attacking = false;
				}
				if(attacking && (timeFromAttack + attackInterval) < Time.time) {
					playerVitals.TakeDamage(1, DamageType.HIT);
					timeFromAttack = Time.time;
				}
			} 
			
			if(target == focusTarget.TRESAURE) {
				if(!looting && getTreasureDistance() <= lootDistance) {
					looting = true;	
				}
 				if (getTreasureDistance() > lootDistance){
					looting = false;
				}
				if(looting && (timeFromLoot + lootInterval) < Time.time) {					
					treasure.Loot(1);
					timeFromLoot = Time.time;
				}
			}
		} else {
			attacking = false;	
			looting = false;
		}
	}
	
	private void checkFocus() {
		if (!treasureAvailable){
			if (game.treasureState == TreasureState.SET_ON_GROUND){
				treasureAvailable = true;
			}
			if (treasureAvailable){
				swapTarget();
			}
		}
		if(target == focusTarget.PLAYER) {
			//If the focus time expires change back to focus the tresaure
			if(timeWhenFocusedPlayer + focusTime < Time.time) {
				swapTarget();
			}
		} else { // if focus not in player, check if player nearby
			if (Vector3.Distance(playerTransform.position, transform.position)<= detectDistance){				
				// add timeDifference to timeWhenFocusedPlayer so that we actually check nearbyTime if enemy haven't been shot
				float timeDifference = focusTime - nearbyTime;
				if (timeDifference < 0){
					timeDifference = 0;
				}
				timeWhenFocusedPlayer = Time.time + timeDifference;
				swapTarget();
			}
		}
	}
	
	private void swapTarget() {
		//Debug.Log("Swapping focus target");
		if(target == focusTarget.PLAYER) {
			enemyMovement.setTarget(treasureTransform);
			target = focusTarget.TRESAURE;
			//Debug.Log("New target is tresaure");
			return;
		} 
		
		if (target == focusTarget.TRESAURE){
			enemyMovement.setTarget(playerTransform);
			target = focusTarget.PLAYER;
			//Debug.Log("New target is player");
			return;
		}
	}
	
	// TakeDamage without adding force
	public void TakeDamage(int damageAmount, DamageType damageType){
		TakeDamage(damageAmount, damageType, new RaycastHit(), new Vector3(), 0f);
	}

	// TakeDamage from explosion
	public void TakeDamage(int damageAmount, Vector3 explosionPosition, float power){
		Debug.Log("Explosion detected");
		
		// alert nearby enemies about hostile player
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
		if (enemies.Length!=0) {
			foreach (GameObject enemy in enemies) {
				enemy.SendMessage("PlayerAttackingEnemy", this.transform.position, SendMessageOptions.DontRequireReceiver);
			}
		}

		health -= damageAmount;

		if(health <= 0) {
			Die(explosionPosition, power);
		}
		
		PlaySound(painSounds);
		
		target = focusTarget.PLAYER;
		timeWhenFocusedPlayer = Time.time;
		enemyMovement.setTarget(playerTransform);
		
		Debug.Log("Enemy health left: " + health);
	}

	// TakeDamage with applying damage force
	public void TakeDamage(int damageAmount, DamageType damageType, RaycastHit hit, Vector3 direction, float power){
		Debug.Log("Hit detected");

		// alert nearby enemies about hostile player
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
		if (enemies.Length!=0) {
			foreach (GameObject enemy in enemies) {
				enemy.SendMessage("PlayerAttackingEnemy", this.transform.position, SendMessageOptions.DontRequireReceiver);		
			}
		}
		
		
		if (damageType == DamageType.BULLET) {
			health -= damageAmount;
		}
		
		if(health <= 0) {
			Die(hit, direction, power);
		}

		PlaySound(painSounds);
		
		target = focusTarget.PLAYER;
		timeWhenFocusedPlayer = Time.time;
		enemyMovement.setTarget(playerTransform);
		
		Debug.Log("Enemy health left: " + health);
	}
	
	// enemy death without force
	public void Die(){
		
		Die(new RaycastHit(), new Vector3(), 0f);
	}
	
	// enemy death with force from explosion
	public void Die(Vector3 explosionPosition, float power){
		game.statistics.AddKillStats(this.enemyType);
		enemyManager.currentEnemyCount--;
		
		//make enemy a ragdoll
		GameObject ragdoll = ragdolls.MakeRagdoll(enemyType, this.gameObject, true);
		Rigidbody ragdollRigidBody = ragdoll.GetComponentInChildren(typeof(Rigidbody)) as Rigidbody;		
		
		ragdollRigidBody.AddExplosionForce(power, explosionPosition, 10f, 3.0f);
		Debug.Log("Explosion position: " +explosionPosition+ ", Enemy position: " + this.transform.position + ", Explosion power: " +power);
	}	

	// enemy death with force from gunshot
	public void Die(RaycastHit hit, Vector3 direction, float power){
		game.statistics.AddKillStats(this.enemyType);
		
		//make enemy a ragdoll
		GameObject ragdoll = ragdolls.MakeRagdoll(enemyType, this.gameObject, true);
		Rigidbody ragdollRigidBody = ragdoll.GetComponentInChildren(typeof(Rigidbody)) as Rigidbody;		
		
		// apply impact to ragdoll
		if (power != 0f){
			ragdollRigidBody.AddForceAtPosition(direction.normalized * power *10, hit.point, ForceMode.Impulse);
		}
	}	

	
	// if any of the enemies attacked by player, check if player nearby and attack
	private void PlayerAttackingEnemy(Vector3 attackLocation){
		float distanceToAttact = Vector3.Distance(attackLocation, this.transform.position);
		float distanceToTreasure = Vector3.Distance(treasureTransform.position, this.transform.position);
		if (distanceToAttact != 0f && distanceToAttact <= joinAttackDistance){
			// if not near the treasure, just join the chase
			// else use greedyness to choose whether to chase the player or not
			if ((distanceToTreasure > 10f) ||(Random.Range(0f,1f) <= greedyness)){
				timeWhenFocusedPlayer = Time.time;
				if (target == focusTarget.TRESAURE){
					swapTarget();
				} 			
			}
		}
	}

	private void PlaySound(AudioClip clip)
	{
		if (!audio.isPlaying)
		{
			audio.clip = clip;
			audio.Play();
		}
	}

	private void PlaySound(AudioClip[] clips)
	{
		int clipNum = Random.Range(0, clips.Length - 1);
		PlaySound(clips[clipNum]);
	}
}
