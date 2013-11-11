using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public sVector3 sTreasureLevelPosition = new sVector3(0,0,0);
	public sGameObject sPlayer = new sGameObject();
	public sGameObject sTreasure = new sGameObject();
	public List<sGameObject> sEnemies = new List<sGameObject>();
	public List<sGameObject> sBodies = new List<sGameObject>();
	public List<sGameObject> sObjects = new List<sGameObject>();
	
	void Awake(){
		//screenshot = new Texture2D(320, 180);			
	}
	
	public void SaveValues(){
		GameObject player = GameObject.Find("Player");
		
		// save GameObject, Transform, Vector3 and Quaternion by adding .Serializable() extension
		sPlayer = player.Serializable();
		sTreasure = Treasure.instance.gameObject.Serializable();
		sTreasureLevelPosition = Treasure.instance.treasureLevelPosition.Serializable();

		// clear lists
		sEnemies.Clear();
		sBodies.Clear();
		sObjects.Clear();

		// save enemies and misc objects by listing them
		foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]){
    		if(go.name.Equals("orc(Clone)")){
    			sEnemies.Add(go.Serializable());
    		}
    		if(go.name.Equals("orc ragdoll(Clone)")){				
    			sBodies.Add(go.Serializable());
    		}
			if(go.name.Equals("1x1x1m Box")){
				sObjects.Add(go.Serializable());
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
		
		// restore treasurelevel position
		Treasure.instance.treasureLevelPosition = sTreasureLevelPosition.toVector3;

		// restore player gameobject
		GameObject player = GameObject.Find("Player");		
		player.fromSerialized(sPlayer);
		
		// restore treasure gameobject
		Treasure.instance.gameObject.fromSerialized(sTreasure);		

		// if treasure on ground, make sure animation states are correct by calling SetTreasureOnGround
		if (Treasure.instance.onGround){
			Treasure.instance.RestoreTreasureOnGround();
		}
		
		// select correct gun after loading savegame
		GunManager.instance.ChangeToCurrentWeapon();
		
		// restore ragdolls
		foreach (sGameObject sgo in sBodies){
			GameObject go =	RagdollManager.instance.instantiateRagdoll(EnemyType.ORC, 
										sgo.transform.position.toVector3, sgo.transform.rotation.toQuaternion);
			sgo.restoreChildTransforms(go.transform);
		}
		
		// restore enemies
		foreach (sGameObject sgo in sEnemies){
    		if(sgo.name.Equals("orc(Clone)")){
				EnemySpawnManager.instance.instantiateEnemy(EnemyType.ORC, sgo.transform.position.toVector3, 
																sgo.transform.rotation.toQuaternion);
    		}
    	}		
		
		// find misc objects on scene		
		List<GameObject> Objects = new List<GameObject>();	
		foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]){
			if(go.name.Equals("1x1x1m Box")){
				Objects.Add(go);
			}				
    	}
		
		// failsafe check for object count
		if (Objects.Count != sObjects.Count){
			Debug.Log("ERROR: Object counts do not match!");
			return;			
		}
		
		// restore misc objects position and rotation
		for (int i=0; i < Objects.Count; i++){
			Objects[i].transform.position = sObjects[i].transform.position.toVector3;
			Objects[i].transform.rotation = sObjects[i].transform.rotation.toQuaternion;
    	}
	}	
	
	public void SaveTexture(Texture2D source) {
		//screenshot = ScaleTexture(source, 320, 180);		
	}
	
	public Texture2D ScaleTexture(Texture2D source,int targetWidth,int targetHeight) {
//    	Texture2D result=new Texture2D(targetWidth,targetHeight,source.EncodeToPNG,true);
    	Texture2D result=new Texture2D(targetWidth,targetHeight,source.format,true);
    	Color[] rpixels=result.GetPixels(0);
    	float incX=(1.0f / (float)targetWidth);
    	float incY=(1.0f / (float)targetHeight);
    	for(int px=0; px<rpixels.Length; px++) {
        	rpixels[px] = source.GetPixelBilinear(incX*((float)px%targetWidth), incY*((float)Mathf.Floor(px/targetWidth)));
	    }
    	result.SetPixels(rpixels,0);
	    result.Apply();
    	return result;
	}	
}