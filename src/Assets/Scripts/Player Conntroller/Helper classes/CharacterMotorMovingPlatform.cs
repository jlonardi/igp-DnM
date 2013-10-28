using UnityEngine;
using System.Collections;

[System.Serializable]
public class CharacterMotorMovingPlatform {
	public bool enabled = true;
	
	public MovementTransferOnJump movementTransfer = MovementTransferOnJump.PermaTransfer;	
	[System.NonSerialized]
	public Transform hitPlatform;
	
	[System.NonSerialized]
	public Transform activePlatform;
	
	[System.NonSerialized]
	public Vector3 activeLocalPoint;
	
	[System.NonSerialized]
	public Vector3 activeGlobalPoint;
	
	[System.NonSerialized]
	public Quaternion activeLocalRotation;
	
	[System.NonSerialized]
	public Quaternion activeGlobalRotation;
	
	[System.NonSerialized]
	public Matrix4x4 lastMatrix;
	
	[System.NonSerialized]
	public Vector3 platformVelocity;
	
	[System.NonSerialized]
	public bool newPlatform;
}