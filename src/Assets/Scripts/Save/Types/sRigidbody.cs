using UnityEngine;

namespace UnitySerialization {	
	[System.Serializable]
	public class sRigidbody : sComponent {
		public float angularDrag;
		public sVector3 angularVelocity;
		public sVector3 centerOfMass;
		public CollisionDetectionMode collisionDetectionMode;
		public RigidbodyConstraints constraints;
		public bool detectCollisions;
		public float drag;
		public bool freezeRotation;
		public sVector3 inertiaTensor;
		public sQuaternion inertiaTensorRotation;
		public RigidbodyInterpolation interpolation;
		public bool isKinematic;
		public float mass;
		public float maxAngularVelocity;
		public sVector3 position;
		public sQuaternion rotation;
		public float sleepAngularVelocity;
		public float sleepVelocity;
		public int solverIterationCount;
		public bool useConeFriction;
		public bool useGravity;
		public sVector3 velocity;
		public sVector3 worldCenterOfMass;
		
		public sRigidbody() {
		}
		
		public sRigidbody(Rigidbody r) {
		
		}	
	}	
}
