using UnityEngine;
using System.Collections;

public enum MovementTransferOnJump {
		None, // The jump is not affected by velocity of floor at all.
		InitTransfer, // Jump gets its initial velocity from the floor, then gradualy comes to a stop.
		PermaTransfer, // Jump gets its initial velocity from the floor, and keeps that velocity until landing.
		PermaLocked // Jump is relative to the movement of the last touched floor and will move together with that floor.
}

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Character/Character Motor")]

public class CharacterMotor : MonoBehaviour {
	
	public bool canControl = true;
	public bool useFixedUpdate = true;

	public float pushPower = 2.0f;
	
	// For the next variables, [System.NonSerialized] tells Unity to not serialize the variable or show it in the inspector view.
	[System.NonSerialized]
	public Vector3 inputMoveDirection = Vector3.zero;
	[System.NonSerialized]
	public bool inputJump = false;
	[System.NonSerialized]
	public bool grounded = true;
	[System.NonSerialized]
	public Vector3 groundNormal = Vector3.zero;	
	
	private Vector3 lastGroundNormal  = Vector3.zero;	
	private Transform tr;	
	private CharacterController controller;
	
	public CharacterMotorMovement movement = new CharacterMotorMovement();	
	public CharacterMotorJumping jumping = new CharacterMotorJumping();
	public CharacterMotorMovingPlatform movingPlatform = new CharacterMotorMovingPlatform();
	public CharacterMotorSliding sliding = new CharacterMotorSliding();

	
	public void Awake () {
		controller = GetComponent<CharacterController>();
		tr = transform;
	}

	private void UpdateFunction () {				
		if(!GameManager.instance.gameRunning || GameManager.instance.paused){ //Mouse look do not work if game is paused or over
			return;
		}
		
		Vector3 velocity = movement.velocity;			// We copy the actual velocity into a temporary variable that we can manipulate.
		velocity = ApplyInputVelocityChange(velocity);	// Update velocity based on input		
		velocity = ApplyGravityAndJumping (velocity);	// Apply gravity and jumping force
		
		// Moving platform support
		Vector3 moveDistance = Vector3.zero;
		if (MoveWithPlatform()) {			
			Vector3 newGlobalPoint = movingPlatform.activePlatform.TransformPoint(movingPlatform.activeLocalPoint);
			moveDistance = (newGlobalPoint - movingPlatform.activeGlobalPoint);
			if (moveDistance != Vector3.zero){
				controller.Move(moveDistance);
			}
			
			// Support moving platform rotation as well:
	        Quaternion newGlobalRotation = movingPlatform.activePlatform.rotation * movingPlatform.activeLocalRotation;
	        Quaternion rotationDiff = newGlobalRotation * Quaternion.Inverse(movingPlatform.activeGlobalRotation);
	        
	        float yRotation = rotationDiff.eulerAngles.y;
	        if (yRotation != 0) {
		        // Prevent rotation of the local up vector
		        tr.Rotate(0, yRotation, 0);
	        }
		}
		
		// Save lastPosition for velocity calculation.
		Vector3 lastPosition = tr.position;
		
		// We always want the movement to be framerate independent.  Multiplying by Time.deltaTime does this.
		Vector3 currentMovementOffset = velocity * Time.deltaTime;
		
		// Find out how much we need to push towards the ground to avoid loosing grouning
		// when walking down a step or over a sharp change in slope.
		float pushDownOffset = Mathf.Max(controller.stepOffset, new Vector3(currentMovementOffset.x, 0f, currentMovementOffset.z).magnitude);
		if (grounded){
			currentMovementOffset -= pushDownOffset * Vector3.up;
		}
		
		// Reset variables that will be set by collision function
		movingPlatform.hitPlatform = null;
		groundNormal = Vector3.zero;
		
	   	// Move our character!
		movement.collisionFlags = controller.Move (currentMovementOffset);
		
		movement.lastHitPoint = movement.hitPoint;
		lastGroundNormal = groundNormal;
		
		if (movingPlatform.enabled && movingPlatform.activePlatform != movingPlatform.hitPlatform) {
			if (movingPlatform.hitPlatform != null) {
				movingPlatform.activePlatform = movingPlatform.hitPlatform;
				movingPlatform.lastMatrix = movingPlatform.hitPlatform.localToWorldMatrix;
				movingPlatform.newPlatform = true;
			}
		}
		
		// Calculate the velocity based on the current and previous position.  
		// This means our velocity will only be the amount the character actually moved as a result of collisions.
		Vector3 oldHVelocity = new Vector3(velocity.x, 0, velocity.z);
		movement.velocity = (tr.position - lastPosition) / Time.deltaTime;
		Vector3 newHVelocity = new Vector3(movement.velocity.x, 0, movement.velocity.z);
		
		// The CharacterController can be moved in unwanted directions when colliding with things.
		// We want to prevent this from influencing the recorded velocity.
		if (oldHVelocity == Vector3.zero) {
			movement.velocity = new Vector3(0f, movement.velocity.y, 0f);
		}
		else {
			float projectedNewVelocity = Vector3.Dot(newHVelocity, oldHVelocity) / oldHVelocity.sqrMagnitude;
			movement.velocity = oldHVelocity * Mathf.Clamp01(projectedNewVelocity) + movement.velocity.y * Vector3.up;
		}
		
		if (movement.velocity.y < velocity.y - 0.001f) {
			if (movement.velocity.y < 0f) {
				// Something is forcing the CharacterController down faster than it should.
				// Ignore this
				movement.velocity.y = velocity.y;
			}
			else {
				// The upwards movement of the CharacterController has been blocked.
				// This is treated like a ceiling collision - stop further jumping here.
				jumping.holdingJumpButton = false;
			}
		}
		
		// We were grounded but just loosed grounding
		if (grounded && !IsGroundedTest()) {
			grounded = false;
			
			// Apply inertia from platform
			if (movingPlatform.enabled &&
				(movingPlatform.movementTransfer == MovementTransferOnJump.InitTransfer ||
				movingPlatform.movementTransfer == MovementTransferOnJump.PermaTransfer)
			) {
				movement.frameVelocity = movingPlatform.platformVelocity;
				movement.velocity += movingPlatform.platformVelocity;
			}
			
			SendMessage("OnFall", SendMessageOptions.DontRequireReceiver);
			// We pushed the character down to ensure it would stay on the ground if there was any.
			// But there wasn't so now we cancel the downwards offset to make the fall smoother.
			tr.position += pushDownOffset * Vector3.up;
		}
		// We were not grounded but just landed on something
		else if (!grounded && IsGroundedTest()) {
			grounded = true;
			jumping.jumping = false;
			SubstractNewPlatformVelocity();
			
			SendMessage("OnLand", SendMessageOptions.DontRequireReceiver);
		}
		
		// Moving platforms support
		if (MoveWithPlatform()) {
			// Use the center of the lower half sphere of the capsule as reference point.
			// This works best when the character is standing on moving tilting platforms. 
			movingPlatform.activeGlobalPoint = tr.position + Vector3.up * (controller.center.y - controller.height*0.5f + controller.radius);
			movingPlatform.activeLocalPoint = movingPlatform.activePlatform.InverseTransformPoint(movingPlatform.activeGlobalPoint);
			
			// Support moving platform rotation as well:
	        movingPlatform.activeGlobalRotation = tr.rotation;
	        movingPlatform.activeLocalRotation = Quaternion.Inverse(movingPlatform.activePlatform.rotation) * movingPlatform.activeGlobalRotation; 
		}
	}
	
	void FixedUpdate () {
		if (movingPlatform.enabled) {
			if (movingPlatform.activePlatform != null) {
				if (!movingPlatform.newPlatform) {
					//Vector3 lastVelocity = movingPlatform.platformVelocity;
					
					movingPlatform.platformVelocity = (
						movingPlatform.activePlatform.localToWorldMatrix.MultiplyPoint3x4(movingPlatform.activeLocalPoint)
						- movingPlatform.lastMatrix.MultiplyPoint3x4(movingPlatform.activeLocalPoint)
					) / Time.deltaTime;
				}
				movingPlatform.lastMatrix = movingPlatform.activePlatform.localToWorldMatrix;
				movingPlatform.newPlatform = false;
			}
			else {
				movingPlatform.platformVelocity = Vector3.zero;	
			}
		}
		
		if (useFixedUpdate)
			UpdateFunction();
	}
	
	void Update () {
		if (!useFixedUpdate)
			UpdateFunction();
	}
	
	private Vector3 ApplyInputVelocityChange(Vector3 velocity) {	
		if (!canControl)
			inputMoveDirection = Vector3.zero;
		
		// Find desired velocity
		Vector3 desiredVelocity;
		if (grounded && TooSteep()) {
			// The direction we're sliding in
			desiredVelocity = new Vector3(groundNormal.x, 0f, groundNormal.z).normalized;
			// Find the input movement direction projected onto the sliding direction
			var projectedMoveDir = Vector3.Project(inputMoveDirection, desiredVelocity);
			// Add the sliding direction, the spped control, and the sideways control vectors
			desiredVelocity = desiredVelocity + projectedMoveDir * sliding.speedControl + (inputMoveDirection - projectedMoveDir) * sliding.sidewaysControl;
			// Multiply with the sliding speed
			desiredVelocity *= sliding.slidingSpeed;
		}
		else
			desiredVelocity = GetDesiredHorizontalVelocity();
		
		if (movingPlatform.enabled && movingPlatform.movementTransfer == MovementTransferOnJump.PermaTransfer) {
			desiredVelocity += movement.frameVelocity;
			desiredVelocity.y = 0f;
		}
		
		if (grounded)
			desiredVelocity = AdjustGroundVelocityToNormal(desiredVelocity, groundNormal);
		else
			velocity.y = 0f;
		
		// Enforce max velocity change
		float maxVelocityChange = GetMaxAcceleration(grounded) * Time.deltaTime;
		Vector3 velocityChangeVector = (desiredVelocity - velocity);
		if (velocityChangeVector.sqrMagnitude > maxVelocityChange * maxVelocityChange) {
			velocityChangeVector = velocityChangeVector.normalized * maxVelocityChange;
		}
		// If we're in the air and don't have control, don't apply any velocity change at all.
		// If we're on the ground and don't have control we do apply it - it will correspond to friction.
		if (grounded || canControl)
			velocity += velocityChangeVector;
		
		if (grounded) {
			// When going uphill, the CharacterController will automatically move up by the needed amount.
			// Not moving it upwards manually prevent risk of lifting off from the ground.
			// When going downhill, DO move down manually, as gravity is not enough on steep hills.
			velocity.y = Mathf.Min(velocity.y, 0f);
		}
		
		return velocity;
	}
	
	private Vector3 ApplyGravityAndJumping (Vector3 velocity) {
		
		if (!inputJump || !canControl) {
			jumping.holdingJumpButton = false;
			jumping.lastButtonDownTime = -100;
		}
		
		if (inputJump && jumping.lastButtonDownTime < 0 && canControl)
			jumping.lastButtonDownTime = Time.time;
		
		if (grounded)
			velocity.y = Mathf.Min(0, velocity.y) - movement.gravity * Time.deltaTime;
		else {
			velocity.y = movement.velocity.y - movement.gravity * Time.deltaTime;
			
			// When jumping up we don't apply gravity for some time when the user is holding the jump button.
			// This gives more control over jump height by pressing the button longer.
			if (jumping.jumping && jumping.holdingJumpButton) {
				// Calculate the duration that the extra jump force should have effect.
				// If we're still less than that duration after the jumping time, apply the force.
				if (Time.time < jumping.lastStartTime + jumping.extraHeight / CalculateJumpVerticalSpeed(jumping.baseHeight)) {
					// Negate the gravity we just applied, except we push in jumpDir rather than jump upwards.
					velocity += jumping.jumpDir * movement.gravity * Time.deltaTime;
				}
			}
			
			// Make sure we don't fall any faster than maxFallSpeed. This gives our character a terminal velocity.
			velocity.y = Mathf.Max (velocity.y, -movement.maxFallSpeed);
		}
			
		if (grounded) {
			// Jump only if the jump button was pressed down in the last 0.2 seconds.
			// We use this check instead of checking if it's pressed down right now
			// because players will often try to jump in the exact moment when hitting the ground after a jump
			// and if they hit the button a fraction of a second too soon and no new jump happens as a consequence,
			// it's confusing and it feels like the game is buggy.
			if (jumping.enabled && canControl && (Time.time - jumping.lastButtonDownTime < 0.2)) {
				grounded = false;
				jumping.jumping = true;
				jumping.lastStartTime = Time.time;
				jumping.lastButtonDownTime = -100;
				jumping.holdingJumpButton = true;
				
				// Calculate the jumping direction
				if (TooSteep())
					jumping.jumpDir = Vector3.Slerp(Vector3.up, groundNormal, jumping.steepPerpAmount);
				else
					jumping.jumpDir = Vector3.Slerp(Vector3.up, groundNormal, jumping.perpAmount);
				
				// Apply the jumping force to the velocity. Cancel any vertical velocity first.
				velocity.y = 0;
				velocity += jumping.jumpDir * CalculateJumpVerticalSpeed (jumping.baseHeight);
				
				// Apply inertia from platform
				if (movingPlatform.enabled &&
					(movingPlatform.movementTransfer == MovementTransferOnJump.InitTransfer ||
					movingPlatform.movementTransfer == MovementTransferOnJump.PermaTransfer)
				) {
					movement.frameVelocity = movingPlatform.platformVelocity;
					velocity += movingPlatform.platformVelocity;
				}
				
				SendMessage("OnJump", SendMessageOptions.DontRequireReceiver);
			}
			else {
				jumping.holdingJumpButton = false;
			}
		}
		
		return velocity;
	}
	
	     
	// this script pushes all rigidbodies that the character touches
	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if (hit.normal.y > 0f && hit.normal.y > groundNormal.y && hit.moveDirection.y < 0f) {
			if ((hit.point - movement.lastHitPoint).sqrMagnitude > 0.001f || lastGroundNormal == Vector3.zero)
				groundNormal = hit.normal;
			else
				groundNormal = lastGroundNormal;
			
			movingPlatform.hitPlatform = hit.collider.transform;
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
	
	private IEnumerator SubstractNewPlatformVelocity() {
		// When landing, subtract the velocity of the new ground from the character's velocity
		// since movement in ground is relative to the movement of the ground.
		if (movingPlatform.enabled &&
			(movingPlatform.movementTransfer == MovementTransferOnJump.InitTransfer ||
			movingPlatform.movementTransfer == MovementTransferOnJump.PermaTransfer)
		) {
			// If we landed on a new platform, we have to wait for two FixedUpdates
			// before we know the velocity of the platform under the character
			if (movingPlatform.newPlatform) {
				Transform platform  = movingPlatform.activePlatform;
				yield return new WaitForFixedUpdate();
				yield return new WaitForFixedUpdate();
				if (grounded && platform == movingPlatform.activePlatform)
					yield return 1;
			}
			movement.velocity -= movingPlatform.platformVelocity;
		}
	}
	
	private bool MoveWithPlatform() {
		return (
			movingPlatform.enabled
			&& (grounded || movingPlatform.movementTransfer == MovementTransferOnJump.PermaLocked)
			&& movingPlatform.activePlatform != null
		);
	}
	
	private Vector3 GetDesiredHorizontalVelocity () {
		// Find desired velocity
		Vector3 desiredLocalDirection = tr.InverseTransformDirection(inputMoveDirection);
		float maxSpeed = MaxSpeedInDirection(desiredLocalDirection);
		if (grounded) {
			// Modify max speed on slopes based on slope speed multiplier curve
			var movementSlopeAngle = Mathf.Asin(movement.velocity.normalized.y)  * Mathf.Rad2Deg;
			maxSpeed *= movement.slopeSpeedMultiplier.Evaluate(movementSlopeAngle);
		}
		return tr.TransformDirection(desiredLocalDirection * maxSpeed);
	}
	
	private Vector3 AdjustGroundVelocityToNormal(Vector3 hVelocity, Vector3 groundNormal){
		Vector3 sideways = Vector3.Cross(Vector3.up, hVelocity);
		return Vector3.Cross(sideways, groundNormal).normalized * hVelocity.magnitude;
	}
	
	private bool IsGroundedTest () {
		return (groundNormal.y > 0.01);
	}
	
	public float GetMaxAcceleration(bool grounded) {
		// Maximum acceleration on ground and in air
		if (grounded)
			return movement.maxGroundAcceleration;
		else
			return movement.maxAirAcceleration;
	}
	
	public float CalculateJumpVerticalSpeed(float targetJumpHeight) {
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt (2 * targetJumpHeight * movement.gravity);
	}
	
	public bool IsJumping() {
		return jumping.jumping;
	}
	
	public bool IsSliding() {
		return (grounded && sliding.enabled && TooSteep());
	}
	
	public bool IsTouchingCeiling() {
		return (movement.collisionFlags & CollisionFlags.CollidedAbove) != 0;
	}
	
	public bool IsGrounded() {
		return grounded;
	}
	
	public bool TooSteep() {
		return (groundNormal.y <= Mathf.Cos(controller.slopeLimit * Mathf.Deg2Rad));
	}
	
	public Vector3 GetDirection() {
		return inputMoveDirection;
	}
	
	public void SetControllable(bool controllable) {
		canControl = controllable;
	}
	
	// Project a direction onto elliptical quater segments based on forward, sideways, and backwards speed.
	// The function returns the length of the resulting vector.
	public float MaxSpeedInDirection(Vector3 desiredMovementDirection){
		if (desiredMovementDirection == Vector3.zero)
			return 0f;
		else {
			float zAxisEllipseMultiplier = (desiredMovementDirection.z > 0f ? movement.maxForwardSpeed : movement.maxBackwardsSpeed) / movement.maxSidewaysSpeed;
			Vector3 temp = new Vector3(desiredMovementDirection.x, 0f, desiredMovementDirection.z / zAxisEllipseMultiplier).normalized;
			float length = new Vector3(temp.x, 0f, temp.z * zAxisEllipseMultiplier).magnitude * movement.maxSidewaysSpeed;
			return length;
		}
	}
	
	public void SetVelocity(Vector3 velocity) {
		grounded = false;
		movement.velocity = velocity;
		movement.frameVelocity = Vector3.zero;
		SendMessage("OnExternalVelocity");
	}	
}
