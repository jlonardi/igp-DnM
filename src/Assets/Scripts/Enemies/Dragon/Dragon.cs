using UnityEngine;
using System.Collections;

public class Dragon : MonoBehaviour {

	public bool breathFire = false;
	public bool flying = true;
	public bool grabbing = false;
	public float speed = 15f;
	public float turningSpeed = 5;
	public float minDistanceFromPlayer = 7;
	private float health = 10000;
	private float maxHealth = 10000;

	private bool patroling = true;
	private bool walking = false;
	private bool landing = false;
	private bool fighting = false;
	private Transform tr;
	private Transform player;
	public Transform head;
	private Player plr;
	private Vector3 landingPoint;

	private float grabTime = 0f;
	public float timeOfLastFireBreath = 0f;

	private Vector3 dir;
	public Vector3 offset = new Vector3(0,-1.5f,-0.3f);
	public AudioClip[] painSounds;
	private GameManager game;
	private RagdollManager ragdolls;

	void Start () {
		game = GameManager.instance;
		ragdolls = RagdollManager.instance;
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
					plr.SetDamageImmunity(true);
					grabTime = Time.time;
				}
			}

			if(grabbing) {

				if(grabTime + 5 > Time.time) {

					Debug.Log ("Head position = " + head.position);

					player.position = head.position + offset;

				} else {
					plr.SetDamageImmunity(false);
					plr.TakeDamage(9001, DamageType.HIT);
					grabbing = false;
					fighting = false;
				}
			}

			if(timeOfLastFireBreath + 5 < Time.time) {
				breathFire = true;
				timeOfLastFireBreath = Time.time;

				Quaternion rot = tr.rotation;
				Quaternion toTarget = Quaternion.LookRotation (dir);

				Vector3 euler = toTarget.eulerAngles;
				euler.z = 0;
				euler.x = 0;
				rot = Quaternion.Euler (euler);
				
				tr.rotation = rot;
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

	public bool GetFighting(){
		return fighting;
	}

	public float GetHealth(){
		return health;
	}

	public void SetHealth(float value){
		health = value;
	}

	public float GetMaxHealth(){
		return maxHealth;
	}

	// TakeDamage without aplying force
	public void TakeDamage(int damageAmount){
		TakeDamage(damageAmount, new RaycastHit(), new Vector3(), 0f);
	}
	
	// TakeDamage and apply explosive force
	public void TakeDamage(int damageAmount, Vector3 explosionPosition, float power){
		health -= damageAmount;
		if(health <= 0) {
			Rigidbody deadBody = MakeDead();
			deadBody.AddExplosionForce(power, explosionPosition, 10f, 2.0f);
		}		
		AfterTakeDamage();
	}
	
	// TakeDamage and apply damage force
	public void TakeDamage(int damageAmount, RaycastHit hit, Vector3 direction, float power){
		health -= damageAmount;
		if(health <= 0) {
			Rigidbody deadBody = MakeDead();
			deadBody.AddForceAtPosition(direction.normalized * power * 11, hit.point, ForceMode.Impulse);
			EnemyManager.instance.disableDragonSpawns();
			game.statistics.AddKillStats(EnemyType.DRAGON);
		}
		AfterTakeDamage();
	}

	// run this after taking a shot or explosive damage
	public void AfterTakeDamage(){
		PlaySound(painSounds);
		Debug.Log("Dragon health left: " + health);
	}

	// turn living into dead
	public Rigidbody MakeDead(){
		game.statistics.AddKillStats(EnemyType.DRAGON);

		// actually make enemy a ragdoll
		GameObject ragdoll = ragdolls.MakeRagdoll(EnemyType.DRAGON, this.gameObject, true);
		return ragdoll.GetComponentInChildren(typeof(Rigidbody)) as Rigidbody;		
	}

	private void PlaySound(AudioClip clip)
	{
		if (!audio.isPlaying)
		{
			audio.clip = clip;
			audio.Play();
		}
	}
	
	private void PlaySound(AudioClip[] clips)
	{
		int clipNum = Random.Range(0, clips.Length - 1);
		PlaySound(clips[clipNum]);
	}


}
