using UnityEngine;
using System.Collections;

[System.Serializable]	
public class CharacterMotorMovement {
	// The maximum horizontal speed when moving
	public float maxForwardSpeed = 10.0f;
	public float maxSidewaysSpeed = 10.0f;
	public float maxBackwardsSpeed = 10.0f;
	
	//time to rest before new sprint (s)
	public float sprintCoolDown = 3;
	//how long can player sprint (s)
	public float sprintDuration = 10;

	public float sprintSpeedMultiplier = 1.6f;

	// Curve for multiplying speed based on slope (negative = downwards)
	public AnimationCurve slopeSpeedMultiplier = new AnimationCurve(new Keyframe(-90, 1), new Keyframe(0, 1), new Keyframe(90, 0));
	
	// How fast does the character change speeds?  Higher is faster.
	public float maxGroundAcceleration = 30.0f;
	public float maxAirAcceleration = 20.0f;

	// The gravity for the character
	public float gravity = 10.0f;
	public float maxFallSpeed = 20.0f;
	
	// The last collision flags returned from controller.Move
	[System.NonSerialized]
	public CollisionFlags collisionFlags; 

	// We will keep track of the character's current velocity,
	[System.NonSerialized]
	public Vector3 velocity;
	
	// This keeps track of our current velocity while we're not grounded
	[System.NonSerialized]
	public Vector3 frameVelocity = Vector3.zero;
	
	[System.NonSerialized]
	public Vector3 hitPoint = Vector3.zero;
	
	[System.NonSerialized]
	public Vector3 lastHitPoint = new Vector3(Mathf.Infinity, 0, 0);
}