using UnityEngine;
using System.Collections;

public class EnemyLogic : MonoBehaviour {
	
	
	public int health = 100;
	
	void Start () {
	
	}
	
	void Update () {
	
	}
	
	void Hit(RaycastHit hit) {
		Debug.Log("Hit detected");
		//Debug.Log("Distance was " + hit.distance);
		if(hit.distance > 10) {
			health -= 20;	
		} else if(hit.distance > 6) {
			health -= 35;		
		} else {
			health -= 50;
		}
		if(health <= 0) {
			Destroy(gameObject);
		}
		Debug.Log("Enemy health left: " + health);
	}
}
