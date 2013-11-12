using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnitySerialization;

public class SaveContainer {
	// define all variables to be saved here
	public string name;
	public int saveSlot;
	public byte[] screenshot;
	public int level;
	public float mouseX;
	public float mouseY;
	public float timeOfLastWave;
	public float playerArmor;
	public int playerHealth;
	public TreasureType treasureType;
	public int treasureAmount;
	public int treasureFullAmount;
	public bool treasureOnGround;
	public float playTime;
	public int wave;
	public int gun0_rounds;
	public int gun1_rounds;
	public int gun0_clips;
	public int gun1_clips;
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
		GameObject player = GameObject.Find("Player");

		// save GameObject, Transform, Vector3 and Quaternion by adding .Serializable() extension
		sPlayer = player.Serializable();
		sTreasure = Treasure.instance.gameObject.Serializable();

		// save enemies and misc objects by listing them
		sGameObjects = new List<sGameObject>();
		foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]){
    		if (go.name.Equals("orc(Clone)") || go.name.Equals("orc ragdoll(Clone)") || 
						go.name.Equals("1x1x1m Box")){
    			sGameObjects.Add(go.Serializable());
    		}
    	}				
		
		// save regular variable here
		mouseX = SmoothMouseLookX.instance.position;
		mouseY = SmoothMouseLookY.instance.position;
		level = GameManager.instance.statistics.level;
		timeOfLastWave = EnemySpawnManager.instance.timeOfLastWave;
		playerArmor = PlayerHealth.instance.armor;
		playerHealth = PlayerHealth.instance.health;
		treasureAmount = Treasure.instance.treasureAmount;
		treasureFullAmount = Treasure.instance.treasureFullAmount;
		treasureOnGround = Treasure.instance.onGround;
		playTime = GameManager.instance.statistics.playTime;
		wave = GameManager.instance.waves.wave;
		gun0_rounds = GunManager.instance.guns[0].gun.currentRounds;
		gun1_rounds = GunManager.instance.guns[1].gun.currentRounds;
		gun0_clips = GunManager.instance.guns[0].gun.totalClips;
		gun1_clips = GunManager.instance.guns[1].gun.totalClips;
		currentGunIndex = GunManager.instance.currentGunIndex;
		bodycount = GameManager.instance.statistics.bodycount;
		score = GameManager.instance.statistics.score;
	}

	public void RestoreValues(){
		try{
			GameObject playerOnScene = GameObject.Find("Player");
			playerOnScene.GetValuesFrom(sPlayer);

			Treasure.instance.gameObject.GetValuesFrom(sTreasure);

			// restore variables
			SmoothMouseLookX.instance.position = mouseX;
			SmoothMouseLookY.instance.position = mouseY;
			GameManager.instance.statistics.level = level;
			EnemySpawnManager.instance.timeOfLastWave = timeOfLastWave;
			PlayerHealth.instance.armor = playerArmor;
			PlayerHealth.instance.health = playerHealth;
			Treasure.instance.treasureType = treasureType;
			Treasure.instance.treasureAmount = treasureAmount;
			Treasure.instance.treasureFullAmount = treasureFullAmount;
			Treasure.instance.onGround = treasureOnGround;
			GameManager.instance.statistics.playTime = playTime;
			GameManager.instance.waves.wave = wave;
			GunManager.instance.guns[0].gun.currentRounds = gun0_rounds;
			GunManager.instance.guns[1].gun.currentRounds = gun1_rounds;
			GunManager.instance.guns[0].gun.totalClips = gun0_clips;
			GunManager.instance.guns[1].gun.totalClips = gun1_clips;
			GunManager.instance.currentGunIndex = currentGunIndex;
			GameManager.instance.statistics.bodycount = bodycount;
			GameManager.instance.statistics.score = score;
				
			// if treasure on ground, make sure animation states are correct by calling SetTreasureOnGround
			if (Treasure.instance.onGround){
				Treasure.instance.RestoreTreasureOnGround();
			}
			
			// select correct gun after loading savegame
			GunManager.instance.ChangeToCurrentWeapon();
			
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
					GameObject go = RagdollManager.instance.instantiateRagdoll(EnemyType.ORC, 
														sgo.transform.position.toVector3, sgo.transform.rotation.toQuaternion);
					sgo.restoreChildTransforms(go.transform);
					
				} else if(sgo.name.Equals("orc(Clone)")) {
					EnemySpawnManager.instance.instantiateEnemy(EnemyType.ORC, sgo.transform.position.toVector3,
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