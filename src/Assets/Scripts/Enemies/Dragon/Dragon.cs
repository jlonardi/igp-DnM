using UnityEngine;
using System.Collections;

public class Dragon : MonoBehaviour {

	public bool breathFire = false;
	public bool flying = true;
	public bool patroling = true;
	public bool walking = false;
	public bool grabbing = false;
	public bool landing = false;
	public bool fighting = false;
	public bool playerKilled = false;
	public float speed = 15f;
	public float turningSpeed = 5;
	public float minDistanceFromPlayer = 7;
		
	public Transform tr;
	public Transform player;
	public Transform head;
	public Player plr;
	public Vector3 landingPoint;

	public float grabTime = 0f;
	public float releaseTime = float.MaxValue;

	public Vector3 dir;
	public Vector3 offset = new Vector3(0,-1.5f,-0.2f);
	
	void Start () {

		tr = transform;
		landingPoint = GameObject.Find("PointInGround").transform.position;
		player = GameObject.Find("playerFocus").transform;
		head = GameObject.Find("IK_Target_Kopf").transform;
		plr = GameObject.Find("Player").GetComponent<Player>();
	}

	void Update () {

		if(patroling) {
			tr.Translate(0f,0f,Time.deltaTime*speed); // move forward
			tr.Rotate(0f,Time.deltaTime*speed,0f); // turn a little
		}

		if(landing) {
			moveTowards(landingPoint);
			if(tr.position == landingPoint) {
				landing = false;
				flying = false;
				fighting = true;
				speed = 15f;
				dir = (player.position - tr.position);
			}
		}

		if(fighting) {

			if(!grabbing) {
				dir = (player.position - tr.position);
				rotateTowards(dir);
			}

			if(walking) {
				if((tr.position - player.position).magnitude > minDistanceFromPlayer) {

					// THIS HAS TO BE FIXED BY CHANGING THE ORIGO OF THE DRAGON
					Vector3 fixedPlayerPosition = player.position;
					fixedPlayerPosition.y -= 2.5f;
					moveTowards(fixedPlayerPosition);

				} else {
					walking = false;
					grabbing = true;
					player = GameObject.Find ("Player").transform;
					plr.TakeDamage(99, DamageType.HIT);
					grabTime = Time.time;
				}
			}

			if(grabbing) {

				if(grabTime + 5 > Time.time) {
					player.position = head.position + offset;
				} else {
					plr.TakeDamage(9001, DamageType.HIT);
				}
			}
		}
	}

	public void flyBackToLair() {
		patroling = false;
		landing = true;
		speed = 35f;

		dir = (landingPoint - tr.position);

	}

	public void killPlayer() {
		if(fighting) {
			Debug.Log ("Kill player");
			walking = true;
		}
	}

	void rotateTowards (Vector3 dir) {
		Quaternion rot = tr.rotation;
		Quaternion toTarget = Quaternion.LookRotation (dir);
		
		rot = Quaternion.Slerp (rot,toTarget,turningSpeed*Time.deltaTime);
		Vector3 euler = rot.eulerAngles;
		euler.z = 0;
		euler.x = 0;
		rot = Quaternion.Euler (euler);
		
		tr.rotation = rot;
	}

	void moveTowards(Vector3 target) {
		rotateTowards(dir);
		float step = speed * Time.deltaTime;
		tr.position = Vector3.MoveTowards(tr.position, target, step);
	}
}
