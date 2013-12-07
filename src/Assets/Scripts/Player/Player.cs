using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private float health = 100f;
	private float armor; // armor scale 0-50

	private GameManager game;
	private OnGuiManager guiManager;
	private PlayerSounds sounds;
	public bool canBeAttacked = true;

	[HideInInspector]
	public CharacterMotor motor;

    public void Start() {
		game = GameManager.instance;
		guiManager = OnGuiManager.instance;
		motor = GetComponent<CharacterMotor>();
		sounds = PlayerSounds.instance;
	}	

	public void TakeDamage(float damageAmount, DamageType damageType){
		if(canBeAttacked) {
			float tempArmor = 0;
			float tempHealth = 0;

			//handle fire damage here
			if (damageType == DamageType.FIRE){
				//if player has an armor, firedamage is 50% less
				tempArmor = armor - (damageAmount*0.5f);
				if (tempArmor<0){
					armor = 0;
					// amount without armor is 100% of firedamage
					tempHealth = health + (tempArmor*2);
				} else {
					tempHealth = health;
					armor = tempArmor;
				}
			}

			//handle enemy hits and everything but firedamage here
			if (damageType != DamageType.FIRE){
				//if player has an armor, take less damage
				tempArmor = armor - damageAmount;
				if (tempArmor<0){
					armor = 0;
					tempHealth = health + tempArmor;
				} else {
					tempHealth = health;
					armor = tempArmor;
				}
			}

			// if enemy hits player, treasure drops
			if(!game.treasure.OnGround()){
				game.treasure.SetTreasureOnGround();
			}

			// visualize the pain
			guiManager.bloodSplatter.setSplatterVisible( (1f-(tempHealth/100f)));

			if (tempHealth <= 0){
				health = 0;
				game.GameOver();	
				sounds.PlayDeathSound();
			} else {
				health = tempHealth;
				sounds.PlayPainSound();
			}
		}
	}
	
	public float GetHealth(){
		return health;
	}
	
	public void SetHealth(float value){
		health = value;
	}
	
	public float GetArmor(){
		return armor;
	}
	
	public void SetArmor(float value){
		armor = value;
	}

	public void makeImmuneToDamage() {
		canBeAttacked = false;
	}

	public void disableImmunity() {
		canBeAttacked = true;
	}


}
