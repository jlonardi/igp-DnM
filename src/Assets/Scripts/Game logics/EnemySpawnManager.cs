using UnityEngine;
using System.Collections;

public class EnemySpawnManager : MonoBehaviour {

	public Transform[] areas;
	
	public float timeOfLastWave = 0f;
	public float waveInterval = 5f;
	
	public GameObject orc;
	
	private EnemyLogic logic;
	
	void Start () {
	
		GameObject o = GameObject.Find("orc");
		logic = o.GetComponent<EnemyLogic>();
		
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
		
		int index = (int)Mathf.Round(Random.Range(0f, areas.Length-1));
		
		Vector3 targetPosition = logic.getTarget().position;
		
		while(Vector3.Distance( areas[index].position, targetPosition) < 50) {
			index = (int)Mathf.Round(Random.Range(0f, areas.Length-1));
		}

		GameObject go = (GameObject)Instantiate(orc);
		Vector3 offset = new Vector3(Random.Range(-10f,10f),0,Random.Range(-10f,10f));
		go.transform.position = areas[index].position + offset;
		//Debug.Log("Orc instantiated");

	}
}
