using UnityEngine;
using System.Collections;

public class EnemyLogic : MonoBehaviour {
	public EnemyType enemyType = EnemyType.ORC;
	public int health = 100;
	public int damage = 7;
	public bool canLoot = true;
	public float lootInterval = 2f;
	public float attackDistance = 2.4f;
	public float lootDistance = 2.7f;

	public AudioClip[] painSounds;
	
	public bool attacking = false;
	public bool looting = false;

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

	private float timeFromAttack;
	private float timeFromLoot;
		
	private GameManager game;
	private RagdollManager ragdolls;
	private EnemyManager enemyManager;
	private EnemyNavigation navigation;
	
	private Transform playerTransform;
	private Transform treasureTransform;

	private focusTarget target;
	private float timeWhenFocusedPlayer = 0f;
	
	public void Start() {
		game = GameManager.instance;
		navigation = GetComponent<EnemyNavigation>();
		enemyManager = EnemyManager.instance;
		ragdolls = RagdollManager.instance;
//		playerVitals = Player.instance;

		GameObject player = GameObject.Find("playerFocus");	
		GameObject treasureObj = game.treasure.gameObject;
		GameObject focusPoint = treasureObj.transform.FindChild("treasureFocus").gameObject;
			
		treasureTransform = focusPoint.transform;
		playerTransform = player.transform;

		// if treasure is on ground and enemy can loot, set first target as treasure
		if (game.treasure.OnGround() && canLoot){
			target = focusTarget.TRESAURE;
			navigation.target = treasureTransform;
		} else {
			target = focusTarget.PLAYER;
			navigation.target = playerTransform;
		}

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
		
	private float GetDistance(Transform t){
		Vector3 from = t.position;
		Vector3 to = transform.position;
		// don't take elevation into account
		float x = Mathf.Abs(from.x - to.x);
		float z = Mathf.Abs(from.z - to.z);
		float distance = Mathf.Sqrt(x*x + z*z);
		//float distance = Vector3.Distance(new Vector3(from.x, 0, from.z),new Vector3(to.x, 0, to.z));
		return distance;
	}

	private void checkActions() {
		float playerDistance = GetDistance(playerTransform);
		float treasureDistance = GetDistance(treasureTransform);

		//The object is at target and ready to do some actions
		if(navigation.targetReached) {
			if(target == focusTarget.PLAYER) {
				if(!attacking && playerDistance <= attackDistance) {
					if(!attacking) {
						attacking = true;
					}
					if (playerDistance > attackDistance){
						attacking = false;
					}
				}
			} 
			
			if(target == focusTarget.TRESAURE) {
				if(!looting && treasureDistance <= lootDistance) {
					looting = true;	
				}
				if (treasureDistance > lootDistance){
					looting = false;
				}
				if(looting && (timeFromLoot + lootInterval) < Time.time) {					
					game.treasure.Loot(1);
					timeFromLoot = Time.time;
				}
			}
		} else {
			if (playerDistance > attackDistance){
				attacking = false;	
			}
			if (treasureDistance > lootDistance){
				looting = false;
			}
		}
	}
	
	private void checkFocus() {
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
		if(target == focusTarget.PLAYER  && canLoot) {
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
		
		//Debug.Log("Enemy health left: " + health);
	}

	// if any of the enemies attacked by player, check if player nearby and attack
	private void PlayerAttackingEnemy(Vector3 attackLocation){
		try{
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
		} catch {
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
		if(GetDistance(playerTransform) > attackDistance) {
			return;
		}

		int hitdamage = damage;
		if (message.Equals("standing")){
			hitdamage = damage+2;
		}
		game.player.TakeDamage(1, DamageType.HIT);	
		//playerVitals.TakeDamage(damage, DamageType.HIT);	
	}
}
