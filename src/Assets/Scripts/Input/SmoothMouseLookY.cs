using UnityEngine;
using System.Collections;

public class SmoothMouseLookY : MonoBehaviour {
	//use singleton since only we need once instance of this class
	public static SmoothMouseLookY instance;

	private float mouseSensitivity = 4f;
	private float mouseSmoothing = 1f;
	
	private float joySensitivity = 3f;
	private float joySmoothing = 2f;

	public bool invertMouse = false;
	public bool invertJoy = false;

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
		// if invert selected, invert input
		if (invertMouse){
			input *= -1;
		}

		inputJoy = Input.GetAxis("Joystick Look Vertical");

		// if invert selected, invert input
		if (invertJoy){
			inputJoy *= -1;
		}
		

		if (Mathf.Abs(inputJoy) > Mathf.Abs(input)){
			input = inputJoy;

			// scale input against the sensitivity multiply against smoothing value.
			input = input * (joySensitivity * joySmoothing);
			
			// interpolate movement over time to apply smoothing delta.
			deltaSmooth = Mathf.Lerp(deltaSmooth, input, 1f / joySmoothing);
			
		} else {
			// scale input against the sensitivity multiply against smoothing value.
			input = input * (mouseSensitivity * mouseSmoothing);
			
			// interpolate movement over time to apply smoothing delta.
			deltaSmooth = Mathf.Lerp(deltaSmooth, input, 1f / mouseSmoothing);
		}

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

	public float GetMouseSensitivity(){
		return mouseSensitivity;
	}
	
	public void SetMouseSensitivity(float value){
		mouseSensitivity = value;
	}
	
	public float GetJoySensitivity(){
		return joySensitivity;
	}
	
	public void SetJoySensitivity(float value){
		joySensitivity = value;
	}
	
	public float GetMouseSmoothing(){
		return mouseSmoothing;
	}
	
	public void SetMouseSmoothing(float value){
		mouseSmoothing = value;
	}
	
	public float GetJoySmoothing(){
		return joySmoothing;
	}
	
	public void SetJoySmoothing(float value){
		joySmoothing = value;
	}
}