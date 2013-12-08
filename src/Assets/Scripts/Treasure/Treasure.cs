using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {	
	public LayerMask groundLayer;

	private int treasureAmount = 100;
	//original amount is used for calculation current percentage of the treasure	
	private int treasureFullAmount = 100;

	// object which describes how much treasure is left
	private GameObject treasureLevelMesh;	
	// animator handles animation played when treasure is set on ground
	private Animator animator;
	// if treasure is on ground onGround is true
	private bool onGround = false;
	private GameManager game;
	private float timeFromLoot;

	public AudioClip lootSound;
	public AudioClip dropSound;

	void Awake()
    {
		game = GameManager.instance;
		animator = GetComponent<Animator>();
		animator.speed = 1;	// animation playback speed
		animator.SetBool("onGround",false);
		treasureLevelMesh = transform.FindChild("gold").gameObject;
    }

	void Update(){
		if (audio.isPlaying && (timeFromLoot + 1 < Time.time || game.player.GetAliveStatus() == false)){
			audio.Stop();
		}
	}
	
	public int Loot(int lootAmount){
		audio.clip = lootSound;
		audio.loop = true;
		audio.Play();

		timeFromLoot = Time.time;
		animator.SetBool("isOpen", true);

		// no loot allowed if player is still carrying treasure
		if (!onGround){
			return 0;
		}
		
		if (treasureAmount > lootAmount){
			treasureAmount -= lootAmount;
		} 
		else {
			lootAmount = treasureAmount;
			treasureAmount = 0;
		}
		
		// change visible money position on chest so treasure seems smaller after every loot.
		// chest's treasure y range is from 0.09 to 0.49 (0.4 total).
		treasureLevelMesh.transform.localPosition -= new Vector3(0, 0.4f * lootAmount/treasureFullAmount, 0);				

		// if all taken, game over
		if (treasureAmount <= 0 && game.player.GetAliveStatus() == true){
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
		game.pickupState = PickupState.NONE;
		animator.SetBool("isOpen",false);
		onGround = false;

		// if sprinting, stop it while carrying the treasure
		if (game.player.motor.sprinting){
			game.player.motor.StopSprint();
		}
		
		// disable gun so we can carry treasure
		game.weapons.currentGun.enabled = false;
		// find treasure positions from scene
		GameObject treasureBox = GameObject.Find("treasure_box");
		treasureBox.collider.isTrigger = true;
		GameObject treasureOnPlayer = GameObject.Find("Treasure");
		// change parent to players Treasure-object
		treasureBox.transform.parent = treasureOnPlayer.transform;
		// and change local position & rotation back to start values
		treasureBox.transform.localPosition = new Vector3(0,0,-1.28f);
		treasureBox.transform.localRotation = Quaternion.identity;
	}

	// called when player sets treasure on ground	
	public void SetTreasureOnGround(){
		game.pickupState = PickupState.TREASURE;
		audio.PlayOneShot(dropSound, 0.8f);
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
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, groundLayer)){
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

	public int GetTreasureAmount(){
		return treasureAmount;
	}
	
	public void SetTreasureAmount(int value){
		treasureAmount = value;
	}
	
}
