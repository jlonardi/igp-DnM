using UnityEngine;
using System.Collections;

public class SmoothMouseLookY : MonoBehaviour {
	//use singleton since only we need once instance of this class
	public static SmoothMouseLookY instance;
	public float sensitivity = 5f;
	public float smoothing = 2f;
	public bool invertMouse = false;

	private float position = 0f;	
	private float input;
	private float inputJoy;
	private float deltaSmooth;

	private float minimumY;
	private float maximumY;

	private GameManager game;

	void Awake() {
		SmoothMouseLookY.instance = this;
	}	

	void Start() {
		game = GameManager.instance;
	}

	void Update(){
		// mouse look doesn't work if game not running
		if(game.gameState != GameState.RUNNING || game.player.GetAliveStatus() == false){
			return;
		}

		// get raw mouse data
   	    input = Input.GetAxisRaw("Mouse Y");

		inputJoy = Input.GetAxisRaw("Joystick Look Vertical");
		
		if (Mathf.Abs(inputJoy) > Mathf.Abs(input)){
			input = inputJoy;
		}

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

	public float GetPosition(){
		return position;
	}

	public void SetPosition(float value){
		position = value;
	}

	public float GetSensitivity(){
		return sensitivity;
	}
	
	public void SetSensitivity(float value){
		sensitivity = value;
	}
	
	public float GetSmoothing(){
		return smoothing;
	}
	
	public void SetSmoothing(float value){
		sensitivity = value;
	}

}