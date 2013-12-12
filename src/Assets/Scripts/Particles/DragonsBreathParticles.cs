using UnityEngine;
using System.Collections;

public class DragonsBreathParticles : MonoBehaviour {
	public int destroyAfter = 100;
	private ParticleEmitter emitter;
	private GameManager game;

	void Start(){
		game = GameManager.instance;
		emitter = GetComponent<ParticleEmitter>();
	}

	void Update(){

		emitter.emit = game.dragon.breathFire;

		int count = emitter.particles.Length;
		if (count > destroyAfter){
			int offset = count - destroyAfter;
			Particle[] tempParticles = new Particle[destroyAfter];
			for (int i=0; i<destroyAfter; i++){
				tempParticles[i] = emitter.particles[i+offset];
			}
			emitter.particles = tempParticles;
		}
	}

	// this applies fire particle damage to player
	void OnParticleCollision(GameObject other){
		if(other.tag == "Player"){
			game.player.TakeDamage(game.player.GetFireDamage(), DamageType.FIRE);
		}
	}
}
