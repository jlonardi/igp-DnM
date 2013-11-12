using UnityEngine;

namespace UnitySerialization {	
	[System.Serializable]
	public class sParticleSystem : sComponent {
		//public float duration;
		public float emissionRate;
		public bool enableEmission;
		public float gravityModifier;
		public bool isPaused;
		public bool isPlaying;
		public bool isStopped;
		public bool loop;
		public int particleCount;
		public float playbackSpeed;		
		public bool playOnAwake;
		public uint randomSeed;
		public int safeCollisionEventSize;
		public ParticleSystemSimulationSpace simulationSpace;
		public sColor startColor;
		public float startDelay;
		public float startLifetime;
		public float startRotation;
		public float startSize;
		public float startSpeed;
		public float time;
		
		public sParticleSystem() {
		}
		
		public sParticleSystem(ParticleSystem p) {
		
		}	
	}
}
