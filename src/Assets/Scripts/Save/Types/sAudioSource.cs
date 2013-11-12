using UnityEngine;

namespace UnitySerialization {
	[System.Serializable]
	public class sAudioSource : sBehaviour {
		public bool bypassEffects;
		//public sAudioClip clip;
		public float dopplerLevel;
		public bool ignoreListenerPause;
		public bool ignoreListenerVolume;
		public bool isPlaying;
		public bool loop;
		public float maxDistance;
		public float minDistance;
		public bool mute;
		public float pan;
		public float panLevel;
		public float pitch;
		public bool playOnAwake;
		public int priority;
		//public sAudioRolloffMode rolloffMode;
		public float spread;
		public float time;
		public int timeSamples;
		//public sAudioVelocityUpdateMode velocityUpdateMode;
		public float volume;
		
		public sAudioSource(){
			
		}
		public sAudioSource(AudioSource a){
			
		}
	}
}
