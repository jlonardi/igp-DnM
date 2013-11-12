using UnityEngine;
using System.Collections.Generic;

namespace UnitySerialization {	
	[System.Serializable]
	public class sTransform : sComponent {
		public int childCount;
		//public sVector3 eulerAngles; use rotation instead for storing rotation values
		public sVector3 forward;
		public bool hasChanged;
		//public sVector3 localEulerAngles;
		public sVector3 localPosition;
		public sQuaternion localRotation;
		public sVector3 localScale;
		//public sMatrix4x4 localToWorldMatrix;
		//public sVector3 lossyScale;
		
		// dont store parent or root, just store some ID of it
		public int parentChildCount;
		public string parentName;
		public string parentTag;
		//public sTransform parent;
		public int rootChildCount;
		public string rootName;
		public string rootTag;
		//public sTransform root;
		
		public sVector3 position;
		public sVector3 right;
		public sQuaternion rotation;
		public sVector3 up;
		//public sMatrix4x4 worldToLocalMatrix;
		
		public sTransform(){
		}		

		public sTransform(Transform t){
			forward = t.forward.Serializable();
			hasChanged = t.hasChanged;
			localPosition = t.localPosition.Serializable();
			localRotation = t.localRotation.Serializable();
			localScale = t.localScale.Serializable();
			name = t.name;
			position = t.position.Serializable();
			right = t.right.Serializable();
			rotation = t.rotation.Serializable();
			tag = t.tag;
			up = t.up.Serializable();
			
			//read-only:
			childCount = t.childCount;
			if (t.parent != null){
				parentChildCount = t.parent.childCount;
				parentName = t.parent.name;
				parentTag = t.parent.tag;
			}
			if (t.root != null){
				rootChildCount = t.root.childCount;
				rootName = t.root.name;
				rootTag = t.root.tag;				
			}
		}
		
		public void toTransform(GameObject go) {
			go.transform.forward = forward.toVector3;
			go.transform.hasChanged = hasChanged;
			go.transform.localPosition = localPosition.toVector3;
			go.transform.localRotation = localRotation.toQuaternion;
			go.transform.localScale = localScale.toVector3;
			go.transform.name = name;
			go.transform.position = position.toVector3;
			go.transform.right = right.toVector3;
			go.transform.rotation = rotation.toQuaternion;
			go.transform.tag = tag;
			go.transform.up = up.toVector3;
			//read-only:
			//go.transform.childCount = childCount;
			//go.transform.gameObject = gameObject.toGameObject;
			//go.transform.lossyScale = lossyScale.toVector3;
			//go.transform.parent = parent.toTransform;
			//go.transform.root = root.toTransform;
			//go.transform.transform = transform.toTransform;
		}
	}
}