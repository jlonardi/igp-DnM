using UnityEngine;

namespace UnitySerialization {
	[System.Serializable]
	public class sAnimation : sBehaviour {
		public bool animatePhysics;
		public sAnimationClip clip;
		public AnimationCullingType cullingType;
		public bool isPlaying;
		public sBounds localBounds;
		public bool playAutomatically;
		public WrapMode wrapMode;
		// this[string], we probably don't need dictionary here for serialization
				
		public sAnimation(){
		}
		
		public sAnimation(Animation a){
			
		}
	}
}