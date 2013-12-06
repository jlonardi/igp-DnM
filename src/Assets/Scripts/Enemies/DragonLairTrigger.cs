using UnityEngine;
using System.Collections;

public class DragonLairTrigger : MonoBehaviour {

	void OnTriggerEnter (Collider other) {

		transform.parent.GetComponent<TriggerHandler>().handleTrigger(other);
	}
}
