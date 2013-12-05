using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMotor : MonoBehaviour {
	CharacterController controller;
	
	public float movementSpeed = 0f;
	private float prevMovementSpeed = 0f;   
	private List<Vector3> movementPositions = new List<Vector3>();

	public float pushPower = 10.0f;

	void Awake(){
		controller = GetComponent<CharacterController>();
	}

	void Update(){
		//make sure that our enemy is always on ground
		if (!controller.isGrounded){
			controller.Move(-Vector3.up);
		}
	}

	void FixedUpdate(){
		//calculate current speed for animations
		movementPositions.Add(transform.position);
		if (movementPositions.Count>7){
			prevMovementSpeed = movementSpeed;
			movementSpeed = Vector3.Distance(transform.position, movementPositions[0]) * 30;
			movementPositions.RemoveAt(0);
		}
	}

	// this script pushes all rigidbodies that the character touches
	void OnControllerColliderHit (ControllerColliderHit hit)
	{
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
