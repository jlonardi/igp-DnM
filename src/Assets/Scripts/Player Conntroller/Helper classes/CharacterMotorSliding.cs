using UnityEngine;
using System.Collections;

[System.Serializable]
public class CharacterMotorSliding {
	// Does the character slide on too steep surfaces?
	public bool enabled = true;
	
	// How fast does the character slide on steep surfaces?
	public float slidingSpeed = 15f;
	
	// How much can the player control the sliding direction?
	// If the value is 0.5 the player can slide sideways with half the speed of the downwards sliding speed.
	public float sidewaysControl = 1.0f;
	
	// How much can the player influence the sliding speed?
	// If the value is 0.5 the player can speed the sliding up to 150% or slow it down to 50%.
	public float speedControl = 0.4f;
}