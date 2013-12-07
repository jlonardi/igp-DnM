using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private int health = 100;
	private int armor; // armor scale 0-50

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

	public void TakeDamage(int damageAmount, DamageType damageType){
		if(canBeAttacked) {
			//if player has an armor, take less damage
			int tempArmor = armor - damageAmount;
			int tempHealth = health;

			if (tempArmor<0){
				armor = 0;
				tempHealth = health + tempArmor;
			} else {
				armor = tempArmor;
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
				health = (int)Mathf.Round(tempHealth);
				sounds.PlayPainSound();
			}
		}
	}
	
	public int GetHealth(){
		return health;
	}
	
	public void SetHealth(int value){
		health = value;
	}
	
	public int GetArmor(){
		return armor;
	}
	
	public void SetArmor(int value){
		armor = value;
	}

	public void makeImmuneToDamage() {
		canBeAttacked = false;
	}

	public void disableImmunity() {
		canBeAttacked = true;
	}
}
