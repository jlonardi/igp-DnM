using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	public int health = 100;
	private Hud hud;
	
	void Start() {
		hud = GameObject.Find("Game Manager").GetComponent<Hud>();
	}
	
	void Update() {
		hud.setHealth(health);
	}
	
	public void TakeDamage(int damageAmount, DamageType damageType){
		health -= damageAmount;
	}	
}
