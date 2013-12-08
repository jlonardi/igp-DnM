using UnityEngine;
using System.Collections;

public class PlayerSounds : MonoBehaviour {
	//use singleton since only we need one instance of this class
	public static PlayerSounds instance;
	
	public AudioClip[] painSounds;
	public AudioClip deathSound;
	public AudioClip[] walkSounds;
	public AudioClip[] jumpSounds;
	public AudioClip armorPickupSound;
	public AudioClip gunPickupSound;

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
			int clipNum = Random.Range(0, painSounds.Length);
			audio.clip = painSounds[clipNum];
			audio.Play();
		}
	}

	public void PlayDeathSound() {
		audio.clip = deathSound;
		audio.Play();
	}

	public void PlayArmorPickupSound() {
		audio.PlayOneShot(armorPickupSound);
	}

	public void PlayGunPickupSound() {
		audio.PlayOneShot(gunPickupSound);
	}

	public void PlayWalkSound() {
		if (!motor.IsJumping() && walkSoundTimer <= 0) {
			int clipNum = Random.Range(0, walkSounds.Length);
			audio.PlayOneShot(walkSounds[clipNum], 0.6f);

			if (motor.IsSprinting())
				walkSoundTimer = 0.3f;
			else
				walkSoundTimer = 0.45f;
		}
	}

	public void PlayJumpSound() {
		int clipNum = Random.Range(0, jumpSounds.Length);
		audio.PlayOneShot(jumpSounds[clipNum], 0.6f);
	}
}
