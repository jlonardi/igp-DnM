using UnityEngine;

namespace UnitySerialization {
	[System.Serializable]
	public class sCollider : sComponent {
		public sRigidbody attachedRigidbody;
		public sBounds bounds;
		public bool enabled;
		public bool isTrigger;
		//public sPhysicMaterial material;
		//public sPhysicMaterial sharedMaterial;
		
		public sCollider() {
		}
		
		public sCollider(Collider c) {
		
		}		
	}
}
