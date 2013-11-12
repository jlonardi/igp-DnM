using UnityEngine;

namespace UnitySerialization {	
	[System.Serializable]
	public class sVector2 {
		public float x,y;
	
		public sVector2(Vector2 v2){
			this.x = v2.x;
			this.y = v2.y;
		}
		
		public sVector2(float x, float y){
			this.x = x;
			this.y = y;
		}
		
		public Vector3 toVector2 { 
			get {
				return new Vector2(x, y);
			} 
		}
	}
}