using UnityEngine;
using System.Collections;

public class EnemyLogic : MonoBehaviour {
	
	public int health = 100;
	public GameObject ragdoll;	
	public float focusTime = 3f;
		
	private bool attacking;
	private bool looting;
	
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
			
			if(target == focusTarget.PLAYER) {
				float attackInterval = 1f;
				if(timeSinceLastAction + attackInterval < Time.time) {
					//TODO attack the player
				}	
			}
			
			if(target == focusTarget.TRESAURE) {
				float lootInterval = 1f;
				if(timeSinceLastAction + lootInterval < Time.time) {
					//TODO grab loot from the chest	
				}
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
	
	public void TakeDamage(int damageAmount, DamageType damageType){
		Debug.Log("Hit detected");
		
		if (damageType == DamageType.BULLET) {
			health -= damageAmount;
		}
		
		if(health <= 0) {
			Die();
		}
		
		target = focusTarget.PLAYER;
		timeWhenFocusedPlayer = Time.time;
		ai.setTarget(player);
		
		Debug.Log("Enemy health left: " + health);
	}
	
	public void Die(){		
		Ragdoll r = (Instantiate(ragdoll, this.transform.position,this.transform.rotation) as GameObject).GetComponent<Ragdoll>();
		r.CopyPose(this.gameObject.transform);
		Destroy(this.gameObject);
	}
	
}
