using UnityEngine;

namespace UnitySerialization {
	[System.Serializable]
	public class sBehaviour : sComponent {
		public bool enabled;
		
		public sBehaviour() {
		}
		
		public sBehaviour(Behaviour b) {
			enabled = b.enabled;
		}
	}
}