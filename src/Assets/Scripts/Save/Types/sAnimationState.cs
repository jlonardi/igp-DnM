using UnityEngine;

namespace UnitySerialization {
	[System.Serializable]
	public class sAnimationState : sTrackedReference {	
		public AnimationBlendMode blendMode;
		public sAnimationClip clip;
		public bool enabled;
		public int layer;
		public float length;
		public string name;
		public float normalizedSpeed;
		public float normalizedTime;
		public float speed;
		public float time;
		public float weight;
		public WrapMode wrapMode;
		
		public sAnimationState(){
		}
	
		public sAnimationState(AnimationState a){
		}
	}
}