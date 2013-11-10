using UnityEngine;
using System.Collections;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Character/Character Motor")]
public class EnemyMotor : MonoBehaviour {
	
	public bool canControl = true;
	public bool useFixedUpdate = true;

	public float pushPower = 2.0f;
	private CharacterController controller;
	private Transform tr;	
	
	public Vector3 groundNormal = Vector3.zero;	
	private Vector3 lastGroundNormal  = Vector3.zero;	
	public CharacterMotorMovement movement = new CharacterMotorMovement();	
	
	
	public void Awake () {
		controller = GetComponent<CharacterController>();
		tr = transform;
	}

	void Update () {
		if(GameManager.instance.gameState != GameState.RUNNING){
			return;
		}
		lastGroundNormal = groundNormal;
		movement.lastHitPoint = movement.hitPoint;
	}
	     
	// this script pushes all rigidbodies that the character touches
	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if (hit.normal.y > 0f && hit.normal.y > groundNormal.y && hit.moveDirection.y < 0f) {
			if ((hit.point - movement.lastHitPoint).sqrMagnitude > 0.001f || lastGroundNormal == Vector3.zero)
				groundNormal = hit.normal;
			else
				groundNormal = lastGroundNormal;
			
			movement.hitPoint = hit.point;
			movement.frameVelocity = Vector3.zero;
		}
	    Rigidbody body = hit.collider.attachedRigidbody;
	     
	    // no rigidbody
	    if (body == null || body.isKinematic) { return; }
	     
	    // We dont want to push objects below us
	    if (hit.moveDirection.y < -0.3f) { return; }
	     
	    // Calculate push direction from move direction,
	    // we only push objects to the sides never up and down
	    Vector3 pushDir = new Vector3 (hit.moveDirection.x, 0f, hit.moveDirection.z);
	     
	    // If you know how fast your character is trying to move,
	    // then you can also multiply the push velocity by that.
	     
	    // Apply the push
	    body.velocity = pushDir * pushPower;
	}
}
