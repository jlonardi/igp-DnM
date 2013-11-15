using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {
	//use singleton since only we need one instance of this class
	public static PlayerCamera instance;

	public void Awake()
	{
		PlayerCamera.instance = this;
	}	


	public void Shake(float shakeAmount){
	}

}