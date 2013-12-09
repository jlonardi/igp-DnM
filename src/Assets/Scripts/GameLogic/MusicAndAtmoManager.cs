using UnityEngine;
using System.Collections;

public class MusicAndAtmoManager : MonoBehaviour {
	//use singleton since only we need one instance of this class
	public static MusicAndAtmoManager instance;

	public AudioClip battleMusic;
	public AudioClip beforeBattleMusic;
	public AudioClip ambience;
	public AudioClip deathMusic;

	private AudioSource ambienceSource;
	private AudioSource musicSource;
	private GameManager game;

	private bool onBattle = false;

	void Awake() {
		MusicAndAtmoManager.instance = this;
		AudioSource[] aSources = GetComponents<AudioSource>();
		ambienceSource = aSources[0];
		musicSource = aSources[1];
	}

	void Start() {
		game = GameManager.instance;
	}

	void Update() {
		if (game.gameState == GameState.GAME_OVER || game.gameState == GameState.HIGHSCORE_DIALOG && onBattle)
		{
			musicSource.Stop();
			ambienceSource.Stop();
			musicSource.loop = false;
			musicSource.clip = deathMusic;
			musicSource.Play();
			onBattle = false;
		}
	}

	public void PlayBattleMusic() {
		onBattle = true;
		musicSource.Stop();
		musicSource.clip = battleMusic;
		musicSource.Play();
	}
}
