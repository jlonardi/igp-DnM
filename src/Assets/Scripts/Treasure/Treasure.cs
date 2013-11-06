using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {	
	//use singleton since only we need once instance of this class
	public static Treasure instance;
    public void Awake()
    {
        Treasure.instance = this;
    }
	
	public int amount = 100;
	public bool onGround;
	private Terrain terrain;
	private Animator animator;
	private SimpleSmoothMouseLook mouseLook;
	
	void Start () {
		animator = GetComponent<Animator>();
		animator.speed = 4;	// animation playback speed
		animator.SetBool("onGround",false);
		
		terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
		mouseLook = GameObject.FindObjectOfType(typeof(SimpleSmoothMouseLook)) as SimpleSmoothMouseLook;
		
		//temp fix for saving
		if (onGround){
			GunManager.instance.EnableWeapons();
		}//
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
	
	public void SetTreasureOnGround(){		
		GameObject treasureOnScene = GameObject.Find("TreasureOnGround");
		this.transform.parent = treasureOnScene.transform;	
		this.onGround = true;	

		// align treasure on terrain
	    transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z); //first move up to make sure it doesn't go through terrain
 		RaycastHit hit = new RaycastHit();		
		if (Physics.Raycast(transform.position, -Vector3.up, out hit)){
			float yValue = terrain.SampleHeight(transform.position);
		    transform.position = new Vector3(transform.position.x, yValue + 0.01f, transform.position.z); // align just above terrain
 			Vector3 proj = transform.forward - (Vector3.Dot(transform.forward, hit.normal)) * hit.normal;
			transform.rotation = Quaternion.LookRotation(proj, hit.normal);
		}
		animator.SetBool("onGround",true);
		
		GunManager.instance.EnableWeapons();
		mouseLook.clampInDegrees = new Vector2(360, 180);
	}
}
