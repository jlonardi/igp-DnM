using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ragdoll : MonoBehaviour {
	
	public AudioClip[] deathSounds;
	private List<Transform> poseBones = new List<Transform>();
	private List<Transform> ragdollBones = new List<Transform>();

	void Start () {
		// play randdom deathsound when ragdoll has been made
		int clipNum = Random.Range(0, deathSounds.Length - 1);
		audio.clip = deathSounds[clipNum];
		audio.Play();
	}	

	public void CopyPose(Transform pose){
		AddChildren(pose, poseBones);
		AddChildren(this.transform, ragdollBones);
		
		foreach (Transform b in poseBones){
			foreach(Transform r in ragdollBones){
				if (r.name == b.name){
					r.rotation = b.rotation;
					r.position = b.position;
					//r.eulerAngles = b.eulerAngles;
					break;
				}
			}
		}				
		
	}
	
	private void AddChildren(Transform parent, List<Transform> list){
		list.Add(parent);
		foreach(Transform t in parent){
			AddChildren(t,list);
		}
	}
}
