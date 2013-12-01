using UnityEngine;
using System.Collections;

public enum TreasureType {
	CHEST,
	NONE
}

public class Treasure : MonoBehaviour {	
	// use singleton since only we need one instance of this class
	public static Treasure instance;

	// define treasure type
	public TreasureType treasureType = TreasureType.NONE;

	// object which describes how much treasure is left
	public GameObject treasureLevel;	

	// animator handles animation played when treasure is set on ground
	private Animator animator;

	private bool setOnGround = false;

	private GameManager game;
	
    void Awake()
    {
        Treasure.instance = this;
		animator = GetComponent<Animator>();
		animator.speed = 4;	// animation playback speed
		animator.SetBool("onGround",false);
    }

	void Update(){
		if (game == null){
			game = GameManager.instance;
		}

		if (!setOnGround && game.gameState == GameState.RUNNING &&
		    game.levelState == LevelState.LOADED && game.treasureState == TreasureState.SET_ON_GROUND){
			SetTreasureOnGround();
		}

	}

	public int Loot(int lootAmount){
		// no loot allowed if player is still carrying treasure
		if (game.treasureState != TreasureState.SET_ON_GROUND){	
			return 0;
		}
		
		if (game.statistics.treasureAmount > lootAmount){
			game.statistics.treasureAmount -= lootAmount;
		} 
		else {
			lootAmount = game.statistics.treasureAmount;
			game.statistics.treasureAmount = 0;
		}
		
		if (treasureType == TreasureType.CHEST){
			// change visible money position on chest so treasure seems smaller after every loot.
			// chest's treasure y range is from -0.03 to 0.2371817 (0.2671817 total).
			treasureLevel.transform.localPosition -= new Vector3(0, 0.2671817f * lootAmount/game.statistics.treasureFullAmount, 0);				
		}

		// if all taken, game over
		if (game.statistics.treasureAmount <= 0){			
			GameManager.instance.GameOver();
		}
		return lootAmount;
		
	}
	
	// called when savegame restores and treasure is already on ground
	public void RestoreTreasureOnGround(){
		animator.speed = 100;
		setOnGround = true;
		SetTreasureOnGround();
	}
	
	// called when player sets treasure on ground	
	public void SetTreasureOnGround(){
		audio.Play();
		GameObject treasureOnScene = GameObject.Find("TreasureOnGround");
		transform.parent = treasureOnScene.transform;	

		GameManager.instance.treasureState = TreasureState.SET_ON_GROUND;

		animator.SetBool("onGround",true);

		// set back to normal colliders when on ground 
		GameObject treasureTop = GameObject.Find("krishka");
		GameObject treasureBottom = GameObject.Find("osnSunduk");
		if (treasureBottom != null && treasureTop != null){
			treasureBottom.collider.isTrigger = false;
			treasureTop.collider.isTrigger = false;
		}		

		// if already on ground, do nothing else
		if (setOnGround){
			return;	
		}
		
		setOnGround = true;

		// call gunmanager to enable current weapon
		GunManager.instance.ChangeToCurrentWeapon();

		MusicAndAtmoManager.instance.PlayBattleMusic();
		
		
		// =========== align treasure on terrain ======================
		
		 //first move up to make sure it doesn't go through terrain
	    transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
 		RaycastHit hit = new RaycastHit();		
		if (Physics.Raycast(transform.position, -Vector3.up, out hit)){
			// align just above terrain		    
			transform.position = new Vector3(transform.position.x, transform.position.y - hit.distance + 0.001f, transform.position.z);
			// rotate to correct angle
 			Vector3 proj = transform.forward - (Vector3.Dot(transform.forward, hit.normal)) * hit.normal;
			transform.rotation = Quaternion.LookRotation(proj, hit.normal);
		}
		//=============================================================
	}
}
