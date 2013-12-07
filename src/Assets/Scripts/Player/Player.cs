using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	//use singleton since only we need one instance of this class
	public static Player instance;

	private GameManager game;
	private OnGuiManager guiManager;
	private PlayerSounds sounds;
	[HideInInspector]
	public CharacterMotor motor;

    public void Awake() {
        Player.instance = this;
		motor = GetComponent<CharacterMotor>();
    }	

	void Update(){
		if (game == null){
			game = GameManager.instance;
		}
		if (guiManager == null){
			guiManager = OnGuiManager.instance;
		}


		if (sounds == null){
			sounds = PlayerSounds.instance;
		}
		if(Input.GetKeyDown(KeyCode.K)) {
			TakeDamage(20, DamageType.HIT);
		}
	}
	
	public void TakeDamage(int damageAmount, DamageType damageType){
		//if player has an armor, take less damage
		int tempArmor = game.statistics.playerArmor - damageAmount;
		int tempHealth = game.statistics.playerHealth;

		if (tempArmor<0){
			game.statistics.playerArmor = 0;
			tempHealth = game.statistics.playerHealth + tempArmor;
		} else {
			game.statistics.playerArmor = tempArmor;
		}

		// if enemy hits player, treasure drops
		if(!game.treasure.OnGround()){
			game.treasure.SetTreasureOnGround();
		}

		// visualize the pain
		guiManager.bloodSplatter.setSplatterVisible( (1f-(tempHealth/100f)));

		if (tempHealth <= 0){
			game.statistics.playerHealth = 0;
			game.GameOver();	
			sounds.PlayDeathSound();
		} else {
			game.statistics.playerHealth = (int)Mathf.Round(tempHealth);
			sounds.PlayPainSound();
		}
	}
	


}
