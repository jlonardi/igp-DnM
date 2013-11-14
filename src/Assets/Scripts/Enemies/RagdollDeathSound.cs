using UnityEngine;
using System.Collections;

public class RagdollDeathSound : MonoBehaviour {

	public AudioClip[] deathSounds;
	// Use this for initialization
	void Start () {
		int clipNum = Random.Range(0, deathSounds.Length - 1);
		audio.clip = deathSounds[clipNum];
		audio.Play();
	}
}
