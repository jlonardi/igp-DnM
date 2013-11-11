using UnityEngine;
using System.Collections;

public class SmoothMouseLookX : MonoBehaviour {
	//use singleton since only we need once instance of this class
	public static SmoothMouseLookX instance;
    public void Awake()
    {
        SmoothMouseLookX.instance = this;
    }	
	
	public float sensitivity = 5f;
	public float smoothing = 2f;

	public float position = 0f;	
	private float input;
    private float deltaSmooth;
	
	void Start(){
		//preserve initial player rotation when level starts
		position = transform.eulerAngles.y;	
	}
	
	void Update(){			
 		// mouse look doesn't work if game not running
		if(GameManager.instance.gameState != GameState.RUNNING){
			return;
		}		
		// get raw mouse data
   	    input = Input.GetAxisRaw("Mouse X");

        // scale input against the sensitivity multiply against smoothing value.
        input = input * (sensitivity * smoothing);

        // interpolate movement over time to apply smoothing delta.
        deltaSmooth = Mathf.Lerp(deltaSmooth, input, 1f / smoothing);

        // apply smoothing
        position += deltaSmooth;
		
		// rotate transform with x
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, position, 0);
	}
}
