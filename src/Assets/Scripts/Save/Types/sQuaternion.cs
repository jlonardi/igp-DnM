using UnityEngine;

namespace UnitySerialization {	
	[System.Serializable]
	public class sQuaternion
	{
		public float x,y,z,w;
	
		public sQuaternion(Quaternion q) {
			this.x = q.x;
			this.y = q.y;
			this.z = q.z;
			this.w = q.w;
		}
	
		public sQuaternion(float x, float y, float z, float w) {
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}
	
		public Quaternion toQuaternion {
			get {
				return new Quaternion(x, y, z, w); 
			}
		}
	}
} 
