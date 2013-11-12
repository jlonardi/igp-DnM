using UnityEngine;

namespace UnitySerialization {
	[System.Serializable]
	public class sMatrix4x4 {
		//public sMatrix4x4 inverse;
		public bool isIdentity;
		//public this[4,4];
		//public sMatrix4x4 transpose;
		
		public sMatrix4x4() {
		}
		
		public sMatrix4x4(Matrix4x4 m) {
		
		}				
	}
}
