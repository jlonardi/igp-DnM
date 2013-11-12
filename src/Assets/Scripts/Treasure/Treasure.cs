using UnityEngine;
using System.Collections;

public enum TreasureType {
	CHEST,
	NONE
}
public class Treasure : MonoBehaviour {	
	//use singleton since only we need once instance of this class
	public static Treasure instance;
	
	public TreasureType treasureType = TreasureType.NONE;
	public int treasureAmount = 100;

	//orig amount is used for calculation current percentage of the treasure	
	public int treasureFullAmount;
	
	public bool onGround;
	private Animator animator;
	
	//object which describes how much treasure is left
	public GameObject treasureLevel;	
	
    void Awake()
    {
        Treasure.instance = this;
		animator = GetComponent<Animator>();
		animator.speed = 4;	// animation playback speed
		animator.SetBool("onGround",false);
		treasureFullAmount = treasureAmount;
    }

	public int Loot(int lootAmount){
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
		
		if (treasureType == TreasureType.CHEST){
			// change visible money position on chest so treasure seems smaller after every loot.
			// chest's treasure y range is from -0.03 to 0.2371817 (0.2671817 total).
			treasureLevel.transform.localPosition -= new Vector3(0, 0.2671817f * lootAmount/treasureFullAmount, 0);				
		}

		// if all taken, game over
		if (treasureAmount <= 0){			
			GameManager.instance.GameOver();
		}
		return lootAmount;
		
	}
	
	// called when savegame restores and treasure is already on ground
	public void RestoreTreasureOnGround(){
		animator.speed = 100;
		SetTreasureOnGround();
		if (treasureType == TreasureType.CHEST){
			treasureLevel.transform.localPosition -= new Vector3(0, 
			               0.2671817f * treasureAmount/treasureFullAmount, 0);				
		}
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
