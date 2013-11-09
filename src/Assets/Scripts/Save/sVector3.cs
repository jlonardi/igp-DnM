using UnityEngine;
using System;

[Serializable]
public class sVector3 {
	public float x,y,z;

	public sVector3(Vector3 v3){
		this.x = v3.x;
		this.y = v3.y;
		this.z = v3.z;
	}
	
	public sVector3(float x, float y, float z){
		this.x = x;
		this.y = y;
		this.z = z;
	}
	
	public Vector3 toVector3 { 
		get {
			return new Vector3(x, y, z);
		} 
	}
}