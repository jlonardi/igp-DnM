using UnityEngine;
using System.Collections;

public class TriggerHandler : MonoBehaviour {

	public bool playerInsideLair = false;

	public Dragon dragon;
	public bool dragonHasAggroOnPlayer = false;
	public bool playerInsideTheLair = false;
	public bool playerKilled = false;

	void Awake() {
		dragon = GameObject.Find("Dragon").GetComponent<Dragon>();
	}

	public void handleKillTrigger (Collider other) {
	
		if(other.tag == "Player") {
			if(dragonHasAggroOnPlayer) {
				if(!playerKilled) {
					dragon.killPlayer();
					playerKilled = true;
				}
			}
		}
	}

	public void handleAggroTrigger (Collider other) {
		
		if(other.tag == "Player") {
			if(!dragonHasAggroOnPlayer) {
				dragonHasAggroOnPlayer = true;
				dragon.flyBackToLair();
				Debug.Log("Player aggroed the dragon");
			}
		}
	}

	public void handleEnterTrigger (Collider other) {
		
		if(other.tag == "Player") {
			if(playerInsideTheLair) {
				playerInsideTheLair = false;
				Debug.Log("Player exits the lair");
			} else {
				playerInsideTheLair = true;
				Debug.Log("Player entered the lair");
			}
		}
	}
}
