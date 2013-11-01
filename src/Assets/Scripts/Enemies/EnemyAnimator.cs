using UnityEngine;
using System.Collections;

public class EnemyAnimator : MonoBehaviour {

	private Animator animator;
	private AstarAI astar;
	
	
	void Start () {
		animator = GetComponent<Animator>();
		astar = GetComponent<AstarAI>();
		animator.speed = 1;	// animation playback speed
		animator.SetBool("Jumping",false);
		animator.SetLayerWeight(1,1);
	}
	
	void Update () {
		animator.SetFloat("Speed", astar.movementSpeed);
	}
		
	// Increase towards target
	private float IncrementTowards(float current, float target, float acceleration){
		if (current == target) {
			return current;
		}
		else {
			float dir = Mathf.Sign(target - current); // determine increase or decrease direction
			current += acceleration * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target - current))? current: target; // if current passed target then return target, otherwise return current
		}
	}
	
}
	