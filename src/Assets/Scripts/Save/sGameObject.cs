using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class sGameObject {
	// not yet implemented:
	// renderer 	The Renderer attached to this GameObject (Read Only). (null if there is none attached).
	// rigidbody	The Rigidbody attached to this GameObject (Read Only). (null if there is none attached).
	public bool activeSelf;
	public int layer;
	public string name;
	public string tag;
	public sTransform transform;
	public List<sTransform> recursiveTransforms = new List<sTransform>();
		
	public sGameObject(){
	}
	
	public sGameObject(GameObject go) {
		activeSelf = go.activeSelf;
		layer = go.layer;
		name = go.name;
		tag = go.tag;
		transform = new sTransform(go.transform);
		
		// put all transforms into list recursively
		storeChildTransforms(go.transform, recursiveTransforms);
	}

	public GameObject toGameObject() { 
		GameObject go = new GameObject();		
		return toGameObject(ref go);
	}
	
	public GameObject toGameObject(ref GameObject go) { 
		go.SetActive(activeSelf);
		go.name = name;
		go.tag = tag;
		
		// if parents do not match, find correct parent and change it
		string goParentStr = "";
		string thisParentStr = "";
		if (go.transform.parent !=  null){
			goParentStr = go.transform.parent.name;
		}
		if (transform.parent != null){
			thisParentStr = transform.parent.name;
		}
		if (!goParentStr.Equals(thisParentStr) && !thisParentStr.Equals("")){
			GameObject realParent = GameObject.Find(transform.parent.name);
			if (realParent != null){
				go.transform.parent = realParent.transform;
			}
		}
		
		//set transformation after we made sure we have the correct parent
		transform.toTransform(ref go);
		restoreChildTransforms(go.transform);
		return go;
	}
	
	public void restoreChildTransforms(Transform parent){
		List<Transform> currentRecursiveTransforms = new List<Transform>();
		Transform rParent;
		sTransform bParent;
		string rParentStr, bParentStr;
		
		storeChildTransforms(parent, currentRecursiveTransforms);
		
		foreach (sTransform b in recursiveTransforms){
			foreach(Transform r in currentRecursiveTransforms){
				//get parents too to make sure we have the same exact same object and not only similarly named
				rParent = r.parent;
				bParent = b.parent;
				if (rParent == null){
					rParentStr = "null";
				} else {
					rParentStr = rParent.name;
				}
				if (bParent == null){
					bParentStr = "null";
				} else {
					bParentStr = bParent.name;
				}
				
				if (r.name.Equals(b.name) && rParentStr.Equals(bParentStr)){
					r.rotation = b.rotation.toQuaternion;					
					r.position = b.position.toVector3;
					if (r.rigidbody != null){						
						r.rigidbody.velocity = Vector3.zero;
						r.rigidbody.angularVelocity = Vector3.zero;
						r.rigidbody.isKinematic = true;
					}
					break;
				}
			}
		}		
	}
	public void disableKinematic(Transform parent){
		List<Transform> currentRecursiveTransforms = new List<Transform>();
		
		storeChildTransforms(parent, currentRecursiveTransforms);
		
		foreach (sTransform b in recursiveTransforms){
			foreach(Transform r in currentRecursiveTransforms){
				if (r.name == b.name){
					if (r.rigidbody != null){						
						r.rigidbody.isKinematic = false;
					}
					break;
				}
			}
		}		
	}	
	
	private void storeChildTransforms(Transform parent, List<sTransform> list){
		if (parent==null){
			return;
		}
		
		list.Add(parent.Serializable());
		foreach(Transform t in parent){
			storeChildTransforms(t,list);
		}
	}
	
	private void storeChildTransforms(Transform parent, List<Transform> list){
		if (parent==null){
			return;
		}
		
		list.Add(parent);
		foreach(Transform t in parent){
			storeChildTransforms(t,list);
		}
	}	
}
