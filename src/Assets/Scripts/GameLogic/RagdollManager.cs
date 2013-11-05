using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RagdollManager : MonoBehaviour {
	public GameObject ragdollPrefab;
	public int maxRagdolls = 50;
	private List<GameObject> bodies = new List<GameObject>();

	public Rigidbody MakeRagdoll(GameObject enemyObject){
		
		// if too many dead bodies, remove the oldest
		if (bodies.Count >= maxRagdolls){
			GameObject killObj = bodies[0];
			bodies.RemoveAt(0);
			Destroy(killObj);
		}
		
		// instantiate ragdoll and copy the hosts position and bone rotations for ragdoll replacement
		Ragdoll ragdoll = (Instantiate(ragdollPrefab, enemyObject.transform.position, enemyObject.transform.rotation) as GameObject).GetComponent<Ragdoll>();
		ragdoll.CopyPose(enemyObject.transform);
		Destroy(enemyObject);		
		
		bodies.Add(ragdoll.gameObject);

		return ragdoll.GetComponentInChildren(typeof(Rigidbody)) as Rigidbody;
		
	}
}
