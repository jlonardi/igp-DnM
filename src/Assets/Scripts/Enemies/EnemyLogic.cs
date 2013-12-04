using UnityEngine;
using System.Collections;

public class EnemyLogic : MonoBehaviour {
	public EnemyType enemyType = EnemyType.ORC;
	
	public int health = 100;

	public int damage = 7;
	
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
	public float lootInterval = 2f;
	public float attackDistance = 2.4f;
	public float lootDistance = 2.7f;
	private float timeFromAttack;
	private float timeFromLoot;
	public bool canLoot = false;
	public bool canAttack = true;
		
	private GameManager game;
	private RagdollManager ragdolls;
	private EnemyManager enemyManager;
	private EnemyNavigation navigation;
	private Player playerVitals;

	private Treasure treasure;
	private Transform playerTransform;
	private Transform treasureTransform;

	private focusTarget target;
	private float timeWhenFocusedPlayer = 0f;
	
	private bool treasureAvailable = false;

	public AudioClip[] painSounds;

	public void Start() {
		navigation = GetComponent<EnemyNavigation>();
		enemyManager = EnemyManager.instance;
		ragdolls = RagdollManager.instance;
		playerVitals = Player.instance;

		treasure = Treasure.instance;
		GameObject player = GameObject.Find("Main Camera");	
		GameObject treasureObj = treasure.gameObject;
		GameObject focusPoint = treasureObj.transform.FindChild("focusPoint").gameObject;
			
		treasureTransform = focusPoint.transform;
		playerTransform = player.transform;
		target = focusTarget.PLAYER;
		navigation.target = playerTransform;
	}
	
	public void Update() {
		if (game == null){
			game = GameManager.instance;
		}

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
		if(navigation.targetReached) {
			if(target == focusTarget.PLAYER) {
				//if(!attacking && getPlayerDistance() <= attackDistance) {
				if(!attacking) {
					attacking = true;
				}
 				if (getPlayerDistance() > attackDistance){
					attacking = false;
				}
//				if(attacking && (timeFromAttack + attackInterval) < Time.time) {
//					timeFromAttack = Time.time;
//				}
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
			if (treasureAvailable && canLoot){
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
			navigation.target = treasureTransform;
			target = focusTarget.TRESAURE;
			//Debug.Log("New target is tresaure");
			return;
		} 
		
		if (target == focusTarget.TRESAURE){
			navigation.target = playerTransform;
			target = focusTarget.PLAYER;
			//Debug.Log("New target is player");
			return;
		}
	}
	
	// TakeDamage without aplying force
	public void TakeDamage(int damageAmount){
		TakeDamage(damageAmount, new RaycastHit(), new Vector3(), 0f);
	}

	// TakeDamage and apply explosive force
	public void TakeDamage(int damageAmount, Vector3 explosionPosition, float power){
		AlertNearbyEnemies();

		health -= damageAmount;
		if(health <= 0) {
			Rigidbody deadBody = MakeDead();
			deadBody.AddExplosionForce(power, explosionPosition, 10f, 2.0f);
		}		
		AfterTakeDamage();
	}

	// TakeDamage and apply damage force
	public void TakeDamage(int damageAmount, RaycastHit hit, Vector3 direction, float power){
		AlertNearbyEnemies();

		health -= damageAmount;
		if(health <= 0) {
			Rigidbody deadBody = MakeDead();
			deadBody.AddForceAtPosition(direction.normalized * power * 11, hit.point, ForceMode.Impulse);
		}
		AfterTakeDamage();
	}

	// alert nearby enemies about hostile player
	private void AlertNearbyEnemies(){
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
		if (enemies.Length!=0) {
			foreach (GameObject enemy in enemies) {
				enemy.SendMessage("PlayerAttackingEnemy", this.transform.position, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	// turn living into dead
	public Rigidbody MakeDead(){
		game.statistics.AddKillStats(this.enemyType);
		enemyManager.currentEnemyCount--;
		
		// actually make enemy a ragdoll
		GameObject ragdoll = ragdolls.MakeRagdoll(enemyType, this.gameObject, true);
		return ragdoll.GetComponentInChildren(typeof(Rigidbody)) as Rigidbody;		
	}

	// run this after taking a shot or explosive damage
	public void AfterTakeDamage(){
		PlaySound(painSounds);
		
		target = focusTarget.PLAYER;
		timeWhenFocusedPlayer = Time.time;
		navigation.target = playerTransform;
		
		Debug.Log("Enemy health left: " + health);
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

	private void AttackTrigger(string message){
		if(getPlayerDistance() > attackDistance) {
			return;
		}

		int hitdamage = damage;
		if (message.Equals("standing")){
			hitdamage = damage+2;
		}
		playerVitals.TakeDamage(1, DamageType.HIT);	
		//playerVitals.TakeDamage(damage, DamageType.HIT);	
	}
}
