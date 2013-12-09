using UnityEngine;
using System.Collections;

public class TriggerHandler : MonoBehaviour {
	//use singleton since only we need one instance of this class
	public static TriggerHandler instance;

	public Dragon dragon;
	public bool dragonHasAggroOnPlayer = false;
	public bool playerKilled = false;

	public GameObject lizardPrefab;

	void Awake() {
		TriggerHandler.instance = this;
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
				//if battle is not yet on, start battle music
				EnemyManager enemyManager = EnemyManager.instance;
				if (!enemyManager.inBattle){
					MusicAndAtmoManager.instance.PlayBattleMusic();
					enemyManager.inBattle = true;
				}

				dragonHasAggroOnPlayer = true;
				dragon.flyBackToLair();
				EnemyManager.instance.setDragonSpawns();
				Debug.Log("Player aggroed the dragon");
			}
		}
	}

	public void handleEnterTrigger (Collider other) {
		
		if(other.tag == "enemy") {
			if(dragonHasAggroOnPlayer && !GameManager.instance.statistics.dragonSlayed) {
				if(other.name.Contains("orc") || other.name.Contains("Wolf")) {
					Vector3 pos = other.transform.position;
					pos.y += 1;
					Quaternion rot = other.transform.rotation;
					Destroy (other.gameObject);
					Instantiate(lizardPrefab, pos, rot);
				}
			}
		}


	}
}
