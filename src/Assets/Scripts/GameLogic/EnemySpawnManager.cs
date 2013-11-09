using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour {
	//use singleton since only we need once instance of this class
	public static EnemySpawnManager instance;
    public void Awake()
    {
        EnemySpawnManager.instance = this;
    }	

	public GameObject orcPrefab;
	
	public List<GameObject> enemies = new List<GameObject>();

	public Transform[] areas;	
	public float timeOfLastWave = -5f;
	public float waveInterval = 5f;
		
	private GameObject player;
	
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
		
		if( (timeOfLastWave + waveInterval) < Time.timeSinceLevelLoad) {
			//Debug.Log("New wave needed");
			timeOfLastWave = Time.timeSinceLevelLoad;
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
		
		GameObject newEnemy = instantiateEnemy(EnemyType.ORC, areas[index].position + offset);

		RaycastHit hit = new RaycastHit();
		Ray ray = new Ray(newEnemy.transform.position, -Vector3.up);
		
        Physics.Raycast(ray,out hit);
		newEnemy.transform.position = new Vector3(newEnemy.transform.position.x, hit.point.y + 3, newEnemy.transform.position.z);
		//Debug.Log("Orc instantiated");
	}
	
	public void ClearEnemies(){
		enemies.Clear();
		foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]){
    		if(go.name.Equals("orc(Clone)")){
				Destroy(go);
    		}
    	}
	}
	
	
	// use prefabs rotation when creating a new enemy
	public GameObject instantiateEnemy(EnemyType enemyType, Vector3 position){
		switch(enemyType) {
        	case EnemyType.ORC:
			default:
				return instantiateEnemy(enemyType, position, orcPrefab.transform.rotation);				
   		}
		
	}
	
	// rotation can be also defined (mainly for restoring saved enemys location)
	public GameObject instantiateEnemy(EnemyType enemyType, Vector3 position, Quaternion rotation){
		GameObject enemyPrefab;
		switch(enemyType) {
        	case EnemyType.ORC:
			default:
				enemyPrefab = orcPrefab;
				break;			
   		}
		GameObject enemyObject = (GameObject)Instantiate(enemyPrefab, position, rotation);
		enemies.Add(enemyObject);
		return enemyObject;
	}
}
