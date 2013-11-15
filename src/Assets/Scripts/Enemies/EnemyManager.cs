using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {
	//use singleton since only we need one instance of this class
	public static EnemyManager instance;

	public GameObject orcPrefab;
	public int maxEnemies = 20;

	[HideInInspector]
	public int currentEnemyCount = 0;

	public Transform[] areas;	
	public float timeOfLastWave = -5f;
	public float waveInterval = 5f;
		
	private GameObject player;
	
	void Awake()
	{
		EnemyManager.instance = this;
	}	
	
	void Start () {	
		player = GameObject.Find("Player");	
		Transform spawns = GameObject.Find("Spawns").transform;
		areas = spawns.GetComponentsInChildren<Transform>();
	}

	// Update is called once per frame
	void Update () {
		
		if(newWaveNeeded()) {
			createSpawnWave();
		}
	}
	
	private bool newWaveNeeded() {
		
		if( (timeOfLastWave + waveInterval) < Time.time) {
			//Debug.Log("New wave needed");
			timeOfLastWave = Time.time;
			return true;	
		}
		
		return false;
	}
	
	private void createSpawnWave() {
		// don't add enemies if we have already a maximum number we can handle
		if (currentEnemyCount >= maxEnemies){
			return;
		}

		int index = (int)Mathf.Round(Random.Range(0f, areas.Length-1));		
		
		while(Vector3.Distance( areas[index].position, player.transform.position) < 50) {
			index = (int)Mathf.Round(Random.Range(0f, areas.Length-1));
		}
		
		Vector3 offset = new Vector3(Random.Range(-10f,10f),0,Random.Range(-10f,10f));

		// create a new enemy instance
		GameObject enemy = CreateEnemy(EnemyType.ORC, areas[index].position + offset);

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
