using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Ragdoll : MonoBehaviour {
	
	private List<Transform> poseBones = new List<Transform>();
	private List<Transform> ragdollBones = new List<Transform>();
	
	public void CopyPose(Transform pose){
		AddChildren(pose, poseBones);
		AddChildren(this.transform, ragdollBones);
		
		foreach (Transform b in poseBones){
			foreach(Transform r in ragdollBones){
				if (r.name == b.name){
					r.eulerAngles = b.eulerAngles;
					break;
				}
			}
		}				
		
	}
	
	public void AddChildren(Transform parent, List<Transform> list){
		list.Add(parent);
		foreach(Transform t in parent){
			AddChildren(t,list);
		}
	}
}
