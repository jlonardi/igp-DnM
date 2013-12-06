using UnityEngine;
using System.Collections;

public class TriggerHandler : MonoBehaviour {

	public bool playerInsideLair = false;

	public void handleTrigger (Collider other) {
	
		if(other.tag == "Player") {
			if(playerInsideLair) {
				playerInsideLair = false;
			} else {
				playerInsideLair = true;
			}
			Debug.Log("Player hit the trigger");
		}
	}
}
