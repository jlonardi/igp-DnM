using UnityEngine;
using System.Collections;

public class SmoothMouseLookX : MonoBehaviour {
	//use singleton since only we need once instance of this class
	public static SmoothMouseLookX instance;
	private float mouseSensitivity = 4f;
	private float mouseSmoothing = 1f;

	private float joySensitivity = 0.4f;
	private float joySmoothing = 5f;

	private float position = 0f;	
	private float input;
	private float inputJoy;
	private float deltaSmooth;
	private GameManager game;

	void Awake() {
		SmoothMouseLookX.instance = this;
	}	
	

	void Start(){
		game = GameManager.instance;
		//preserve initial player rotation when level starts
		position = transform.eulerAngles.y;	
	}
	
	void Update(){			
		// mouse look doesn't work if game not running
		if(game.gameState != GameState.RUNNING || game.player.GetAliveStatus() == false){
			return;
		}
		// get raw mouse data
		input = Input.GetAxisRaw("Mouse X");
		inputJoy = Input.GetAxis("Joystick Look Horizontal");

		float absInputJoy = Mathf.Abs(inputJoy);
		if (absInputJoy > Mathf.Abs(input)){
			
			float logScale = Mathf.Log(absInputJoy*10);
			input = inputJoy*logScale;

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

		// rotate transform with x
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, position, 0);
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
