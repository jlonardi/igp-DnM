using UnityEngine;

namespace UnitySerialization {
	[System.Serializable]
	public class sColor : MonoBehaviour {
		public float a;
		public float b;
		public float g;	
		public float r;
		//public color gamma;
		//public float grayscale (Read only)
		//public sColor linear
		//this[int]	
	
		public sColor() {
		}
		
		public sColor(Color c) {
		
		}		
	}
}