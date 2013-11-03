using UnityEngine;
using System.Collections;

using Pathfinding;

public class EnemyAI : MonoBehaviour {
	
	public float movementSpeed;
	
	//The point to move to
    public Vector3 targetPosition;
    
	//The seeker that makes the pathfinds
    private Seeker seeker;
	
	//Controller to handle the objects movement
    private CharacterController controller;
	
	//The object that contains the mesh of the object
	private Transform mesh;
	
	//Tells the rotation where the object has to be directed over time
	private Transform compass;
	
	//The players transform
	private Transform player;
	
	//The position where the player was previosly
	private Vector3 oldPlayerPosition;
	
	//The time when the latest pathfind was made to the object
	private float timeOfLastPathFind = 0f;
	
	//Tells if the object has reached the goal
	private bool atTarget = false;
		
	//Tells the status of the pathfind calculation
	private bool pathCalculationComplete = true;
 
    //The calculated path
    public Path path;
    
    //The speed per second
    public float speed = 100;
	
	//The turn speed per second
	public float turnSpeed = 10f;
    
    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 0.5f;
 
    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;
	
    public void Start () {	
		
		//Get a reference to the object that is targeted
		GameObject p = GameObject.Find("Player");
		targetPosition = p.transform.position;
		
        //Get a reference to the Seeker component
        seeker = GetComponent<Seeker>();
		
		//Get a reference to the CharacterController component
		controller = GetComponent<CharacterController>();
		
		//Turn of the rendered of this objects mesh
		//we don't want to draw it before the first path has been calculated
		mesh = transform.FindChild("Sinbad");
		mesh.renderer.enabled = false;
		
		//Get a reference to the Compass object
		compass = transform.FindChild("Compass");
		
		GameObject playerObject = GameObject.Find("Player");
		player = playerObject.transform;
		oldPlayerPosition = player.position;
		
		//Start a new path to the targetPosition, return the result to the OnPathComplete function
		startNewPathfinding();
        //seeker.StartPath (transform.position,targetPosition, OnPathComplete);
    }
    
    public void OnPathComplete (Path p) {
        //Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
		if (!p.error) {
            path = p;
            pathCalculationComplete = true;
            currentWaypoint = 1;
        }
    }
	
	public void FixedUpdate () {
		
		//Just for testing		
		if(Input.GetKeyDown(KeyCode.P)) {
			startNewPathfinding();
		}
			
		//When the path has been found draw the mesh
		if (path != null && mesh.renderer.enabled == false) {
			mesh.renderer.enabled = true;	
		}
		
		//If the last waypoint has been reach reset the current way point
		//and mark us so that we are at the target
		if (path != null && currentWaypoint >= path.vectorPath.Count) {
            //Debug.Log ("End Of Path Reached");
			currentWaypoint = 0;
			atTarget = true;
            return;
        }
		
		
		//Perform a new pathfind if needed
		if(pathCalculationComplete && newPathNeeded()) {
			if(eligibleToNewPathfind()) {
				Vector3 playerTerrainPosition = terrainLocation(player.position);
				targetPosition = playerTerrainPosition;
				oldPlayerPosition = playerTerrainPosition;
				startNewPathfinding();
				atTarget = false;
			}
		} 
		
		//If we have a path we have to move the object
		if(path != null) {
			moveObject();
		}
		
    }
	
	public void OnDisable () {
    	seeker.pathCallback -= OnPathComplete;
	} 
	
	private void startNewPathfinding() {
		//Debug.Log("Starting a new pathfind");
		
		//Sets the path to start at the terrain y coordinate not from the objects y coordinate
		Vector3 fixedPosition = new Vector3(transform.position.x,0,transform.position.z);
		fixedPosition.y = transform.position.y - (transform.position.y - terrainLocation(transform.position).y);
		
		//Mark that a pathfinding is on going
		pathCalculationComplete = false;
		
		//Starts the calculation for a new path
		seeker.StartPath (fixedPosition,targetPosition,OnPathComplete);	
		
	}
	
	//Gets the point of the terrain right underneath
	private Vector3 terrainLocation(Vector3 objectPosition){
		Ray ray = new Ray(objectPosition, new Vector3(0,-1,0));
		RaycastHit hit;
		Physics.Raycast (ray,out hit);
		return hit.point;
	}
	
	private void moveObject() {
		
		if(!atTarget) {	
			//If we are not yet at the target we have to face the object towards the target and
			//move it towards it using the playercontroller
			
			//Direction to the next waypoint
	        Vector3 dir = (path.vectorPath[currentWaypoint]-terrainLocation(transform.position)).normalized;
	        dir *= speed * Time.fixedDeltaTime;
	        controller.SimpleMove (dir);
		
			//Faces the compass at the next waypoint direction
			compass.rotation = Quaternion.LookRotation(dir);
			//Start moving the objoects facing towards the compass over time
			transform.rotation = Quaternion.Lerp(transform.rotation, compass.rotation, turnSpeed * Time.fixedDeltaTime);
			      
	        //Check if we are close enough to the next waypoint
	        //If we are, proceed to follow the next waypoint
	        if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
	            currentWaypoint++;
	        }	
		} else {
			//If we have allready reached the target we need only to turn the object to face hes target
			
			//Direction to the player
	        Vector3 dir = (player.transform.position-transform.position).normalized;
			
			compass.rotation = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.Lerp(transform.rotation, compass.rotation, turnSpeed * Time.fixedDeltaTime);
		}
	}
	
	//Chekcs if there is a need to calculate a new path
	private bool newPathNeeded() {		
		float distanceFromPlayer = Vector3.Distance(transform.position, player.position);
		float distanceRatio = distanceFromPlayer/10;
		float minDistance = 3.0f;
		float maxInterval = 10f;
		
		//Update movementSpeed for animations
		if(distanceFromPlayer > minDistance) {
			movementSpeed = speed;
		} else {
			movementSpeed = 0.0f;
		}
		
		if(distanceFromPlayer > minDistance) {
			//If enough time has been gone since the last pathfind we have to do a new one
			if(timeOfLastPathFind + maxInterval > Time.fixedTime) {
				return true;
			}
			//If the player has moved enough related to the distance between this object and him
			//perform a new pathfind to get a more accurate path
			if (Vector3.Distance(oldPlayerPosition, player.position) > distanceRatio)  {
				return true;
			}
			//If the object is allready pretty close to the player we need frequent path updates
			if(distanceFromPlayer < 20) {
				return true;	
			}
		} 
		return false;			
	}
	
	//Chekcs if enough time has passed since the last pathfind, too frequent pathfinds causes
	//severe performance issues
	private bool eligibleToNewPathfind() {
		float updateInterval = 0.2f;
		if(timeOfLastPathFind + updateInterval < Time.fixedTime) {
			timeOfLastPathFind = Time.fixedTime;
			//Debug.Log ("Eligible to find a new path.");
			return true;
		}
		return false;
	}
}
