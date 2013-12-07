using UnityEngine;
using System.Collections;

public class DragonKillTrigger : MonoBehaviour {

	void OnTriggerEnter (Collider other) {

		transform.parent.GetComponent<TriggerHandler>().handleKillTrigger(other);
	}
}
