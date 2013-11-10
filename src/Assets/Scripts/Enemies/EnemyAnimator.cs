using UnityEngine;
using System.Collections;

public class EnemyAnimator : MonoBehaviour {

	private Animator animator;
	private EnemyPathfind astar;
	private EnemyLogic logic;
	
	
	void Start () {
		animator = GetComponent<Animator>();
		astar = GetComponent<EnemyPathfind>();
		logic = GetComponent<EnemyLogic>();
		animator.SetBool("Jumping",false);
		animator.SetLayerWeight(1,1);
	}
	
	void Update () {
		// animation playback speed is 0 when game is not running
		if (GameManager.instance.gameState!= GameState.RUNNING){
			animator.speed = 0;
		} else {
			animator.speed = 1;
		}		
		animator.SetFloat("Speed", astar.movementSpeed);
		animator.SetBool("Attacking", logic.attacking);
		animator.SetBool("Looting", logic.looting);
	}
	
}
	