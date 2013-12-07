using UnityEngine;
using System.Collections;

public class DragonAnimator : MonoBehaviour {
	private Animator animator;
	private Dragon dragon;		
		
	void Start () {
		animator = GetComponent<Animator>();
		dragon = GetComponent<Dragon>();
		animator.SetBool("Flying",false);
		animator.SetBool("Breath Fire", false);
	}
		
	void Update () {
		// animation playback speed is 0 when game is not running
		if (GameManager.instance.gameState!= GameState.RUNNING){
			animator.speed = 0;
		} else {
			animator.speed = 1;
		}		
		animator.SetBool("Flying", dragon.flying);
		animator.SetBool("Breath Fire", dragon.breathFire);
	}

}
