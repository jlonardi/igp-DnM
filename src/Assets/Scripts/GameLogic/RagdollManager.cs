using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RagdollManager : MonoBehaviour {
	//use singleton since only we need once instance of this class
	public static RagdollManager instance;
    public void Awake()
    {
        RagdollManager.instance = this;
    }	

	public GameObject orcRagdollPrefab;
	public int maxRagdolls = 50;

	public List<GameObject> bodies = new List<GameObject>();

	
	public void ClearBodies(){
		foreach(GameObject go in bodies){
			Destroy(go);
		}
		bodies.Clear();
	}

	public Rigidbody MakeRagdoll(EnemyType enemyType, GameObject enemyObject){		
		// if too many dead bodies, remove the oldest
		if (bodies.Count >= maxRagdolls){
			GameObject killObj = bodies[0];
			bodies.RemoveAt(0);
			Destroy(killObj);
		}
		
		// instantiate ragdoll and copy the hosts position and bone rotations for ragdoll replacement
		Ragdoll ragdoll = instantiateRagdoll(enemyType, enemyObject.transform.position, enemyObject.transform.rotation).GetComponent<Ragdoll>();
		ragdoll.CopyPose(enemyObject.transform);
		Destroy(enemyObject);		
		
		return ragdoll.GetComponentInChildren(typeof(Rigidbody)) as Rigidbody;		
	}
	
	public GameObject instantiateRagdoll(EnemyType enemyType, Vector3 position, Quaternion rotation){
		GameObject ragdollPrefab;
		switch(enemyType) {
        	case EnemyType.ORC:
			default:
				ragdollPrefab = orcRagdollPrefab;
				break;			
   		}
		GameObject ragdollObject = (GameObject)Instantiate(ragdollPrefab, position, rotation);		
		bodies.Add(ragdollObject.gameObject);
		return ragdollObject;
	}
	
}
