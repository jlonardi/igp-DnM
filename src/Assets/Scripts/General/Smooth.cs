using UnityEngine;
using System.Collections;

public class Smooth {

	// Increase towards target
	public float IncrementTowards(float current, float target, float acceleration){
		if (current == target) {
			return current;
		}
		else {
			float dir = Mathf.Sign(target - current); // determine increase or decrease direction
			current += acceleration * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target - current))? current: target; // if current passed target then return target, otherwise return current
		}
	}
}
