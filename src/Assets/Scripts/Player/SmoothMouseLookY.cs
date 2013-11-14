using UnityEngine;
using System.Collections;

public class SmoothMouseLookY : MonoBehaviour {
	//use singleton since only we need once instance of this class
	public static SmoothMouseLookY instance;
    public void Awake()
    {
        SmoothMouseLookY.instance = this;
    }	
	
	public float sensitivity = 5f;
	public float smoothing = 2f;

	public float position = 0f;	
	private float input;
    private float deltaSmooth;

	private float minimumY;
	private float maximumY;
	 	
	void Update(){			
 		// mouse look doesn't work if game not running
		if(GameManager.instance.gameState != GameState.RUNNING){
			return;
		}
		// get raw mouse data
   	    input = Input.GetAxisRaw("Mouse Y");

        // scale input against the sensitivity multiply against smoothing value.
        input *= (sensitivity * smoothing);

        // interpolate movement over time to apply smoothing delta.
        deltaSmooth = Mathf.Lerp(deltaSmooth, input, 1f / smoothing);

        // apply smoothing
        position += deltaSmooth;
		
		//if carrying treasure, restrict mouse look angle
		if(GameManager.instance.treasureState == TreasureState.SET_ON_GROUND){
			minimumY = -80f;
			maximumY = 80f;
		} else {
			minimumY = -20f;
			maximumY = 45f;
		}
			
		// restrict y movement if not between min & max
		position = Mathf.Clamp(position, minimumY, maximumY);			
		transform.localEulerAngles = new Vector3(-position, transform.localEulerAngles.y, 0);
	}
}