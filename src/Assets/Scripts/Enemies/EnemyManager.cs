using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {
	//use singleton since only we need one instance of this class
	public static EnemyManager instance;
	
	//current spawn count
	private int spawnCount = 0;

	//spawning will stop after this spawncount
	public int maxSpawnCount = 100;

	//spawning will stop after game has lasted longer than this time (s)
	public float maxSpawnTime = 600;

	[HideInInspector]
	public float spawnTimeStart = 0;

	//maximum amount of enemies at once on the game level
	public int maxEnemies = 10;

	//enemy type for next enemy to be spawned (0-2)
	private int nextEnemyType = 0;

	public GameObject orcPrefab;
	public GameObject lizardPrefab;
	public GameObject wolfPrefab;

	[HideInInspector]
	public int currentEnemyCount = 0;

	public Transform[] areas;	
	public float timeOfLastWave = -3f;
	public float waveInterval = 3f;
		
	private GameObject player;
	private bool spawnEnabled = true;
	
	void Awake()
	{
		EnemyManager.instance = this;
	}	
	
	void Start () {	
		player = GameObject.Find("Player");	
		Transform spawns = GameObject.Find("Spawns").transform;
		areas = spawns.GetComponentsInChildren<Transform>();
		spawnTimeStart = Time.time;
	}

	// Update is called once per frame
	void Update () {
		
		if(spawnEnabled && newWaveNeeded()) {
			//Debug.Log("new spawn);
			createSpawnWave();
		}
	}
	
	private bool newWaveNeeded() {
		// don't add enemies if we have already spawned all enemies for this game
		if (spawnCount >= maxSpawnCount || (spawnTimeStart + maxSpawnTime) < Time.time){		
			spawnEnabled = false;
			//Debug.Log("spawns disabled");
			return false;
		}

		// don't add enemies if we have already a maximum number we can handle
		if (currentEnemyCount >= maxEnemies){
			return false;
		}

		if( (timeOfLastWave + waveInterval) < Time.time) {
			timeOfLastWave = Time.time;
			return true;	
		}
		
		return false;
	}
	
	private void createSpawnWave() {
		int index = (int)Mathf.Round(Random.Range(0f, areas.Length-1));		
		
		while(Vector3.Distance( areas[index].position, player.transform.position) < 50) {
			index = (int)Mathf.Round(Random.Range(0f, areas.Length-1));
		}
		
		Vector3 offset = new Vector3(Random.Range(-10f,10f),0,Random.Range(-10f,10f));

		nextEnemyType++;
		// create a new enemy instance
		if (nextEnemyType == 1){
			GameObject enemy = CreateEnemy(EnemyType.ORC, areas[index].position + offset);
			AlignEnemy(enemy);
		} else if (nextEnemyType == 2){
			GameObject enemy = CreateEnemy(EnemyType.LIZARD, areas[index].position + offset);
			AlignEnemy(enemy);
		} else {
			GameObject enemy = CreateEnemy(EnemyType.WEREWOLF, areas[index].position + offset);
			AlignEnemy(enemy);
			// as this is the last enemy type, reset next counter
			nextEnemyType = 0;
		}

		spawnCount++;
	}

	public void AlignEnemy(GameObject enemy){
		// use raycast to set enemy above the terrain
		RaycastHit hit = new RaycastHit();
		Ray ray = new Ray(enemy.transform.position, -Vector3.up);		
		Physics.Raycast(ray,out hit);
		enemy.transform.position = new Vector3(enemy.transform.position.x, hit.point.y + 3, enemy.transform.position.z);
	}

	// make a new enemy with prefab's rotation
	public GameObject CreateEnemy(EnemyType enemyType, Vector3 position){
		return CreateEnemy(enemyType, position, Quaternion.identity);
	}

	// make a new enemy and apply given rotation if applyRotation is set
	public GameObject CreateEnemy(EnemyType enemyType, Vector3 position, Quaternion rotation){
		GameObject enemyObject;		
		GameObject enemyPrefab;

		// add enemycount
		currentEnemyCount++;

		// select correct prefab by enemy type
		switch(enemyType) {
		case EnemyType.LIZARD:
			enemyPrefab = lizardPrefab;
			break;
		case EnemyType.WEREWOLF:
			enemyPrefab = wolfPrefab;
			break;
		case EnemyType.ORC:
			default:
				enemyPrefab = orcPrefab;
				break;			
		}

		//if rotation is not set to null, apply it
		if (rotation != Quaternion.identity){
			enemyObject = (GameObject)Instantiate(enemyPrefab, position, rotation);

		// else use prefabs rotation
		} else {
			enemyObject = (GameObject)Instantiate(enemyPrefab, position, enemyPrefab.transform.rotation);
		}
		return enemyObject;
	}
}
