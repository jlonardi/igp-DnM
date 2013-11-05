using UnityEngine;
using System.Collections;

public class EnemyLogic : MonoBehaviour {
	
	public int health = 100;
	
	//how long enemy chases player if player shoots
	public float focusTime = 10f;
	
	//how long enemy chases player if seen nearby
	public float nearbyTime = 5f;
	
	//distance where enemy detects player
	public float detectDistance = 6f;

	public bool attacking = false;
	public bool looting = false;
	public float attackInterval = 1f;
	public float lootInterval = 1f;
		
	private EnemyAI ai;
	private RagdollManager ragdolls;
	private Treasure treasure;
	private GameObject player;

	private focusTarget target;
	private float timeWhenFocusedPlayer = 0f;
	private float timeOfLastAction = 0f;	
	private int attacks = 0;
	private bool treasureAvailable = false;
	
	public void Start() {
		
		ai = GetComponent<EnemyAI>();
		ragdolls = GameObject.FindObjectOfType(typeof(RagdollManager)) as RagdollManager;
		treasure = GameObject.FindObjectOfType(typeof(Treasure)) as Treasure;

		player = GameObject.Find("Player");	
		target = focusTarget.PLAYER;
		ai.init(player.transform);
	}
	
	public void Update() {
		
		if(Input.GetKeyDown(KeyCode.T)) {
			swapTarget();
		}
		
		
		checkFocus();
		checkActions();
	}
	
	private void checkActions() {
		//The object is at target and ready to do some actions
		if(ai.isAtTarget()) {
			
			if(target == focusTarget.PLAYER) {
				if(!attacking) {
					attacking = true;
					Debug.Log("attacking set to true");
				}
				if((timeOfLastAction + attackInterval) < Time.time && !attacking) {
					
					//TODO attack the player
					
					timeOfLastAction = Time.time;
				}
			} 
			
			if(target == focusTarget.TRESAURE) {
				if(!looting) {
					looting = true;	
				}
				if((timeOfLastAction + attackInterval) < Time.time && !looting) {
					
					//TODO grab loot from the chest	

					timeOfLastAction = Time.time;
				}
			}
			
			
			

		} else {
			attacking = false;	
			looting = false;
		}

		//if(attacking && (timeOfLastAction + attackInterval) < Time.time) {
		//		attacking = false;	
		//}
			
		//if(looting && (timeOfLastAction + attackInterval) < Time.time) {
		//		looting = false;	
		//}
	}
	
	private void checkFocus() {
		if (!treasureAvailable){
			treasureAvailable = treasure.OnGround();
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
			if (Vector3.Distance(player.transform.position, transform.position)<= detectDistance){				
				float timeDifference = focusTime - nearbyTime;
				if (timeDifference < 0){
					timeDifference = 0;
				}
				// add timeDifference to timeWhenFocusedPlayer so that we actually check nearbyTime if enemy haven't been shot
				timeWhenFocusedPlayer = Time.time + timeDifference;
				swapTarget();
			}
		}
	}
	
	private void swapTarget() {
		Debug.Log("Swapping focus target");
		if(target == focusTarget.PLAYER) {
			ai.setTarget(treasure.gameObject);
			target = focusTarget.TRESAURE;
			Debug.Log("New target is tresaure");
			return;
		} 
		
		if (target == focusTarget.TRESAURE){
			ai.setTarget(player);
			target = focusTarget.PLAYER;
			Debug.Log("New target is player");
			return;
		}
	}
	
	// TakeDamage without adding force
	public void TakeDamage(int damageAmount, DamageType damageType){
		TakeDamage(damageAmount, damageType, new RaycastHit(), new Vector3(), 0f);
	}
	
	// TakeDamage with applying damage force
	public void TakeDamage(int damageAmount, DamageType damageType, RaycastHit hit, Vector3 direction, float power){
		Debug.Log("Hit detected");
		
		if (damageType == DamageType.BULLET) {
			health -= damageAmount;
		}
		
		if(health <= 0) {
			Die(hit, direction, power);
		}
		
		target = focusTarget.PLAYER;
		timeWhenFocusedPlayer = Time.time;
		ai.setTarget(player);
		
		Debug.Log("Enemy health left: " + health);
	}
	
	// enemy death without force
	public void Die(){
		
		Die(new RaycastHit(), new Vector3(), 0f);
	}
	
	// enemy death with force
	public void Die(RaycastHit hit, Vector3 direction, float power){
		//make enemy a ragdoll
		Rigidbody ragdollRigidBody = ragdolls.MakeRagdoll(this.gameObject);
		
		// apply impact to ragdoll
		if (power != 0f){
			ragdollRigidBody.AddForceAtPosition(direction.normalized * power *10, hit.point, ForceMode.Impulse);
		}
	}	
	
	public GameObject getTarget() {
		if(target == focusTarget.PLAYER) {
			return player;
		} else {
			return treasure.gameObject;
		}
	}
}
