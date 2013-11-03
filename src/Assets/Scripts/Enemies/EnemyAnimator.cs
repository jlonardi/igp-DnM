using UnityEngine;
using System.Collections;

public class EnemyAnimator : MonoBehaviour {

	private Animator animator;
	private EnemyAI astar;
	
	
	void Start () {
		animator = GetComponent<Animator>();
		astar = GetComponent<EnemyAI>();
		animator.speed = 1;	// animation playback speed
		animator.SetBool("Jumping",false);
		animator.SetLayerWeight(1,1);
	}
	
	void Update () {
		animator.SetFloat("Speed", astar.movementSpeed);
	}
	
}
	