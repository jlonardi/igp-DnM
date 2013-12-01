using UnityEngine;
using System.Collections;

public class FireLight : MonoBehaviour {
	//light min & max intensity
	public float minIntensity = 2.8f;
	public float maxIntensity = 3f;

	//time in seconds how often light intensity can flicker
	public float flickerRateMin = 0.2f;
	public float flickerRateMax = 1f;

	//light attached to same gameobject
	private Light attachedLight;

	private float nextUpdateTime;
	private float direction;

	void FixedUpdate () {
		if (attachedLight == null){
			attachedLight = GetComponent<Light>();
		}

		//add or decreace light towards the current direction on every update
		if ((direction > 0 && attachedLight.intensity < maxIntensity) || (direction <= 0 && attachedLight.intensity > minIntensity)){
				attachedLight.intensity += direction * Time.fixedDeltaTime;
		}

		if (nextUpdateTime < Time.time){
			nextUpdateTime = Random.Range(flickerRateMin, flickerRateMax) + Time.time;
			//get new intensity with random between min & max intensity
			float newIntensity = UnityEngine.Random.Range(minIntensity, maxIntensity);
			//get direction intensity will fade after update
			direction = Mathf.Sign(newIntensity - attachedLight.intensity);
			attachedLight.intensity = newIntensity;
		}
	}
}
