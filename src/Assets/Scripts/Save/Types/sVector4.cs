using UnityEngine;

namespace UnitySerialization {	
	[System.Serializable]
	public class sVector4
	{
		public float x,y,z,w;
	
		public sVector4(sVector4 v4) {
			this.x = v4.x;
			this.y = v4.y;
			this.z = v4.z;
			this.w = v4.w;
		}
	
		public sVector4(float x, float y, float z, float w) {
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}
	
		public Vector4 toVector4 {
			get {
				return new Vector4(x, y, z, w); 
			}
		}
	}
} 

