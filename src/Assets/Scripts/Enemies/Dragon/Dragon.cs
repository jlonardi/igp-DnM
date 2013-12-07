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
		
	private Transform tr;
	private Transform player;
	public Transform head;
	private Player plr;
	private Vector3 landingPoint;

	private float grabTime = 0f;
	public float timeOfLastFireBreath = 0f;

	private Vector3 dir;
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

			if(!grabbing && !breathFire) {
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
					breathFire = false;
					grabbing = true;
					player = GameObject.Find ("Player").transform;
					plr.TakeDamage(plr.GetHealth() - 1, DamageType.HIT);
					plr.makeImmuneToDamage();
					grabTime = Time.time;
				}
			}

			if(grabbing) {

				if(grabTime + 5 > Time.time) {

					Debug.Log ("Head position = " + head.position);

					player.position = head.position + offset;

				} else {
					plr.disableImmunity();
					plr.TakeDamage(9001, DamageType.HIT);
					grabbing = false;
					fighting = false;
				}
			}

			if(timeOfLastFireBreath + 10 < Time.time) {
				breathFire = true;
				timeOfLastFireBreath = Time.time;
			}

			if(breathFire) {
				if(timeOfLastFireBreath + 3 < Time.time) {
					breathFire = false;
				}
			}
		}
	}

	public void flyBackToLair() {
		patroling = false;
		landing = true;
		speed = 35f;

		dir = landingPoint - tr.position;

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
