using UnityEngine;
using System.Collections;

public class PlayerSounds : MonoBehaviour {

	public AudioClip[] painSounds;
	public AudioClip deathSound;
	public AudioClip[] walkSounds;

	private float walkSoundTimer = 0;

	private CharacterMotor motor;
	
	void Start () {
		motor = GetComponent<CharacterMotor>();
	}

	void Update () {
		if (walkSoundTimer > 0)
			walkSoundTimer -= Time.deltaTime;
		else
			walkSoundTimer = 0;
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

	public void PlayWalkSound() {
		if (!motor.IsJumping() && walkSoundTimer <= 0) {
			int clipNum = Random.Range(0, walkSounds.Length - 1);
			audio.PlayOneShot(walkSounds[clipNum]);
			walkSoundTimer = 0.5f;
		}
	}
}
