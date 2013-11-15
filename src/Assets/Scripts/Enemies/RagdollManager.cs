using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RagdollManager : MonoBehaviour {
	//use singleton since only we need one instance of this class
	public static RagdollManager instance;

	public GameObject orcRagdollPrefab;
	public int maxRagdolls = 20;

	private List<GameObject> bodies = new List<GameObject>();

	void Awake()
	{
		RagdollManager.instance = this;
	}	
	
	public GameObject MakeRagdoll(EnemyType enemyType, GameObject enemyObject, bool copyPose){		
		GameObject ragdollPrefab;

		// if too many dead bodies, remove the oldest
		if (bodies.Count >= maxRagdolls){
			GameObject killObj = bodies[0];
			bodies.RemoveAt(0);
			Destroy(killObj);
		}

		// select correct ragdoll prefab by enemytype
		switch(enemyType) {
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
