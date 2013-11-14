using UnityEngine;
using System.Collections;

public class PlayerSounds : MonoBehaviour {

	public AudioClip[] painSounds;
	public AudioClip deathSound;
	
	void Start () {
	
	}

	void Update () {
	
	}

	public void PlayPainSound() {
		
		if (!audio.isPlaying) {
			int clipNum = Random.Range(0, painSounds.Length - 1);
			audio.clip = painSounds[clipNum];
			audio.Play();
		}
	}

	public void PlayDeathSound() {
		audio.clip = deathSound;
		audio.Play();
	}
}
