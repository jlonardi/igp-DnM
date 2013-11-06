using UnityEngine;
using System.Collections;

public class EnemySpawnManager : MonoBehaviour {
	//use singleton since only we need once instance of this class
	public static EnemySpawnManager instance;
    public void Awake()
    {
        EnemySpawnManager.instance = this;
    }	

	public GameObject orcPrefab;
	public Transform[] areas;	
	public float timeOfLastWave = 0f;
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
		
		if( (timeOfLastWave + waveInterval) < Time.time) {
			//Debug.Log("New wave needed");
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
		
		GameObject newOrc = (GameObject)Instantiate(orcPrefab, areas[index].position + offset, orcPrefab.transform.rotation);

		RaycastHit hit = new RaycastHit();
		Ray ray = new Ray(newOrc.transform.position, -Vector3.up);
		
        Physics.Raycast(ray,out hit);
		newOrc.transform.position = new Vector3(newOrc.transform.position.x, hit.point.y + 3, newOrc.transform.position.z);
		//Debug.Log("Orc instantiated");

	}
}
