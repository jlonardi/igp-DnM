using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
	private GameManager game;

	void OnTriggerEnter(Collider c){
		// if not triggered by player, do nothing
		if (!c.name.Contains("Player")){
			return;
		}
		// assign game manager
		if (game == null){
			game = GameManager.instance;
		}

		if (gameObject.name.Equals("pickup_treasure") && game.treasureState == TreasureState.SET_ON_GROUND) {
			game.pickupState = PickupState.TREASURE;

		} else if (gameObject.name.Equals("pickup_grenadebox")) {
			game.pickupState = PickupState.GRENADE_BOX;
			
		} else if (gameObject.name.Equals("pickup_minigun")) {
			game.pickupState = PickupState.MINIGUN;

		} else if (gameObject.name.Equals("pickup_scarl")) {
			game.pickupState = PickupState.SCAR_L;

		} else if (gameObject.name.Equals("pickup_armor")) {
			game.pickupState = PickupState.ARMOR;
		}	
	}

	void OnTriggerExit(Collider c){
		// if not triggered by player, do nothing
		if (!c.name.Contains("Player")){
			return;
		}

		game.pickupState = PickupState.NONE;
	}

}
