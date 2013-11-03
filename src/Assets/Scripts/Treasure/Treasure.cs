using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {
	private Hud hud;
	private int treasureAmount = 100;
	public bool onGround;

	void Start () {
		hud = GameObject.Find("Game Manager").GetComponent<Hud>();
	
	}
	
	void Update () {
		hud.setTreasure(treasureAmount);
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
		return lootAmount;
	}
	
	public void SetTreasureOnGround(){
		GameObject treasureOnScene = GameObject.Find("TreasureOnGround");
		this.transform.parent = treasureOnScene.transform;
		this.onGround = true;		
	}
	
	public bool OnGround(){
		return this.onGround;
	}
	
}
