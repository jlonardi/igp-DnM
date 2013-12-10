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
	private Transform playerTransform;
	private Transform cameraTransform;
	private Transform headTransform;
	private Transform tr;
	private Vector3 landingPoint;

	private float grabTime = 0f;
	public float timeOfLastFireBreath = 0f;

	private Vector3 dir;
	public Vector3 offset = new Vector3(0,-1.5f,-0.3f);
	public AudioClip[] painSounds;
	public AudioClip fireBreathSound;
	public AudioClip[] wingSounds;
	private GameManager game;
	private RagdollManager ragdolls;

	void Start () {
		game = GameManager.instance;
		ragdolls = RagdollManager.instance;
		tr = transform;
		landingPoint = GameObject.Find("PointInGround").transform.position;
		playerTransform = GameObject.Find("playerFocus").transform;
		cameraTransform = GameObject.Find("Main Camera").transform;
		headTransform = GameObject.Find("IK_Target_Kopf").transform;
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
				dir = (playerTransform.position - tr.position);
			}
		}

		if(fighting) {

			if(!grabbing && !breathFire) {
				dir = (playerTransform.position - tr.position);
				rotateTowards(dir);
			}

			if(walking) {
				if((tr.position - playerTransform.position).magnitude > minDistanceFromPlayer) {

					// THIS HAS TO BE FIXED BY CHANGING THE ORIGO OF THE DRAGON
					Vector3 fixedPlayerPosition = playerTransform.position;
					fixedPlayerPosition.y -= 2.5f;
					moveTowards(fixedPlayerPosition);

				} else {
					walking = false;
					breathFire = false;
					grabbing = true;
					playerTransform = game.player.gameObject.transform;
					game.player.SetDeathDuration(4);
					game.player.TakeDamage(100, DamageType.HIT);
					grabTime = Time.time;
				}
			}

			if(grabbing) {

				if(grabTime + 2.5f > Time.time) {
					playerTransform.position = headTransform.position + offset;
				} else {
					fighting = false;
					grabbing = false;

					Debug.Log("Player dropped");
					//move player so that it doesn't drop through terrain
					playerTransform.position += Vector3.up*10;
					RaycastHit hit = new RaycastHit();		
					if (Physics.Raycast(playerTransform.position, -Vector3.up, out hit)){
						// align just above terrain		    
						playerTransform.position = new Vector3(playerTransform.position.x, 
	                                       playerTransform.position.y - hit.distance + 0.001f, playerTransform.position.z);
					}
					//move camera so it's closer to ground and rotate it
					cameraTransform.eulerAngles = new Vector3(20,0,90);
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

				audio.PlayOneShot(fireBreathSound, 0.6f);
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
		audio.PlayOneShot(painSounds[0]);
	}

	public void killPlayer() {
		if(fighting) {
			//Debug.Log ("Kill player");
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

	// TakeDamage without aplying force
	public void TakeDamage(int damageAmount){
		TakeDamage(damageAmount, new RaycastHit(), new Vector3(), 0f);
	}
	
	// TakeDamage and apply explosive force
	public void TakeDamage(int damageAmount, Vector3 explosionPosition, float power){
		health -= damageAmount;
		if(health <= 0) {
			afterDeath();
			Rigidbody deadBody = MakeDead();
			deadBody.AddExplosionForce(power, explosionPosition, 10f, 2.0f);
		}		
		AfterTakeDamage();
	}
	
	// TakeDamage and apply damage force
	public void TakeDamage(int damageAmount, RaycastHit hit, Vector3 direction, float power){
		health -= damageAmount;
		if(health <= 0) {
			afterDeath();
			Rigidbody deadBody = MakeDead();
			deadBody.AddForceAtPosition(direction.normalized * power * 11, hit.point, ForceMode.Impulse);
		}
		AfterTakeDamage();
	}

	// run this after taking a shot or explosive damage
	public void AfterTakeDamage(){
		PlaySound(painSounds);
		//Debug.Log("Dragon health left: " + health);
	}

	public void afterDeath() {
		EnemyManager.instance.disableDragonSpawns();
		game.statistics.AddKillStats(EnemyType.DRAGON);
	}

	// turn living into dead
	public Rigidbody MakeDead(){

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
		int clipNum = Random.Range(0, clips.Length);
		PlaySound(clips[clipNum]);
	}

	//gets triggered every time dragon flaps wings
	private void WingTrigger(){
		int clipNum = Random.Range(0, wingSounds.Length);
		audio.PlayOneShot(wingSounds[clipNum], 0.7f);
	}

	
	public bool GetFighting(){
		return fighting;
	}
	
	public void SetFighting(bool value){
		fighting = value;
	}
	
	public float GetHealth(){
		return health;
	}
	
	public void SetHealth(float value){
		health = value;
	}

	public void SetMaxHealth(float value){
		maxHealth = value;
		if (health>maxHealth){
			health = maxHealth;
		}
	}

	public float GetMaxHealth(){
		return maxHealth;
	}
	
	public bool GetPatroling(){
		return patroling;
	}
	
	public void SetPatroling (bool value){
		patroling = value;
	}
	
	public bool GetWalking(){
		return walking;
	}
	
	public void SetWalking(bool value){
		walking = value;
	}
	
	public bool GetLanding(){
		return landing;
	}
	
	public void SetLanding(bool value){
		landing = value;
	}
}
