using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnitySerialization;

public class SaveContainer {
	// define all variables to be saved here
	public int formatVersion;
	public int saveSlot;
	public DifficultySetting difficulty;
	public string name;
	public string dateTime;
	public byte[] screenshot;
	public int level;
	public PickupState pickupState;
	public float bloodAlpha;
	public float mousePositionX;
	public float mousePositionY;
	//public float mouseSensitivity;
	//public float mouseSmoothing;
	public float playerArmor;
	public float playerHealth;
	public bool playerAlive;
	public bool treasureOnGround;
	public int treasureAmount;
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
	public bool pickedup_gun3;
	public bool pickedup_gun4;
	public bool pickedup_armor;
	public bool dragonHasAggroOnPlayer;
	public bool playerKilledTrigger;
	public int grenades;
	public int currentGunIndex;
	public int bodycount;
	public int score;
	public bool dragonSlayed;
	public float dragonHealth;
	public float dragonMaxHealth;
	public float dragonLastBreath;
	public bool dragonBreathFire;
	public bool dragonFlying;
	public bool dragonPatroling;
	public bool dragonWalking;
	public bool dragonLanding;
	public bool dragonFighting;
	public float maxSpawnCount;
	public float maxSpawnTime;
	public float maxEnemies;
	public float timeBetweenEnemyCountAddition;
	public bool dragonFightSpwans;
	public float waveInterval;	
	public float originalWaveInterval;
	public float originalMaxEnemies;
	public bool inBattle;
	public int spawnCount;
	public int nextEnemyType;
	public int currentEnemyCount;
	public float waveIntervalOnDragonFight;
	public int maxDragonFightEnemies;
	public float playTime;
	public float timeOfLastWave;
	public float timeOfEnemyCountRising;
	public float spawnTimeStart;

	private GameObject player;
	public bool spawnEnabled = true;
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
		TriggerHandler dragonTriggerHandler = TriggerHandler.instance;

		dragonSlayed = game.statistics.dragonSlayed;

		// save GameObject, Transform, Vector3 and Quaternion by adding .Serializable() extension
		sPlayer = game.player.gameObject.Serializable();
		sTreasure = game.treasure.gameObject.Serializable();
		if (dragonSlayed){
			GameObject dragonRagdoll = GameObject.Find("Dragon Ragdoll(Clone)");
			sDragon = dragonRagdoll.Serializable();
		} else {
			sDragon = game.dragon.gameObject.Serializable();
		}

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

		// save time variables relative to Time.timeSinceLevelLoad as load game's time starts from 0
		timeOfLastWave = enemyManager.timeOfLastWave - Time.timeSinceLevelLoad;
		timeOfEnemyCountRising = enemyManager.timeOfEnemyCountRising - Time.timeSinceLevelLoad;
		spawnTimeStart = enemyManager.spawnTimeStart - Time.timeSinceLevelLoad;

		// save game difficulty setting
		difficulty = game.difficulty;

		formatVersion = game.saves.GetFormatVersion();

		// save regular variable here
		timeBetweenEnemyCountAddition = enemyManager.timeBetweenEnemyCountAddition;
		waveIntervalOnDragonFight = enemyManager.waveIntervalOnDragonFight;
		maxDragonFightEnemies = enemyManager.maxDragonFightEnemies;
		dragonFightSpwans = enemyManager.dragonFightSpwans;
		waveInterval = enemyManager.waveInterval;
		maxEnemies = enemyManager.maxEnemies;
		originalWaveInterval = enemyManager.originalWaveInterval;
		originalMaxEnemies = enemyManager.originalMaxEnemies;
		inBattle = enemyManager.inBattle;
		spawnCount = enemyManager.spawnCount;
		nextEnemyType = enemyManager.nextEnemyType;
		currentEnemyCount = enemyManager.currentEnemyCount;
		dragonHasAggroOnPlayer = dragonTriggerHandler.dragonHasAggroOnPlayer;
		playerKilledTrigger = dragonTriggerHandler.playerKilled;
		mousePositionX = mouseX.GetPosition();
		mousePositionY = mouseY.GetPosition();
		//mouseSensitivity = mouseX.sensitivity;
		//mouseSmoothing = mouseX.smoothing;
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
		pickedup_gun3 = game.weapons.guns[3].picked_up;
		pickedup_gun4 = game.weapons.guns[3].picked_up;
		pickedup_armor = game.statistics.armorPickedUp;
		grenades = game.weapons.grenadeCount;
		currentGunIndex = game.weapons.currentGunIndex;
		dragonFighting = game.dragon.GetFighting();
		dragonPatroling = game.dragon.GetPatroling();
		dragonWalking = game.dragon.GetWalking();
		dragonLanding = game.dragon.GetLanding();
		dragonHealth = game.dragon.GetHealth(); 
		dragonMaxHealth = game.dragon.GetMaxHealth(); 
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
			TriggerHandler dragonTriggerHandler = TriggerHandler.instance;

			GameObject player = GameObject.Find("Player");
			player.GetValuesFrom(sPlayer);
			game.treasure.gameObject.GetValuesFrom(sTreasure);

			game.statistics.dragonSlayed = dragonSlayed;
			//if slayed, make ragdoll
			if (dragonSlayed){
				GameObject dragon = GameObject.Find("Dragon");

				GameObject go = ragdollManager.MakeRagdoll(EnemyType.DRAGON, dragon, false);
				Ragdoll dragonRagdoll = go.GetComponent<Ragdoll>();
				dragonRagdoll.Mute();
				sDragon.restoreChildTransforms(go.transform);
			//else restore dragon
			} else {
				game.dragon.gameObject.GetValuesFrom(sDragon);
			}

			//restore time variables
			game.statistics.playTime = playTime;
			enemyManager.timeOfLastWave = timeOfLastWave;
			enemyManager.timeOfEnemyCountRising = timeOfEnemyCountRising;
			enemyManager.spawnTimeStart = spawnTimeStart;

			// restore variables
			game.difficulty = difficulty;
			enemyManager.timeBetweenEnemyCountAddition = timeBetweenEnemyCountAddition;
			enemyManager.waveIntervalOnDragonFight = waveIntervalOnDragonFight;
			enemyManager.maxDragonFightEnemies = maxDragonFightEnemies;
			enemyManager.dragonFightSpwans = dragonFightSpwans;
			enemyManager.waveInterval = waveInterval;
			enemyManager.maxEnemies = maxEnemies;
			enemyManager.originalWaveInterval = originalWaveInterval;
			enemyManager.originalMaxEnemies = originalMaxEnemies;
			enemyManager.spawnCount = spawnCount;
			enemyManager.nextEnemyType = nextEnemyType;
			enemyManager.currentEnemyCount = currentEnemyCount;
			enemyManager.inBattle = inBattle;

			dragonTriggerHandler.dragonHasAggroOnPlayer = dragonHasAggroOnPlayer;
			dragonTriggerHandler.playerKilled = playerKilledTrigger;
			mouseX.SetPosition(mousePositionX);
			mouseY.SetPosition(mousePositionY);
			//mouseX.SetSensitivity(mouseSensitivity);
			//mouseY.SetSensitivity(mouseSensitivity);
			//mouseX.SetSmoothing(mouseSmoothing);
			//mouseY.SetSmoothing(mouseSmoothing);
			onGuiManager.bloodSplatter.SetBloodAlpha(bloodAlpha);
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
			game.weapons.guns[3].picked_up = pickedup_gun3;
			game.weapons.guns[3].picked_up = pickedup_gun4;
			game.statistics.armorPickedUp = pickedup_armor;
			game.weapons.grenadeCount = grenades;
			game.weapons.currentGunIndex = currentGunIndex;
			game.statistics.level = level;
			game.statistics.bodycount = bodycount;
			game.statistics.score = score;
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
			game.dragon.SetMaxHealth(dragonMaxHealth);
			game.dragon.breathFire = dragonBreathFire;
			game.dragon.flying = dragonFlying;
			//game.dragon.timeOfLastFireBreath = dragonLastBreath;

			//if in battle, change music
			if (inBattle){
				game.GetComponent<MusicAndAtmoManager>().PlayBattleMusic();
			}

			// if armor picked up, disable it on level
			if (pickedup_armor){
				GameObject armor = GameObject.Find("armorOnGround");
				armor.SetActive(false);
			}
			// if minigun picked up, disable it on level
			if (pickedup_armor){
				GameObject minigun = GameObject.Find("minigunOnGround");
				minigun.SetActive(false);
			}
			// if scar picked up, disable it on level
			if (pickedup_armor){
				GameObject scarL = GameObject.Find("scarlOnGround");
				scarL.SetActive(false);
			}
			game.pickupState = PickupState.NONE;

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
			Ragdoll enemyRagdoll;
			foreach (sGameObject sgo in sGameObjects){
				if (sgo==null){
					break;
				}
				if(sgo.name.Equals("orc ragdoll(Clone)")) {
					GameObject go = ragdollManager.MakeRagdoll(EnemyType.ORC, sgo.toGameObject(), false);
					sgo.restoreChildTransforms(go.transform);
					enemyRagdoll = go.GetComponent<Ragdoll>();
					enemyRagdoll.Mute();

				} else if(sgo.name.Equals("orc(Clone)")) {
					enemyManager.CreateEnemy(EnemyType.ORC, sgo.transform.position.toVector3,
					                         sgo.transform.rotation.toQuaternion);
				} else if(sgo.name.Equals("Lizard ragdoll(Clone)")) {
					GameObject go = ragdollManager.MakeRagdoll(EnemyType.LIZARD, sgo.toGameObject(), false);
					sgo.restoreChildTransforms(go.transform);
					enemyRagdoll = go.GetComponent<Ragdoll>();
					enemyRagdoll.Mute();

				} else if(sgo.name.Equals("Lizard(Clone)")) {
					enemyManager.CreateEnemy(EnemyType.LIZARD, sgo.transform.position.toVector3,
					                         sgo.transform.rotation.toQuaternion);
				} else if(sgo.name.Equals("WereWolf ragdoll(Clone)")) {
					GameObject go = ragdollManager.MakeRagdoll(EnemyType.WEREWOLF, sgo.toGameObject(), false);
					sgo.restoreChildTransforms(go.transform);
					enemyRagdoll = go.GetComponent<Ragdoll>();
					enemyRagdoll.Mute();

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