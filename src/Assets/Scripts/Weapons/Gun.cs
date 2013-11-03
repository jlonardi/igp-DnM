using UnityEngine;
using System.Collections;

enum FireType {
	RAYCAST,
	PHYSIC_PROJECTILE,
}

public class Gun : MonoBehaviour {
	
	public string gunName;
	
	public Transform weaponTransformReference;
	public LayerMask hitLayer;

	//How many shots the gun can take in one second
	public float fireRate;
	
	//Range of fire in meters
	public float fireRange = 50.0f;
	
	//Max damage on optimal hit
	public float maxDamage = 50.0f;
	
	//Speed of the projectile in m/s
	public float projectileSpeed;
	
	public int clipSize;
	public int totalClips;

	public bool useGravity;
	
	public bool isExplosive;
	//Time to reload the weapon in seconds
	public float reloadTime;
	
	public bool autoReload;
	public int currentRounds;

	public float pushPower = 3.0f;	

//	public SoldierCamera soldierCamera;
	private Camera cam;

	public GameObject bulletMark;
	public GameObject projectilePrefab;
	
	public GunParticles shootingEmitter;

	public ParticleEmitter[] capsuleEmitter;
	
	public ShotLight shotLight;
	
	public bool unlimited = true;
	public float shootVolume = 0.4f;
	public AudioClip shootSound;
	public AudioClip reloadSound;	
	public AudioClip outOfAmmoSound;
	
	[HideInInspector]
	public bool freeToShoot;
	
	[HideInInspector]
	public bool reloading;
	
	[HideInInspector]
	public bool fire;

	private FireType fireType;	
	private float reloadTimer;
	private float lastShootTime;
	private float shootDelay;
	
	private AudioSource shootSoundSource;
	private AudioSource reloadSoundSource;
	private AudioSource outOfAmmoSoundSource;
	
	private Transform shootingParticles;
	private float timerToCreateDecal;
	
	private HitParticles hitParticles;
	
	void Start(){		
		cam = Camera.main.camera;
		GunManager gm = GameObject.Find("Gun Manager").GetComponent<GunManager>();
		hitParticles = gm.hitParticles;
	}
	
	public void OnDisable(){
		if(shootingEmitter != null){
			shootingEmitter.ChangeState(false);
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
	
	public void OnEnable()
	{
//		cam = soldierCamera.camera;
		
		reloadTimer = 0.0f;
		reloading = false;
		freeToShoot = true;
		shootDelay = 1.0f / fireRate;
		
		totalClips--;
		currentRounds = clipSize;
		
		if(projectilePrefab != null){
			fireType = FireType.PHYSIC_PROJECTILE;
		}
		
		if(shotLight != null){
			shotLight.enabled = false;
		}
		
		shootingParticles = null;
		if(shootingEmitter != null){
			for(int i = 0; i < shootingEmitter.transform.childCount; i++){
				if(shootingEmitter.transform.GetChild(i).name == "bullet_trace"){
					shootingParticles = shootingEmitter.transform.GetChild(i);
					break;
				}
			}
		}
	}
	
	public void ShotTheTarget(){
		if(fire && !reloading){
			if(currentRounds > 0){
				if(Time.time > lastShootTime && freeToShoot){
					lastShootTime = Time.time + shootDelay;			
					
					if(capsuleEmitter != null){
						for(int i = 0; i < capsuleEmitter.Length; i++){
							capsuleEmitter[i].Emit();
						}
					}					
					PlayShootSound();
					
					if(shootingEmitter != null){
						shootingEmitter.ChangeState(true);
						
					}
					
					if(shotLight != null){
						shotLight.enabled = true;
					}
					
					switch(fireType){
						case FireType.RAYCAST:
//							TrainingStatistics.shotsFired++;
							CheckRaycastHit();
							break;
						case FireType.PHYSIC_PROJECTILE:
//							TrainingStatistics.grenadeFired++;
							LaunchProjectile();
							break;
					}					
					currentRounds--;
					
					if(currentRounds <= 0){
						Reload();
					}
				}
			}
			else if(autoReload && freeToShoot){
				if(shootingEmitter != null){
					shootingEmitter.ChangeState(false);
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
			if(shootingEmitter != null){
				shootingEmitter.ChangeState(false);
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
		
		if(weaponTransformReference != null){
			startPosition = weaponTransformReference.position;
		}
		else{
			startPosition = cam.ScreenToWorldPoint(new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0.5f));
		}
		
		GameObject projectile = (GameObject)Instantiate(projectilePrefab, startPosition, Quaternion.identity);
		
		Grenade grenadeObj = projectile.GetComponent("Grenade") as Grenade;
//		grenadeObj.soldierCamera = soldierCamera;
		
		projectile.transform.rotation = Quaternion.LookRotation(camRay.direction);
		
		Rigidbody projectileRigidbody = projectile.rigidbody;
		
		if(projectile.rigidbody == null){
			projectileRigidbody = (Rigidbody)projectile.AddComponent("Rigidbody");	
		}
		projectileRigidbody.useGravity = useGravity;
		
		RaycastHit hit;
		Ray camRay2 = cam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.55f, 0f));
		
		if(Physics.Raycast(camRay2.origin, camRay2.direction, out hit, fireRange, hitLayer)){
			projectileRigidbody.velocity = (hit.point - weaponTransformReference.position).normalized * projectileSpeed;
		}
		else{
			projectileRigidbody.velocity = (cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.55f, 40f)) - weaponTransformReference.position).normalized * projectileSpeed;
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
		
		if(weaponTransformReference == null){
			camRay = cam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));
			origin = camRay.origin;
			dir = camRay.direction;
			origin += dir * 0.1f;
		}
		else{
			camRay = cam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));			  
			origin = weaponTransformReference.position + (weaponTransformReference.right * 0.2f);
			if(Physics.Raycast(camRay.origin + camRay.direction * 0.1f, camRay.direction, out hit, fireRange, hitLayer))	{
				dir = (hit.point - origin).normalized;
				if(hit.collider.tag == "glass")	{
					glassOrigin = hit.point + dir * 0.05f;
					if(Physics.Raycast(glassOrigin, camRay.direction, out glassHit, fireRange - hit.distance, hitLayer)){
						glassDir = glassHit.point - glassOrigin;
					}
				}
			}
			else{
				dir = weaponTransformReference.forward;
			}
		}
		
		if(shootingParticles != null){
			shootingParticles.rotation = Quaternion.FromToRotation(Vector3.forward, (cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, cam.farClipPlane)) - weaponTransformReference.position).normalized);
		}
		
		if(Physics.Raycast(origin, dir, out hit, fireRange, hitLayer)){
			hit.collider.gameObject.SendMessage("Hit", hit, SendMessageOptions.DontRequireReceiver);
			GenerateGraphicStuff(hit);
			if(hit.collider.tag == "enemy")	{
				CalculateDamage(hit);
			}
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
   				Vector3 direction = hit.collider.transform.position - weaponTransformReference.position;
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
	
	void Update (){
		timerToCreateDecal -= Time.deltaTime;
		if(Input.GetButtonDown("Fire1") && currentRounds == 0 && !reloading && freeToShoot){
			PlayOutOfAmmoSound();
		}
		if(Input.GetButtonUp("Fire1")){
			freeToShoot = true;
		}
		HandleReloading();
		ShotTheTarget();
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
		enemyObject.TakeDamage((int)damageAmount, DamageType.BULLET);
		Debug.Log("Gun's range: "+fireRange + ", Distance: " +hit.distance+ ", Gun's damage: " + damageAmount);
	}
		
	//---------------AUDIO METHODS--------
	// These require Audio Source to be available on Gun Manager object
	public void PlayOutOfAmmoSound()
	{
		transform.parent.gameObject.audio.PlayOneShot(outOfAmmoSound, 1.5f);
	}
	
	public void PlayReloadSound()
	{
		transform.parent.gameObject.audio.PlayOneShot(reloadSound, 1.5f);
	}
	
	public void PlayShootSound()
	{
		transform.parent.gameObject.audio.PlayOneShot(shootSound);
	}	
}
