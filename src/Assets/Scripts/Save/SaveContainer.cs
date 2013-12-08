﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnitySerialization;

public class SaveContainer {
	// define all variables to be saved here
	public int saveSlot;
	public string name;
	public string dateTime;
	public byte[] screenshot;
	public int level;
	public PickupState pickupState;
	public float bloodAlpha;
	public float mousePositionX;
	public float mousePositionY;
	public float mouseSensitivity;
	public float mouseSmoothing;
	public float timeOfLastWave;
	public float playerArmor;
	public float playerHealth;
	public bool playerAlive;
	public bool treasureOnGround;
	public int treasureAmount;
	public float playTime;
	public int wave;
	public int gun0_rounds;
	public int gun1_rounds;
	public int gun2_rounds;
	public int gun3_rounds;
	public int gun4_rounds;
	public int gun0_clips;
	public int gun1_clips;
	public int gun2_clips;
	public int gun3_clips;
	public int gun4_clips;
	public int grenades;
	public int currentGunIndex;
	public int bodycount;
	public int score;
	public float dragonHealth;
	public float dragonLastBreath;
	public bool dragonBreathFire;
	public bool dragonFlying;
	public bool dragonPatroling;
	public bool dragonWalking;
	public bool dragonLanding;
	public bool dragonFighting;

	// Vector2, Vector3, Quaternion and GameObjects need prefix 's' to use serialized versions
	public sGameObject sPlayer;
	public sGameObject sDragon;
	public sGameObject sTreasure;
	public List<sGameObject> sGameObjects;

	// constructor
	public SaveContainer(){
	}
	
	public void SaveValues(){		
		GameManager game = GameManager.instance;
		EnemyManager enemyManager = EnemyManager.instance;
		OnGuiManager onGuiManager = OnGuiManager.instance;
		SmoothMouseLookX mouseX = SmoothMouseLookX.instance;
		SmoothMouseLookY mouseY = SmoothMouseLookY.instance;

		// save GameObject, Transform, Vector3 and Quaternion by adding .Serializable() extension
		sPlayer = game.player.gameObject.Serializable();
		sDragon = game.dragon.gameObject.Serializable();
		sTreasure = game.treasure.gameObject.Serializable();

		// save enemies and misc objects by listing them
		sGameObjects = new List<sGameObject>();
		foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]){
			if (go.name.Equals("orc(Clone)") || go.name.Equals("orc ragdoll(Clone)") || 
			    go.name.Equals("Lizard(Clone)") || go.name.Equals("Lizard ragdoll(Clone)") || 
			    go.name.Equals("WereWolf(Clone)") || go.name.Equals("WereWolf ragdoll(Clone)")){ 
				sGameObjects.Add(go.Serializable());
    		}
    	}				

		// save current time and date
		dateTime = System.DateTime.Now.ToString("MM/dd/yyyy, HH:mm");

		// save playtime with previous value + time since level load
		playTime = game.statistics.playTime + Time.timeSinceLevelLoad;

		// save regular variable here
		mousePositionX = mouseX.GetPosition();
		mousePositionY = mouseY.GetPosition();
		mouseSensitivity = mouseX.sensitivity;
		mouseSmoothing = mouseX.smoothing;
		level = game.statistics.level;
		bodycount = game.statistics.bodycount;
		score = game.statistics.score;
		pickupState = game.pickupState;
		bloodAlpha = onGuiManager.bloodSplatter.GetBloodAlpha();
		playerArmor = game.player.GetArmor();
		playerHealth = game.player.GetHealth();
		playerAlive = game.player.GetAliveStatus();
		treasureOnGround = game.treasure.OnGround();
		treasureAmount = game.treasure.GetTreasureAmount();
		wave = game.statistics.wave;
		timeOfLastWave = enemyManager.timeOfLastWave;
		gun0_rounds = game.weapons.guns[0].currentRounds;
		gun1_rounds = game.weapons.guns[1].currentRounds;
		gun2_rounds = game.weapons.guns[2].currentRounds;
		gun3_rounds = game.weapons.guns[3].currentRounds;
		gun4_rounds = game.weapons.guns[4].currentRounds;
		gun0_clips = game.weapons.guns[0].totalClips;
		gun1_clips = game.weapons.guns[1].totalClips;
		gun2_clips = game.weapons.guns[2].totalClips;
		gun3_clips = game.weapons.guns[3].totalClips;
		gun4_clips = game.weapons.guns[4].totalClips;
		grenades = game.weapons.grenadeCount;
		currentGunIndex = game.weapons.currentGunIndex;
		dragonFighting = game.dragon.GetFighting();
		dragonPatroling = game.dragon.GetPatroling();
		dragonWalking = game.dragon.GetWalking();
		dragonLanding = game.dragon.GetLanding();
		dragonHealth = game.dragon.GetHealth(); 
		dragonBreathFire = game.dragon.breathFire;
		dragonLastBreath = game.dragon.timeOfLastFireBreath;
		dragonFlying = game.dragon.flying;
	}

	public void RestoreValues(){
		try{
			GameManager game = GameManager.instance;
			EnemyManager enemyManager = EnemyManager.instance;
			OnGuiManager onGuiManager = OnGuiManager.instance;
			RagdollManager ragdollManager = RagdollManager.instance;
			SmoothMouseLookX mouseX = SmoothMouseLookX.instance;
			SmoothMouseLookY mouseY = SmoothMouseLookY.instance;

			GameObject player = GameObject.Find("Player");
			player.GetValuesFrom(sPlayer);
			game.treasure.gameObject.GetValuesFrom(sTreasure);
			game.dragon.gameObject.GetValuesFrom(sDragon);

			// restore variables
			mouseX.SetPosition(mousePositionX);
			mouseY.SetPosition(mousePositionY);
			mouseX.SetSensitivity(mouseSensitivity);
			mouseY.SetSensitivity(mouseSensitivity);
			mouseX.SetSmoothing(mouseSmoothing);
			mouseY.SetSmoothing(mouseSmoothing);
			onGuiManager.bloodSplatter.SetBloodAlpha(bloodAlpha);
			game.pickupState = pickupState;
			game.weapons.guns[0].currentRounds = gun0_rounds;
			game.weapons.guns[1].currentRounds = gun1_rounds;
			game.weapons.guns[2].currentRounds = gun2_rounds;
			game.weapons.guns[3].currentRounds = gun3_rounds;
			game.weapons.guns[4].currentRounds = gun4_rounds;
			game.weapons.guns[0].totalClips = gun0_clips;
			game.weapons.guns[1].totalClips = gun1_clips;
			game.weapons.guns[2].totalClips = gun2_clips;
			game.weapons.guns[3].totalClips = gun3_clips;
			game.weapons.guns[4].totalClips = gun4_clips;
			game.weapons.grenadeCount = grenades;
			game.weapons.currentGunIndex = currentGunIndex;
			game.statistics.level = level;
			game.statistics.bodycount = bodycount;
			game.statistics.score = score;
			game.statistics.playTime = playTime;
			game.statistics.wave = wave;
			game.player.SetArmor(playerArmor);
			game.player.SetHealth(playerHealth);
			game.player.SetAliveStatus(playerAlive);
			game.treasure.SetTreasureAmount(treasureAmount);
			game.dragon.SetFighting(dragonFighting);
			game.dragon.SetPatroling(dragonPatroling);
			game.dragon.SetWalking(dragonWalking);
			game.dragon.SetLanding(dragonLanding);
			game.dragon.SetHealth(dragonHealth);
			game.dragon.breathFire = dragonBreathFire;
			game.dragon.flying = dragonFlying;
			game.dragon.timeOfLastFireBreath = dragonLastBreath;

			// calculate time of last enemy wave
			enemyManager.timeOfLastWave = Time.time - (playTime - timeOfLastWave);

			// if treasure on ground, make sure animation states are correct by calling SetTreasureOnGround
			if (treasureOnGround){
				game.treasure.RestoreTreasureOnGround();
			}
			
			// select correct gun after loading savegame
			game.weapons.ChangeToCurrentWeapon();
			
			// if no objects to process, stop here
			if (sGameObjects.Count == 0){
				return;
			}
			
			// restore game objects on scene by gameobject type
			foreach (sGameObject sgo in sGameObjects){
				if (sgo==null){
					break;
				}
				if(sgo.name.Equals("orc ragdoll(Clone)")) {
					GameObject go = ragdollManager.MakeRagdoll(EnemyType.ORC, sgo.toGameObject(), false);
					sgo.restoreChildTransforms(go.transform);
					
				} else if(sgo.name.Equals("orc(Clone)")) {
					enemyManager.CreateEnemy(EnemyType.ORC, sgo.transform.position.toVector3,
					                         sgo.transform.rotation.toQuaternion);
				} else if(sgo.name.Equals("Lizard ragdoll(Clone)")) {
					GameObject go = ragdollManager.MakeRagdoll(EnemyType.LIZARD, sgo.toGameObject(), false);
					sgo.restoreChildTransforms(go.transform);
					
				} else if(sgo.name.Equals("Lizard(Clone)")) {
					enemyManager.CreateEnemy(EnemyType.LIZARD, sgo.transform.position.toVector3,
					                         sgo.transform.rotation.toQuaternion);
				} else if(sgo.name.Equals("WereWolf ragdoll(Clone)")) {
					GameObject go = ragdollManager.MakeRagdoll(EnemyType.WEREWOLF, sgo.toGameObject(), false);
					sgo.restoreChildTransforms(go.transform);
					
				} else if(sgo.name.Equals("WereWolf(Clone)")) {
					enemyManager.CreateEnemy(EnemyType.WEREWOLF, sgo.transform.position.toVector3,
					                         sgo.transform.rotation.toQuaternion);
				}

	    	}
		} catch {
			Debug.LogError("Failed to load savegame");
		}
	}	
}