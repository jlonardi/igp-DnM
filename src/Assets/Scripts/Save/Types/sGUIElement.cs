using UnityEngine;

namespace UnitySerialization {
	[System.Serializable]
	public class sGUIElement : sBehaviour {
		//public sConstantForce constantForce;
		public GameObject gameObject;
		//public sHingeJoint hingeJoint;
		//public sNetworkView networkView;
		public sTransform transform;
		
		public sGUIElement(){
		}
		
		public sGUIElement(GUIElement g){
		}
		
	}
}
