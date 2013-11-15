using UnityEngine;
using System.Collections;

public class PlayerSounds : MonoBehaviour {
	//use singleton since only we need one instance of this class
	public static PlayerSounds instance;
	
	public AudioClip[] painSounds;
	public AudioClip deathSound;
	public AudioClip[] walkSounds;
	public AudioClip[] jumpSounds;

	private float walkSoundTimer = 0;

	private CharacterMotor motor;
	
	public void Awake()
	{
		PlayerSounds.instance = this;
	}	
	
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

	public void PlayJumpSound() {
		int clipNum = Random.Range(0, jumpSounds.Length - 1);
		audio.PlayOneShot(jumpSounds[clipNum]);
	}
}
