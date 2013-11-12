using UnityEngine;
using System.Collections.Generic;

namespace UnitySerialization {
	[System.Serializable]
	public class sGameObject : sObject {
		public bool activeInHierarchy;
		public bool activeSelf;
		public sAnimation animation;
		//public sAudioSource audio;
		//public sCamera camera;
		public sCollider collider;
		//public sConstantForce constantForce;
		public sGUIText guiText;
		public sGUITexture guiTexture;
		//public sHingeJoint hingeJoint
		//public bool isStatic;
		public int layer;
		public sLight light;
		//public sNetworkView networkView
		public sParticleEmitter particleEmitter;
		public sParticleSystem particleSystem;
		public sRenderer renderer;
		public sRigidbody rigidbody;
		public string tag;
		public sTransform transform;	
		
		// store child transforms too
		public List<sTransform> transforms;
	
	
		public sGameObject(){
		}
		
		public sGameObject(GameObject go) {
			activeInHierarchy = go.activeInHierarchy;
			activeSelf = go.activeSelf;
			layer = go.layer;
			name = go.name;
			tag = go.tag;
			
			if (go.transform != null){
				transform = new sTransform(go.transform);
				
				// put all transforms into list recursively
				transforms = new List<sTransform>();
				storeTransforms(go.transform, transforms);
			}
		}

		// this builds a sTransform list of all transforms attached to the gameobject
		private void storeTransforms(Transform t_parent, List<sTransform> list){
			if (t_parent==null){
				return;
			}			
			list.Add(t_parent.Serializable());
			foreach(Transform t_child in t_parent){
				storeTransforms(t_child, list);
			}
		}			
		
		// this builds a Transform list of all transforms attached to the gameobject
		private void storeTransforms(Transform t_parent, List<Transform> list){
			if (t_parent==null){
				return;
			}
			
			list.Add(t_parent);
			foreach(Transform t in t_parent){
				storeTransforms(t,list);
			}
		}
		
		//makes a new GameObject with values stored on this sGameObject
		public GameObject toGameObject() { 
			GameObject go = new GameObject();		
			return toGameObject(go);
		}
		
		//copies sGameObject values to referenced GameObject
		public GameObject toGameObject(GameObject go) { 
			go.SetActive(activeSelf);
			go.name = name;
			go.tag = tag;
			go.layer = layer;

			string goParentName = "";
			if (go.transform.parent !=  null){
				goParentName = go.transform.parent.name;
			}
			if (transform.parentName == null){
				transform.parentName = "";
			}
			// if parents do not match, find correct parent and change it
			if (!goParentName.Equals(transform.parentName) && !transform.parentName.Equals("")){
				GameObject realParent = GameObject.Find(transform.parentName);
				if (realParent != null){
					go.transform.parent = realParent.transform;
				}
			}
			
			//set transformation after we made sure we have the correct parent
			transform.toTransform(go);
			restoreChildTransforms(go.transform);
			return go;
		}
		
		public void restoreChildTransforms(Transform parent){
			List<Transform> goTransforms = new List<Transform>();
			
			Transform t_Parent;
			string t_ParentName, s_ParentName;
			
			// put all transforms into list recursively
			storeTransforms(parent, goTransforms);
			
			foreach (sTransform s in transforms){
				foreach(Transform t in goTransforms){
					//get parents too to make sure we have the same exact same object and not only similarly named
					t_Parent = t.parent;
					if (t_Parent == null){
						t_ParentName = "null";
					} else {
						t_ParentName = t_Parent.name;
					}
					if (s.parentName == null){
						s_ParentName = "null";
					} else {
						s_ParentName = s.parentName;
					}
					
					if (t.name.Equals(s.name) && t_ParentName.Equals(s_ParentName)){
						t.rotation = s.rotation.toQuaternion;					
						t.position = s.position.toVector3;
						if (t.rigidbody != null){						
							t.rigidbody.velocity = Vector3.zero;
							t.rigidbody.angularVelocity = Vector3.zero;
							t.rigidbody.isKinematic = true;
						}
						break;
					}
				}
			}		
		}
	}
}