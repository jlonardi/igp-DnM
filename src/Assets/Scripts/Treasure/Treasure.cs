using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {
	public bool onGround;
	private Hud hud;
	private int treasureAmount = 100;
	private Terrain terrain;
	private Animator animator;
	private SimpleSmoothMouseLook mouseLook;
	
	void Start () {
		animator = GetComponent<Animator>();
		animator.speed = 4;	// animation playback speed
		animator.SetBool("onGround",false);
		
		terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
		hud = GameObject.Find("Game Manager").GetComponent<Hud>();
		hud.setTreasure(treasureAmount);
		mouseLook = GameObject.FindObjectOfType(typeof(SimpleSmoothMouseLook)) as SimpleSmoothMouseLook;
	}
	
	public int Loot(int lootAmount){
		// no loot allowed if player is still carrying treasure
		if (!onGround){	
			return 0;
		}
		
		if (treasureAmount > lootAmount){
			treasureAmount -= lootAmount;
		} else {
			lootAmount = treasureAmount;
			treasureAmount = 0;
		}
		hud.setTreasure(treasureAmount);
		return lootAmount;
	}
	
	public void SetTreasureOnGround(){
		GameObject treasureOnScene = GameObject.Find("TreasureOnGround");
		this.transform.parent = treasureOnScene.transform;	
		this.onGround = true;	
		hud.setTreasureOnGround();

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
		mouseLook.clampInDegrees = new Vector2(360, 180);
	}
	
	public bool OnGround(){
		return this.onGround;
	}
	
}
