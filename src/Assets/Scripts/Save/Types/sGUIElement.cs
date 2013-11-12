using UnityEngine;

namespace UnitySerialization {
	[System.Serializable]
	public class sGUIElement : sBehaviour {
		public sAnimation animation;
		public sAudioSource audio;
		public sCamera camera;
		public sCollider collider;
		//public sConstantForce constantForce;
		public GameObject gameObject;
		public sGUIText guiText;
		public sGUITexture guiTexture;
		//public sHingeJoint hingeJoint;
		public sLight light;
		//public sNetworkView networkView;
		public sParticleEmitter particleEmitter;
		public sParticleSystem particleSystem;
		public sRenderer renderer;
		public sRigidbody rigidbody;
		public string tag;
		public sTransform transform;
		
		public sGUIElement(){
		}
		
		public sGUIElement(GUIElement g){
		}
		
	}
}
