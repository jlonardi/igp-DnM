using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RagdollManager : MonoBehaviour {
	//use singleton since only we need one instance of this class
	public static RagdollManager instance;

	public GameObject orcRagdollPrefab;
	public GameObject lizardRagdollPrefab;
	public GameObject wolfRagdollPrefab;
	public int maxRagdolls = 20;
	private GameObject player;

	private List<GameObject> bodies = new List<GameObject>();

	void Awake() {
		RagdollManager.instance = this;
	}	

	void Start() {
		player = GameObject.Find("Player");
	}
	
	public GameObject MakeRagdoll(EnemyType enemyType, GameObject enemyObject, bool copyPose){		
		GameObject ragdollPrefab;

		// if too many dead bodies, remove the oldest ragdoll which isn't on rendered on current view
		if (bodies.Count >= maxRagdolls){
			foreach (GameObject body in bodies){
				Renderer bodyRenderer = body.GetComponentInChildren<Renderer>();
				if (bodyRenderer.isVisible == false){
					bodies.Remove(body);
					Destroy(body);
					break;
				}
			}
		}

		// if previous way wasn't possible, remove the body furthest away
		if (bodies.Count >= maxRagdolls){
			int farEnemyIndex = 0;
			float farEnemyDistance = 0f;
			float enemyDistance;
			for (int i=0; i< bodies.Count; i++){
				enemyDistance = Vector3.Distance(bodies[i].transform.position, player.transform.position);
				if (enemyDistance > farEnemyDistance){
					farEnemyDistance = enemyDistance;
					farEnemyIndex = i;
				}
			}
			GameObject killObj = bodies[farEnemyIndex];
			bodies.RemoveAt(farEnemyIndex);
			Destroy(killObj);
		}


		// select correct ragdoll prefab by enemytype
		switch(enemyType) {
		case EnemyType.LIZARD:
			ragdollPrefab = lizardRagdollPrefab;
			break;
		case EnemyType.WEREWOLF:
			ragdollPrefab = wolfRagdollPrefab;
			break;
		case EnemyType.ORC:
		default:
			ragdollPrefab = orcRagdollPrefab;
			break;			
		}

		// instantiate ragdoll and copy the hosts position and bone rotations for ragdoll replacement
		GameObject ragdollGo = (GameObject)Instantiate(ragdollPrefab, enemyObject.transform.position, 
		                                               enemyObject.transform.rotation);

		// add body to ragdoll-list so we can keep track of them
		bodies.Add(ragdollGo);

		// copy pose of enemy used as ragdoll base
		if (copyPose){
			Ragdoll ragdoll = (Ragdoll)ragdollGo.GetComponent<Ragdoll>();
			ragdoll.CopyPose(enemyObject.transform);
		}
		// destroy to old object
		Destroy(enemyObject);

		return ragdollGo;			
	}

}
