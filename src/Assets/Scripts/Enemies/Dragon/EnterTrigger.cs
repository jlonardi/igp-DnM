using UnityEngine;
using System.Collections;

public class EnterTrigger : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		
		transform.parent.GetComponent<TriggerHandler>().handleEnterTrigger(other);
		
	}
}
