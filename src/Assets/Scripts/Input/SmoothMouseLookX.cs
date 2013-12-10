using UnityEngine;
using System.Collections;

public class SmoothMouseLookX : MonoBehaviour {
	//use singleton since only we need once instance of this class
	public static SmoothMouseLookX instance;
	public float sensitivity = 5f;
	public float smoothing = 2f;

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

		inputJoy = Input.GetAxisRaw("Joystick Look Horizontal");

		if (Mathf.Abs(inputJoy) > Mathf.Abs(input)){
			input = inputJoy;
		}

        // scale input against the sensitivity multiply against smoothing value.
        input = input * (sensitivity * smoothing);

        // interpolate movement over time to apply smoothing delta.
        deltaSmooth = Mathf.Lerp(deltaSmooth, input, 1f / smoothing);

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
