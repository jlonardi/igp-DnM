using UnityEngine;
using System.Collections;

public class DragonsBreathParticles : MonoBehaviour {
	public int destroyAfter = 100;
	public ParticleEmitter emitter;
	private Dragon dragon;

	void Update () {
		if (dragon == null){
			dragon = this.transform.root.GetComponentInChildren<Dragon>();
		}

		emitter.emit = dragon.breathFire;

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
}
