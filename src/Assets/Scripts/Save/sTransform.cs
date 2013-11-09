using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class sTransform {
	// not yet implemented:	
	// renderer 			The Renderer attached to this GameObject (null if there is none attached).
	// localToWorldMatrix 	Matrix that transforms a point from local space into world space (Read Only).
	// worldToLocalMatrix 	Matrix that transforms a point from world space into local space (Read Only).
	public sVector3 eulerAngles;
	public sVector3 forward;
	public bool hasChanged;
	public sVector3 localEulerAngles;
	public sVector3 localPosition;
	public sQuaternion localRotation;
	public sVector3 localScale;
	public string name;
	public sVector3 position;
	public sVector3 right;
	public sQuaternion rotation;
	public string tag;
	public sVector3 up;	
	
	readonly int childCount;
	public sGameObject gameObject;
	public sVector3 lossyScale;
	public sTransform parent;
	public sTransform root;
	public sTransform transform;
//	public List<sTransform> childTransforms;

	public sTransform(Transform t){
		eulerAngles = t.eulerAngles.Serializable();
		forward = t.forward.Serializable();
		hasChanged = t.hasChanged;
		localEulerAngles = t.localEulerAngles.Serializable();
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
		//gameObject = t.gameObject.Serializable();
		if (t.lossyScale != null){
			lossyScale = t.lossyScale.Serializable();
		}
		if (t.parent != null){
			parent = t.parent.Serializable();
		}
		//root = t.root.Serializable();	
		//transform = t.transform.Serializable();
	}
	
	public void toTransform(ref GameObject go) { 
		go.transform.eulerAngles = eulerAngles.toVector3;
		go.transform.forward = forward.toVector3;
		go.transform.hasChanged = hasChanged;
		go.transform.localEulerAngles = localEulerAngles.toVector3;
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