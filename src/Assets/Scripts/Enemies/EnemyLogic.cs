using UnityEngine;
using System.Collections;

public class EnemyLogic : MonoBehaviour {
	public int health = 100;
	public GameObject ragdoll;	
	
	public void TakeDamage(int damageAmount, DamageType damageType){
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
		Ragdoll r = (Instantiate(ragdoll, this.transform.position,this.transform.rotation) as GameObject).GetComponent<Ragdoll>();
		r.CopyPose(this.gameObject.transform);
		Destroy(this.gameObject);
	}
	
}
