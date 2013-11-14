using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	//use singleton since only we need once instance of this class
	public static PlayerHealth instance;

	private GameManager game;

    public void Awake()
    {
        PlayerHealth.instance = this;
    }	

	void Update(){
		if (game == null){
			game = GameManager.instance;
		}

		if(Input.GetKeyDown(KeyCode.K)) {
			TakeDamage(20, DamageType.HIT);
		}
	}
	
	public void TakeDamage(int damageAmount, DamageType damageType){
		//if player has an armor, take less damage
		float tempHealth = game.statistics.playerHealth - (damageAmount - (damageAmount * game.statistics.playerArmor));
		if(GameManager.instance.treasureState == TreasureState.CARRYING){
			GameManager.instance.treasureState = TreasureState.SET_ON_GROUND;
		}
		if (tempHealth <= 0){
			game.statistics.playerHealth = 0;
			game.GameOver();	
		} else {
			game.statistics.playerHealth = (int)Mathf.Round(tempHealth);
		}
	}	
}
