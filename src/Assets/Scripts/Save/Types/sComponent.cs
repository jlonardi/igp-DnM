using UnityEngine;

namespace UnitySerialization {
	[System.Serializable]
	public class sComponent : sObject {
		public sAnimation animation;
		public sAudioSource audio;
		public sCamera camera;
		public sCollider collider;
		
		// dont store gameObject or transform, just store some ID of it
		public string gameObjectName;
		public string gameObjectTag;
		//public sGameObject gameObject;
		
		public sGUIText guiText;
		public sGUITexture guiTexture;
		public sLight light;
		public sParticleEmitter particleEmitter;
		public sParticleSystem particleSystem;
		public sRenderer renderer;
		public sRigidbody rigidbody;
		public string tag;
		
		//public sTransform transform;
		//public sConstantForce constantForce;
		//public sHingeJoint hingeJoint;
		//public sNetworkView networkView;
				
		public sComponent(){
		}
	
		public sComponent(Component c){
			
		}
	}
}