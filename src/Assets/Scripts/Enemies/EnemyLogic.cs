using UnityEngine;
using System.Collections;

public enum DamageType {
	BULLET,
	FIRE,
	EXPLOSION,
}

public class EnemyLogic : MonoBehaviour {
	public int health = 100;
	public GameObject ragdoll;	
	
	public void HitDamage(int damageAmount, DamageType damageType){
		Debug.Log("Hit detected");
		
		if (damageType == DamageType.BULLET) {
			health -= damageAmount;
		}
		
		if(health <= 0) {
			Die();
		}
		Debug.Log("Enemy health left: " + health);
	}
	
	public void Die(){
		GameObject killObj = gameObject;
		Ragdoll r = (Instantiate(ragdoll, killObj.transform.position,killObj.transform.rotation) as GameObject).GetComponent<Ragdoll>();		
		Destroy(gameObject);
	}
	
}
