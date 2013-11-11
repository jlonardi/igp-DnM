using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {	
	//use singleton since only we need once instance of this class
	public static Treasure instance;

	public int amount = 100;
	public bool onGround;
	private Animator animator;
	
    public void Awake()
    {
        Treasure.instance = this;
		animator = GetComponent<Animator>();
		animator.speed = 4;	// animation playback speed
		animator.SetBool("onGround",false);
    }
	
	
	public int Loot(int lootAmount){
		// no loot allowed if player is still carrying treasure
		if (!onGround){	
			return 0;
		}
		
		if (amount > lootAmount){
			amount -= lootAmount;
		} 
		else {
			lootAmount = amount;
			amount = 0;
		}
		if (amount <= 0){			
			GameManager.instance.GameOver();
		}
		return lootAmount;
		
	}
	
	// called when savegame restores and treasure is already on ground
	public void RestoreTreasureOnGround(){
		animator.speed = 100;
		SetTreasureOnGround();
	}
	
	// called when player sets treasure on ground	
	public void SetTreasureOnGround(){		
		GameObject treasureOnScene = GameObject.Find("TreasureOnGround");
		transform.parent = treasureOnScene.transform;	

		
		animator.SetBool("onGround",true);

		// set back to normal colliders when on ground 
		GameObject treasureTop = GameObject.Find("krishka");
		GameObject treasureBottom = GameObject.Find("osnSunduk");
		if (treasureBottom != null && treasureTop != null){
			treasureBottom.collider.isTrigger = false;
			treasureTop.collider.isTrigger = false;
		}		

		// if already on ground, do nothing else
		if (onGround){
			return;	
		}
		
		this.onGround = true;
		
		// call gunmanager to enable current weapon
		GunManager.instance.ChangeToCurrentWeapon();
		
		
		// =========== align treasure on terrain ======================
		
		 //first move up to make sure it doesn't go through terrain
	    transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
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
