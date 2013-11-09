using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	//use singleton since only we need once instance of this class
	public static PlayerHealth instance;
    public void Awake()
    {
        PlayerHealth.instance = this;
    }	
	
	public int health = 100;
	public float armor = 0.0f; // armor scale 0-1.0f
		
	void Start(){
		// workaround if we are testing a scene directly in editor and not via Main Menu
        GameManager.instance.NewGame();
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.K)) {
			TakeDamage(20, DamageType.HIT);
		}
	}
	
	public void TakeDamage(int damageAmount, DamageType damageType){
		//if player has an armor, take less damage
		float tempHealth = health - (damageAmount - (damageAmount*armor));
		if(!Treasure.instance.onGround){
			Treasure.instance.SetTreasureOnGround();
		}
		if (tempHealth <= 0){
			health = 0;
			GameManager.instance.GameOver();	
		} else {
			health = (int)Mathf.Round(tempHealth);
		}
	}	
}
