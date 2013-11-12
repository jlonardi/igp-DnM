using UnityEngine;

namespace UnitySerialization {	
	[System.Serializable]
	public class sParticleEmitter : sComponent {
		public float angularVelocity;
		public bool emit;
		public float emitterVelocityScale;
		public bool enabled;
		public sVector3 localVelocity;
		public float maxEmission;
		public float maxEnergy;
		public float maxSize;
		public float minEmission;
		public float minEnergy;
		public float minSize;
		public int particleCount;
		//public sParticle[] particles;
		public float rndAngularVelocity;
		public bool rndRotation;
		public sVector3 rndVelocity;
		public bool useWorldSpace;
		public sVector3 worldVelocity;
		
		public sParticleEmitter() {
		}
		
		public sParticleEmitter(ParticleEmitter p) {
		
		}
	}
}
