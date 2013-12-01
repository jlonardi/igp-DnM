using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	//use singleton since only we need one instance of this class
	public static Player instance;

	private GameManager game;
	private OnGuiManager guiManager;
	private PlayerSounds sounds;

    public void Awake() {
        Player.instance = this;
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
		float tempHealth = game.statistics.playerHealth - (damageAmount - (damageAmount * game.statistics.playerArmor));
		if(GameManager.instance.treasureState == TreasureState.CARRYING){
			GameManager.instance.treasureState = TreasureState.SET_ON_GROUND;
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
