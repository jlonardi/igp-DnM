using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {	
	// object which describes how much treasure is left
	private GameObject treasureLevelMesh;	
	// animator handles animation played when treasure is set on ground
	private Animator animator;
	// if treasure is on ground onGround is true
	private bool onGround = false;
	private GameManager game;

	void Awake()
    {
		game = GameManager.instance;
		animator = GetComponent<Animator>();
		animator.speed = 1;	// animation playback speed
		animator.SetBool("onGround",false);
		treasureLevelMesh = transform.FindChild("gold").gameObject;
    }
	
	public int Loot(int lootAmount){
		animator.SetBool("isOpen", true);

		// no loot allowed if player is still carrying treasure
		if (!onGround){
			return 0;
		}
		
		if (game.statistics.treasureAmount > lootAmount){
			game.statistics.treasureAmount -= lootAmount;
		} 
		else {
			lootAmount = game.statistics.treasureAmount;
			game.statistics.treasureAmount = 0;
		}
		
		// change visible money position on chest so treasure seems smaller after every loot.
		// chest's treasure y range is from 0.09 to 0.49 (0.4 total).
		treasureLevelMesh.transform.localPosition -= new Vector3(0, 0.4f * lootAmount/game.statistics.treasureFullAmount, 0);				

		// if all taken, game over
		if (game.statistics.treasureAmount <= 0){
			game.GameOver();
		}
		return lootAmount;
		
	}
	
	// called when savegame restores and treasure is already on ground
	public void RestoreTreasureOnGround(){
		animator.speed = 100;
		onGround = true;
		SetTreasureOnGround();
	}

	// called when player picks up the treasure
	public void CarryTreasure(){
		animator.SetBool("isOpen",false);
		onGround = false;
	}

	// called when player sets treasure on ground	
	public void SetTreasureOnGround(){
		audio.Play();
		GameObject treasureOnScene = GameObject.Find("TreasureOnGround");
		transform.parent = treasureOnScene.transform;	

		// set back to normal collider when on ground 
		gameObject.collider.isTrigger = false;

		// if already on ground, do nothing else
		if (onGround){
			return;	
		}
		
		onGround = true;

		// call gunmanager to enable current weapon
		game.weapons.ChangeToCurrentWeapon();

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

	public bool OnGround()
	{
		return onGround;
	}
}
