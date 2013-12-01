using UnityEngine;
using System.Collections;

public class MusicAndAtmoManager : MonoBehaviour {
	//use singleton since only we need one instance of this class
	public static MusicAndAtmoManager instance;

	public AudioClip battleMusic;
	public AudioClip beforeBattleMusic;
	public AudioClip ambience;

	private AudioSource ambienceSource;
	private AudioSource musicSource;

	void Awake() {
		MusicAndAtmoManager.instance = this;
	}

	void Start() {
		AudioSource[] aSources = GetComponents<AudioSource>();
		ambienceSource = aSources[0];
		musicSource = aSources[1];
	}

	public void PlayBattleMusic() {
		musicSource.Stop();
		musicSource.clip = battleMusic;
		musicSource.Play();
	}
}
