using UnityEngine;
using System.Collections;

public class EnemyLogic : MonoBehaviour {
	
	public int health = 100;
	public GameObject ragdoll;	
	public float focusTime = 10f;
	public bool attacking = false;
	public bool looting = false;
	
	private EnemyAI ai;
	private GameObject playerObject;
	private Transform player;
	private Transform tresaure;
	private focusTarget target;
	private float timeWhenFocusedPlayer = 0f;
	private float timeSinceLastAction = 0f;
	
	public void Start() {
		
		ai = GetComponent<EnemyAI>();
		GameObject p = GameObject.Find("Player");
		player = p.transform;
		GameObject a = GameObject.Find("arkku");
		tresaure = a.transform;
		
		target = focusTarget.TRESAURE;
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
			
			float attackInterval = 1f;
			float lootInterval = 1f;
			
			if(target == focusTarget.PLAYER) {
				
				if(timeSinceLastAction + attackInterval < Time.time) {
					attacking = true;
					//TODO attack the player
				}	
			}
			
			if(target == focusTarget.TRESAURE) {
				
				if(timeSinceLastAction + lootInterval < Time.time) {
					looting = true;
					//TODO grab loot from the chest	
				}
			}
			
			if(attacking && timeSinceLastAction + attackInterval < Time.time) {
				attacking = false;	
			}
			
			if(looting && timeSinceLastAction + lootInterval < Time.time) {
				looting = false;	
			}
			
			timeSinceLastAction = Time.deltaTime;
		}
	}
	
	private void checkFocus() {
		if(target == focusTarget.PLAYER) {
			//If the focus time expires change back to focus the tresaure
			if(timeWhenFocusedPlayer + focusTime < Time.time) {
				swapTarget();
			}
		}
	}
	
	private void swapTarget() {
		Debug.Log("Swapping focus target");
		if(target == focusTarget.PLAYER) {
			ai.setTarget(tresaure);
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
		Ragdoll r = (Instantiate(ragdoll, this.transform.position,this.transform.rotation) as GameObject).GetComponent<Ragdoll>();
		r.CopyPose(this.gameObject.transform);
		Destroy(this.gameObject);		
		
		// apply impact to ragdoll
		if (power != 0f){
			Rigidbody rb = r.GetComponentInChildren(typeof(Rigidbody)) as Rigidbody;
			rb.AddForceAtPosition(direction.normalized * power *10, hit.point, ForceMode.Impulse);
		}
	}	
}
