using UnityEngine;
using System.Collections;

public class Grenade : MonoBehaviour {
	private Transform thisTransform;
	public float minY = -10.0f;
	public GameObject smoke;
	public GameObject explosionEmitter;
	public float explosionTime;
	public float explosionRadius;
	public float power = 3200f;
	private float timer;
	
	private PlayerCamera playerCam;
	
	public AudioClip[] nearSounds;
	public AudioClip[] farSounds;
	public float farSoundDistance = 25.0f;
	
	private bool exploded;
	private RaycastHit hit;
	public float maxDamage = 300f;
	

	void Start () {
		playerCam = PlayerCamera.instance;
		exploded = false;
		timer = 0.0f;
		thisTransform = transform;
	}
	
	void Update () {
		if(thisTransform.position.y < minY)	{
			Destroy(gameObject);
		}
		
		if(exploded) {
			if(timer > 0.0f) {
				timer -= Time.deltaTime;				
				if(timer <= 0.0f) {
					Destroy(gameObject);
				}	
			}
		}
	}

	void OnCollisionEnter(Collision c) {
		if(exploded) {
			return;
		}		
		Detonate();
	}
	
	void OnCollisionStay(Collision c) {
		if(exploded){
			return;
		}		
		Detonate();
	}
	
	void OnCollisionExit(Collision c) {
		if(exploded){
			return;
		}		
		Detonate();
	}

	public void Detonate () {
		if(exploded){
			return;
		}
		exploded = true;

		if(renderer != null) {
			renderer.enabled = false;
			if(smoke != null) {
				Destroy(smoke);
			}
		} else {
			Renderer[] renderers = GetComponentsInChildren<Renderer>();
			foreach(Renderer r in renderers){
				r.enabled = false;
			}
		}
		
		Vector3 _explosionPosition = thisTransform.position;
		Collider[] col = Physics.OverlapSphere(_explosionPosition, explosionRadius);
		
		float distance = Vector3.Distance(playerCam.transform.position, _explosionPosition);
		playerCam.Shake(distance);
		
		Rigidbody body;
		if(col != null) {
			for(int c = 0; c < col.Length; c++) {
				col[c].gameObject.SendMessage("Destruct", SendMessageOptions.DontRequireReceiver);

				if(col[c].tag == "enemy")	{
					CalculateDamage(col[c], _explosionPosition, distance);
				}
				if(col[c].tag == "Dragon"){
					GameManager.instance.dragon.TakeDamage(100, _explosionPosition, power);
				}

				body = null;
				body = col[c].gameObject.rigidbody;
				if(body != null) {
					body.isKinematic = false;
				} else if(col[c].gameObject.transform.parent != null) {
					body = col[c].gameObject.transform.parent.rigidbody;
					if(body != null) {
						body.isKinematic = false;
					}
				}
				
				if(col[c].collider.tag == "glass") {
					col[c].gameObject.SendMessage("BreakAll", SendMessageOptions.DontRequireReceiver);
				}
			}
		}

		col = Physics.OverlapSphere(_explosionPosition, explosionRadius);
		if(col != null) {
			for(int c = 0; c < col.Length; c++) {
				body = null;
				body = col[c].gameObject.rigidbody;
				if(body != null) {
					body.isKinematic = false;
				} else if(col[c].gameObject.transform.parent != null) {
					body = col[c].gameObject.transform.parent.rigidbody;
					if(body != null) {
						body.isKinematic = false;
					}
				}
				
				if(body != null) {
					body.AddExplosionForce(power, _explosionPosition, explosionRadius, 3.0f);
				}
			}
		}
		gameObject.SendMessage("Explode", SendMessageOptions.DontRequireReceiver);
		PlaySound(distance);
		
		if(explosionEmitter != null) {
			GameObject.Instantiate(explosionEmitter, transform.position, Quaternion.identity);
		}
	}

	// calculates grenades damage by distance
	public void CalculateDamage(Collider col, Vector3 explosionPosition, float distance){
		EnemyLogic enemyObject = col.GetComponentInChildren<EnemyLogic>();
		enemyObject.TakeDamage(100, explosionPosition, power);
	}	

	public void PlaySound(float distance)
	{
		int sIndex;
		
		if (distance < farSoundDistance) {
			sIndex = Random.Range(0, nearSounds.Length);
			audio.PlayOneShot(nearSounds[sIndex]);
			timer = nearSounds[sIndex].length + 1.0f;
		} else {
			sIndex = Random.Range(0, farSounds.Length);
			audio.PlayOneShot(farSounds[sIndex]);
			timer = farSounds[sIndex].length + 1.0f;
		}
	}	


}