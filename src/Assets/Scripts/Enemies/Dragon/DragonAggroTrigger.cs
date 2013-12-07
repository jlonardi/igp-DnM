using UnityEngine;
using System.Collections;

public class DragonAggroTrigger : MonoBehaviour {

	void OnTriggerEnter (Collider other) {

		transform.parent.GetComponent<TriggerHandler>().handleAggroTrigger(other);

	}
}
