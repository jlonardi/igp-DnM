using UnityEngine;
using System.Collections;

public class ShotLight : MonoBehaviour {
	public float time = 0.02f;
	private float timer;
	
	public void OnEnable()
	{
		if(light == null)
		{
			Destroy(this);
		}
		else
		{
			timer = time;
			light.enabled = true;
		}
	}
	
	public void OnDisable()
	{
		if(light == null)
		{
			Destroy(this);
		}
		else
		{
			timer = time;
			light.enabled = false;
		}
	}
	
	public void LateUpdate()
	{
		timer -= Time.deltaTime;
		
		if(timer <= 0.0)
		{
			timer = time;
			light.enabled = !light.enabled;
		}
	}
	
}
