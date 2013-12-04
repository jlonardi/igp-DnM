using UnityEngine;
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
	public float bloodAlpha;
	public float mousePositionX;
	public float mousePositionY;
	public float mouseSensitivity;
	public float mouseSmoothing;
	public float timeOfLastWave;
	public int playerArmor;
	public int playerHealth;
	public TreasureState treasureState;
	public int treasureAmount;
	public int treasureFullAmount;
	public float playTime;
	public int wave;
	public int gun0_rounds;
	public int gun1_rounds;
	public int gun2_rounds;
	public int gun3_rounds;
	public int gun0_clips;
	public int gun1_clips;
	public int gun2_clips;
	public int gun3_clips;
	public int grenades;
	public int currentGunIndex;
	public int bodycount;
	public int score;
	
	// Vector2, Vector3, Quaternion and GameObjects need prefix 's' to use serialized versions
	public sGameObject sPlayer;
	public sGameObject sTreasure;
	public List<sGameObject> sGameObjects;

	// constructor
	public SaveContainer(){
	}
	
	public void SaveValues(){		
		GameManager game = GameManager.instance;
		GunManager gunManager = GunManager.instance;
		EnemyManager enemyManager = EnemyManager.instance;
		OnGuiManager onGuiManager = OnGuiManager.instance;
		GameObject treasure = Treasure.instance.gameObject;
		GameObject player = GameObject.Find("Player");
		SmoothMouseLookX mouseX = SmoothMouseLookX.instance;
		SmoothMouseLookY mouseY = SmoothMouseLookY.instance;

		// save GameObject, Transform, Vector3 and Quaternion by adding .Serializable() extension
		sPlayer = player.Serializable();
		sTreasure = treasure.Serializable();

		// save enemies and misc objects by listing them
		sGameObjects = new List<sGameObject>();
		foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]){
    		if (go.name.Equals("orc(Clone)") || go.name.Equals("orc ragdoll(Clone)") || 
						go.name.Equals("1x1x1m Box")){
    			sGameObjects.Add(go.Serializable());
    		}
    	}				

		// save current time and date
		dateTime = System.DateTime.Now.ToString("MM/dd/yyyy, HH:mm");

		// save playtime with previous value + time since level load
		playTime = game.statistics.playTime + Time.timeSinceLevelLoad;

		// save regular variable here
		mousePositionX = mouseX.position;
		mousePositionY = mouseY.position;
		mouseSensitivity = mouseX.sensitivity;
		mouseSmoothing = mouseX.smoothing;
		level = game.statistics.level;
		bodycount = game.statistics.bodycount;
		score = game.statistics.score;
		bloodAlpha = onGuiManager.bloodSplatter.bloodAlpha;
		playerArmor = game.statistics.playerArmor;
		playerHealth = game.statistics.playerHealth;
		treasureAmount = game.statistics.treasureAmount;
		treasureFullAmount = game.statistics.treasureFullAmount;
		treasureState = game.treasureState;
		wave = game.statistics.wave;
		timeOfLastWave = enemyManager.timeOfLastWave;
		gun0_rounds = gunManager.guns[0].gun.currentRounds;
		gun1_rounds = gunManager.guns[1].gun.currentRounds;
		gun2_rounds = gunManager.guns[2].gun.currentRounds;
		gun3_rounds = gunManager.guns[3].gun.currentRounds;
		gun0_clips = gunManager.guns[0].gun.totalClips;
		gun1_clips = gunManager.guns[1].gun.totalClips;
		gun2_clips = gunManager.guns[2].gun.totalClips;
		gun3_clips = gunManager.guns[3].gun.totalClips;
		grenades = gunManager.grenadeCount;
		currentGunIndex = gunManager.currentGunIndex;
	}

	public void RestoreValues(){
		try{
			GameManager game = GameManager.instance;
			GunManager gunManager = GunManager.instance;
			EnemyManager enemyManager = EnemyManager.instance;
			OnGuiManager onGuiManager = OnGuiManager.instance;
			RagdollManager ragdollManager = RagdollManager.instance;
			Treasure treasure = Treasure.instance;
			SmoothMouseLookX mouseX = SmoothMouseLookX.instance;
			SmoothMouseLookY mouseY = SmoothMouseLookY.instance;

			GameObject player = GameObject.Find("Player");
			player.GetValuesFrom(sPlayer);

			treasure.gameObject.GetValuesFrom(sTreasure);

			// restore variables
			mouseX.position = mousePositionX;
			mouseY.position = mousePositionY;
			mouseX.sensitivity = mouseSensitivity;
			mouseY.sensitivity = mouseSensitivity;
			mouseX.smoothing = mouseSmoothing;
			mouseY.smoothing = mouseSmoothing;
			onGuiManager.bloodSplatter.bloodAlpha = bloodAlpha;
			gunManager.guns[0].gun.currentRounds = gun0_rounds;
			gunManager.guns[1].gun.currentRounds = gun1_rounds;
			gunManager.guns[2].gun.currentRounds = gun2_rounds;
			gunManager.guns[3].gun.currentRounds = gun3_rounds;
			gunManager.guns[0].gun.totalClips = gun0_clips;
			gunManager.guns[1].gun.totalClips = gun1_clips;
			gunManager.guns[2].gun.totalClips = gun2_clips;
			gunManager.guns[3].gun.totalClips = gun3_clips;
			gunManager.grenadeCount = grenades;
			gunManager.currentGunIndex = currentGunIndex;
			game.statistics.level = level;
			game.statistics.playerArmor = playerArmor;
			game.statistics.playerHealth = playerHealth;
			game.statistics.bodycount = bodycount;
			game.statistics.score = score;
			game.statistics.playTime = playTime;
			game.statistics.wave = wave;
			game.statistics.treasureAmount = treasureAmount;
			game.statistics.treasureFullAmount = treasureFullAmount;
			game.treasureState = treasureState;

			// calculate time of last enemy wave
			enemyManager.timeOfLastWave = Time.time - (playTime - timeOfLastWave);

			// if treasure on ground, make sure animation states are correct by calling SetTreasureOnGround
			if (game.treasureState == TreasureState.SET_ON_GROUND){
				treasure.RestoreTreasureOnGround();
			}
			
			// select correct gun after loading savegame
			gunManager.ChangeToCurrentWeapon();
			
			// if no objects to process, stop here
			if (sGameObjects.Count == 0){
				return;
			}
			
			// find misc objects on scene		
			List<GameObject> boxObjects = new List<GameObject>();	
			foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]){
				if (go==null){
					break;
				}
				if(go.name.Equals("1x1x1m Box")){
					boxObjects.Add(go);
				}				
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

				} else if(sgo.name.Equals("1x1x1m Box")) {
					int index = boxObjects.Count - 1;
					if (index > -1){
						boxObjects[index].transform.position = sgo.transform.position.toVector3;
						boxObjects[index].transform.rotation = sgo.transform.rotation.toQuaternion;
						boxObjects.RemoveAt(index);
					}
				}
	    	}
		} catch {
			Debug.LogError("Failed to load savegame");
		}
	}	
}