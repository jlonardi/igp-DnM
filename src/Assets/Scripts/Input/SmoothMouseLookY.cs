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
	public bool invertMouse = false;

	public float position = 0f;	
	private float input;
    private float deltaSmooth;

	private float minimumY;
	private float maximumY;

	private GameManager game;
	 	
	void Update(){
		if (game == null){
			game = GameManager.instance;
		}
		// mouse look doesn't work if game not running
		if(game.gameState != GameState.RUNNING){
			return;
		}

		// get raw mouse data
   	    input = Input.GetAxisRaw("Mouse Y");

		// if invert selected, invert input
		if (invertMouse){
			input *= -1;
		}

        // scale input against the sensitivity multiply against smoothing value.
        input *= (sensitivity * smoothing);

        // interpolate movement over time to apply smoothing delta.
        deltaSmooth = Mathf.Lerp(deltaSmooth, input, 1f / smoothing);

        // apply smoothing
        position += deltaSmooth;
		
		//if carrying treasure, restrict mouse look angle
		if(game.treasure.OnGround()){
			minimumY = -80f;
			maximumY = 80f;
		} else {
			minimumY = -30f;
			maximumY = 65f;
		}
			
		// restrict y movement if not between min & max
		position = Mathf.Clamp(position, minimumY, maximumY);			
		transform.localEulerAngles = new Vector3(-position, transform.localEulerAngles.y, 0);
	}
}