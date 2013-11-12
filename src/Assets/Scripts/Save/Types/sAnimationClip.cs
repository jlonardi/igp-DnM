using UnityEngine;

namespace UnitySerialization {
	[System.Serializable]
	public class sAnimationClip : sMotion {
		public float frameRate;
		public float length;
		public sBounds localBounds;
		public WrapMode wrapMode;
		
		public sAnimationClip(){
		}
	
		public sAnimationClip(AnimationClip a){
			
		}
	}
}