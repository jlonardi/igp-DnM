using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	public int health = 100;
	public float armor = 0.0f; // armor scale 0-1.0f
	private GameManager gameManager;
		
	void Start(){
		gameManager = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.K)) {
			TakeDamage(20, DamageType.HIT);
		}
	}
	
	public void TakeDamage(int damageAmount, DamageType damageType){
		//if player has an armor, take less damage
		float tempHealth = health - (damageAmount - (damageAmount*armor));
		
		if (tempHealth <= 0){
			health = 0;
			gameManager.GameOver();	
		} else {
			health = (int)Mathf.Round(tempHealth);
		}
	}	
}
