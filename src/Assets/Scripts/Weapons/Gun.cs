using UnityEngine;
using System;
using System.Collections;

public class Gun : MonoBehaviour {
	
	public enum GunType {
		SINGLE_FIRE,
		SERIAL_FIRE,
		MINIGUN,
		PROJECTILE
	}

	public enum MiniGunState {
		IDLE,
		WIND_UP,
		FIRING,
		WIND_DOWN,
	}

	public LayerMask hitLayer;

	//How many shots the gun can take in one second
	public float fireRate;
	
	//Range of fire in meters
	public float fireRange = 50.0f;
	
	//Max damage on optimal hit
	public float maxDamage = 50.0f;

	public GunType gunType = GunType.SINGLE_FIRE;
	
	//Speed of the projectile in m/s
	public float projectileSpeed;
	
	//only for minigun!!
	public float startDelay = 0;

	public int clipSize = 30;
	public int totalClips = 20;

	//Time to reload the weapon in seconds
	public float reloadTime;
	
	public int currentRounds;

	public float pushPower = 3.0f;	

	private Camera cam;

	public GameObject bulletMark;
	public GameObject projectilePrefab;
	
	public GunParticles gunParticles;

	public ParticleEmitter[] capsuleEmitter;
	
	public ShotLight shotLight;
	
	public bool unlimited = true;
	public float shootVolume = 0.4f;
	public AudioClip shootSound;
	public AudioClip reloadSound;	
	public AudioClip outOfAmmoSound;
	public AudioClip windUpSound;
	public AudioClip windDownStopSound;

	[HideInInspector]
	public bool freeToShoot;
	
	[HideInInspector]
	public bool reloading;
	
	[HideInInspector]
	public bool fire;

	private float reloadTimer;
	private float lastShootTime;

	//minigun barrelspin speed
	private float spinSpeed = 0f;
	private MiniGunState mgState = MiniGunState.IDLE;

	private float shootDelay;

	private AudioSource audioSource;

	private Transform shootingParticles;
	private float timerToCreateDecal;
	
	private HitParticles hitParticles;
	private GunManager gunManager;
	
	void Start(){		
		gunManager = GunManager.instance;
		cam = Camera.main.camera;
		hitParticles = GunManager.instance.hitParticles;
	}

	void Update (){
		timerToCreateDecal -= Time.deltaTime;
		if (Input.GetButtonDown("Fire1") && currentRounds == 0 && !reloading && freeToShoot){
				PlayOutOfAmmoSound();
		}

		HandleMinigun();

		if(Input.GetButtonUp("Fire1")){
			freeToShoot = true;
		}
		HandleReloading();
		ShootTheTarget();
	}
	
	public void OnEnable()
	{
		if (audioSource == null){
			audioSource = transform.parent.gameObject.audio;
			//make sure we don't have old sounds here
			audioSource.Stop();
			audioSource.loop = false;
		}
		SetRenderer(this.gameObject, true);
		reloadTimer = 0.0f;
		reloading = false;
		freeToShoot = true;
		shootDelay = 1.0f / fireRate;
		
		if(shotLight != null){
			shotLight.enabled = false;
		}
		
		shootingParticles = null;
		if(gunParticles != null){
			for(int i = 0; i < gunParticles.transform.childCount; i++){
				if(gunParticles.transform.GetChild(i).name == "bullet_trace"){
					shootingParticles = gunParticles.transform.GetChild(i);
					break;
				}
			}
		}
	}

	public void OnDisable(){
		SetRenderer(this.gameObject, false);
		StopSounds();
		spinSpeed = 0;
		
		if(gunParticles != null){
			gunParticles.ChangeState(false);
		}
		
		if(capsuleEmitter != null) {
			for(int i = 0; i < capsuleEmitter.Length; i++) {
				if (capsuleEmitter[i] != null)
					capsuleEmitter[i].emit = false;
			}
		}
		
		if(shotLight != null){
			shotLight.enabled = false;
		}
	}
	
	public void HandleMinigun(){
		int maxSpeed = 1000;
		int accelSpeed = 1000;
		int decSpeed = 500;
		if(gunType == GunType.MINIGUN && fire && !reloading && freeToShoot){
			if (mgState == MiniGunState.IDLE){
				PlayWindUpSound();
				mgState = MiniGunState.WIND_UP;
			}
			if (spinSpeed < maxSpeed){
				spinSpeed += accelSpeed * Time.deltaTime;
			} else {
				spinSpeed = maxSpeed;
				mgState = MiniGunState.FIRING;
			}
			
		} else {
			if (mgState != MiniGunState.IDLE){
				mgState = MiniGunState.WIND_DOWN;
				if (spinSpeed > 0){
					spinSpeed -= decSpeed * Time.deltaTime;
				} else {
					spinSpeed = 0;
					mgState = MiniGunState.IDLE;
				}
			}
		}

		// animate minigun barrelspin when minigun selected
		if (gunType == GunType.MINIGUN && enabled){
			this.transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime, Space.Self);
		}
	}

	
	private void SetRenderer(GameObject go, bool enableRenderer){
    	var renderers = go.GetComponentsInChildren<Renderer>();
	    foreach (Renderer r in renderers) {
		    r.enabled = enableRenderer;
    	}					
	}

	
	public void ShootTheTarget(){
		if(fire && !reloading){				
			if(currentRounds > 0){
				if(Time.time > lastShootTime && freeToShoot){
					if (gunType == GunType.MINIGUN && mgState != MiniGunState.FIRING){
						return;
					}

					lastShootTime = Time.time + shootDelay;			
					
					if(capsuleEmitter != null){
						for(int i = 0; i < capsuleEmitter.Length; i++){
							capsuleEmitter[i].Emit();
						}
					}					
					PlayShootSound();
					
					if(gunParticles != null){
						gunParticles.ChangeState(true);
						
					}
					
					if(shotLight != null){
						shotLight.enabled = true;
					}
					
					if (gunType == GunType.PROJECTILE){
						LaunchProjectile();
					} else {
						CheckRaycastHit();
					}

					currentRounds--;
					
					if(currentRounds <= 0){
						Reload();
					}
				}
			}
			else if(freeToShoot){
				if(gunParticles != null){
					gunParticles.ChangeState(false);
				}
				
				if(shotLight != null){
					shotLight.enabled = false;
				}
				
				if(!reloading){
					Reload();
				}
			}
		}
		else{
			//end minigun sounds when not firing
			if (gunType == GunType.MINIGUN && audioSource.loop){
				PlayWindDownSound();
			}

			if(gunParticles != null){
				gunParticles.ChangeState(false);
			}
			
			if(shotLight != null){
				shotLight.enabled = false;
			}
		}
	}
	public void LaunchProjectile()	{
		//Get the launch position (weapon related)
		Ray camRay = cam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.6f, 0f));
		
		Vector3 startPosition;
		
		//startPosition = cam.ScreenToWorldPoint(new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0.5f));
		startPosition = cam.ScreenToWorldPoint(new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 1f));

		GameObject projectile = (GameObject)Instantiate(projectilePrefab, startPosition, Quaternion.identity);
		
		Grenade grenadeObj = projectile.GetComponent("Grenade") as Grenade;

		projectile.transform.rotation = Quaternion.LookRotation(camRay.direction);
		
		Rigidbody projectileRigidbody = projectile.rigidbody;
		
		if(projectile.rigidbody == null){
			projectileRigidbody = (Rigidbody)projectile.AddComponent("Rigidbody");	
		}
		projectileRigidbody.useGravity = true;
		
		RaycastHit hit;
		Ray camRay2 = cam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.55f, 0f));
		
		if(Physics.Raycast(camRay2.origin, camRay2.direction, out hit, fireRange, hitLayer)){
			projectileRigidbody.velocity = (hit.point - this.transform.position).normalized * projectileSpeed;
		}
		else{
			projectileRigidbody.velocity = (cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.55f, 40f)) - this.transform.position).normalized * projectileSpeed;
		}
	}
	
	public void CheckRaycastHit(){
		RaycastHit hit;
		RaycastHit glassHit;
		Ray camRay;
		Vector3 origin;
		Vector3 glassOrigin = new Vector3(0f,0f,0f);
		Vector3 dir;
		Vector3 glassDir = new Vector3(0f,0f,0f);;
		
		camRay = cam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));
		origin = GunManager.instance.shootFrom.transform.position;
		if(Physics.Raycast(camRay.origin + camRay.direction * 0.1f, camRay.direction, out hit, fireRange, hitLayer))	{
			dir = (hit.point - origin).normalized;
			if(hit.collider.tag == "glass")	{
				glassOrigin = hit.point + dir * 0.05f;
				if(Physics.Raycast(glassOrigin, camRay.direction, out glassHit, fireRange - hit.distance, hitLayer)){
					glassDir = glassHit.point - glassOrigin;
				}
			}
		} else{
			dir = this.transform.forward;
		}
		
		if(shootingParticles != null){
			shootingParticles.rotation = Quaternion.FromToRotation(Vector3.forward, (cam.ScreenToWorldPoint(
				new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, cam.farClipPlane)) - this.transform.position).normalized);
		}
		
		if(Physics.Raycast(origin, dir, out hit, fireRange, hitLayer)){
			hit.collider.gameObject.SendMessage("Hit", hit, SendMessageOptions.DontRequireReceiver);
			if(hit.collider.tag == "enemy")	{
				CalculateDamage(hit);
			}
			GenerateGraphicStuff(hit);
			if(hit.collider.tag == "glass")	{
				if(Physics.Raycast(glassOrigin, glassDir, out glassHit, fireRange - hit.distance, hitLayer)){
					glassHit.collider.gameObject.SendMessage("Hit", glassHit, SendMessageOptions.DontRequireReceiver);
					GenerateGraphicStuff(glassHit);
				}
			}
		}
	}

	public void GenerateGraphicStuff(RaycastHit hit){		
		HitType hitType = HitType.CONCRETE;

		Rigidbody body = hit.collider.rigidbody;
		if(body == null){
			if(hit.collider.transform.parent != null){
				body = hit.collider.transform.parent.rigidbody;
			}
		}
		
		if(body != null){
			if(body.gameObject.layer != 10 && !body.gameObject.name.ToLower().Contains("door")){
				body.isKinematic = false;
			}
		
			if(!body.isKinematic){
   				Vector3 direction = hit.collider.transform.position - this.transform.position;
				body.AddForceAtPosition(direction.normalized * pushPower, hit.point, ForceMode.Impulse);
			}
		}
		
		GameObject go;
		
		float delta = -0.02f;
		Vector3 hitUpDir = hit.normal;
		Vector3 hitPoint = hit.point + hit.normal * delta;
				
		switch(hit.collider.tag){
			case "enemy":
				hitType = HitType.ENEMY;
				go = GameObject.Instantiate(hitParticles.bloodParticle, hitPoint, Quaternion.FromToRotation(Vector3.up, hitUpDir)) as GameObject;
				break;
			case "wood":
				hitType = HitType.WOOD;
				go = GameObject.Instantiate(hitParticles.woodParticle, hitPoint, Quaternion.FromToRotation(Vector3.up, hitUpDir)) as GameObject;
				break;
			case "metal":
				hitType = HitType.METAL;
				go = GameObject.Instantiate(hitParticles.metalParticle, hitPoint, Quaternion.FromToRotation(Vector3.up, hitUpDir)) as GameObject;
				break;
			case "rock":
			case "concrete":
				hitType = HitType.CONCRETE;
				go = GameObject.Instantiate(hitParticles.concreteParticle, hitPoint, Quaternion.FromToRotation(Vector3.up, hitUpDir)) as GameObject;
				break;
			case "sand":
			case "dirt":
				hitType = HitType.CONCRETE;
				go = GameObject.Instantiate(hitParticles.sandParticle, hitPoint, Quaternion.FromToRotation(Vector3.up, hitUpDir)) as GameObject;
				break;
			case "water":
				go = GameObject.Instantiate(hitParticles.waterParticle, hitPoint, Quaternion.FromToRotation(Vector3.up, hitUpDir)) as GameObject;
				break;
			default:				
				return;
		}
		
		go.layer = hit.collider.gameObject.layer;
		
		if(hit.collider.renderer == null){
			return;
		}
		
		if(timerToCreateDecal < 0.0f && hit.collider.tag != "water"){
			go = (GameObject)Instantiate(bulletMark, hit.point, Quaternion.FromToRotation(Vector3.forward, -hit.normal));
			BulletMarks bm = (BulletMarks)go.GetComponent("BulletMarks");
			bm.GenerateDecal(hitType, hit.collider.gameObject);		
			timerToCreateDecal = 0.02f;
		}
	}
	
	public void HandleReloading(){
		if(Input.GetKeyDown(KeyCode.R) && !reloading){
			Reload();
		}
		
		if(reloading){
			reloadTimer -= Time.deltaTime;
			
			if(reloadTimer <= 0.0f){
				reloading = false;
				if(!unlimited){
					totalClips--;
				}
				currentRounds = clipSize;
			}
		}
	}
	
	public void Reload(){
		if(totalClips > 0 && currentRounds < clipSize){
			PlayReloadSound();
			reloading = true;
			reloadTimer = reloadTime;
		}
	}
	
	// calculates gun's damage for hitpoint
	public void CalculateDamage(RaycastHit hit){		
		EnemyLogic enemyObject = hit.collider.GetComponentInChildren<EnemyLogic>();
		
		//always give 1/2 of max damage and rest of the damage amount is calculated by the distance
		float damageAmount = maxDamage/2 + maxDamage/2 * (fireRange - hit.distance) / fireRange;	
		Vector3 direction = hit.collider.transform.position - this.transform.position;	
		enemyObject.TakeDamage((int)damageAmount, hit, direction, pushPower);
		Debug.Log("Gun's range: "+fireRange + ", Distance: " +hit.distance+ ", Gun's damage: " + damageAmount);
	}
		
	//---------------AUDIO METHODS--------
	// These require Audio Source to be available on Gun Manager object

	//called when gun is disabled
	public void StopSounds(){
		if (gunType == GunType.MINIGUN && mgState != MiniGunState.IDLE){
			PlayWindDownSound();
		}
	}

	public void PlayWindUpSound() {
		audioSource.loop = false;
		audioSource.Stop();
		audioSource.PlayOneShot(windUpSound);
	}
	
	public void PlayWindDownSound() {
		audioSource.loop = false;
		audioSource.Stop();
		audioSource.PlayOneShot(windDownStopSound);
	}

	public void PlayOutOfAmmoSound() {
		audioSource.loop = false;
		audioSource.Stop();
		audioSource.PlayOneShot(outOfAmmoSound, 1.5f);
	}
	
	public void PlayReloadSound() {
		audioSource.loop = false;
		audioSource.Stop();
		audioSource.PlayOneShot(reloadSound, 1.5f);
	}
	
	public void PlayShootSound() {
		if (gunType == GunType.MINIGUN){
			if (audioSource.loop == false){
				audioSource.Stop();
				audioSource.clip = shootSound;
				audioSource.loop = true;
				audioSource.Play();
			}
		} else {
			audioSource.PlayOneShot(shootSound);
		}
	}	
}
