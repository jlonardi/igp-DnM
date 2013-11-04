using UnityEngine;
using System.Collections;

using Pathfinding;

public class EnemyAI : MonoBehaviour {
	
	public float movementSpeed = 0f;	
	
	//The point to move to
    public Vector3 targetPosition;
    
	//The seeker that makes the pathfinds
    private Seeker seeker;
	
	//Controller to handle the objects movement
    private CharacterController controller;
	
	//The object that contains the mesh of the object
	private Transform mesh;
	
	//The players transform
	private Transform target;
	
	//The position where the target was previosly
	private Vector3 oldTargetPosition;
	
	//The time when the latest pathfind was made to the object
	private float timeOfLastPathFind = 0f;
	
	//Tells if the object has reached the goal
	private bool atTarget = false;
		
	//Tells the status of the pathfind calculation
	private bool pathCalculationComplete = true;
 
    //The calculated path
    public Path path;
    
    //The speed per second
    public float speed = 200f;
	
	//The turn speed per second
	public float turnSpeed = 3f;
    
    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 0.5f;
 
    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;
	
    public void Start () {	
		
		//Get a reference to the object that is targeted
		//GameObject p = GameObject.Find("arkku");
		//targetPosition = p.transform.position;
		
        //Get a reference to the Seeker component
        seeker = GetComponent<Seeker>();
		
		//Get a reference to the CharacterController component
		controller = GetComponent<CharacterController>();
		
		//Turn of the rendered of this objects mesh
		//we don't want to draw it before the first path has been calculated
		mesh = transform.FindChild("Sinbad");
		mesh.renderer.enabled = false;
		
		GameObject targetObject = GameObject.Find("arkku");
		target = targetObject.transform;
		targetPosition = target.position;
		oldTargetPosition = target.position;
		
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
			currentWaypoint = 2;
			atTarget = true;
            return;
        }
		
		//Check if we are at the target yet
		atTarget = targetReached();
		
		//Perform a new pathfind if needed
		if(pathCalculationComplete && newPathNeeded()) {
			if(eligibleToNewPathfind()) {
				 Vector3 terrainPosition = terrainLocation(target.position);
                 targetPosition = terrainPosition;
                 oldTargetPosition = terrainPosition;
                 startNewPathfinding();
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
	
	public void setTarget(Transform t) {
		//Debug.Log ("Setting a new target, new target location is at " + t.position);
		target = t;
		targetPosition = t.position;
		startNewPathfinding();
	}
	
	public bool isAtTarget() {
		return atTarget;	
	}
	
	private bool targetReached() {
		
		float distanceFromPlayer = Vector3.Distance(transform.position, target.position);
		float minDistance = 3.0f;
		
		if(distanceFromPlayer > minDistance) {
			return false;
		} else {
			return true;
		}
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
			
			Vector3 positionBeforeMove = transform.position;		
			controller.SimpleMove (dir);
			
			//calculate current speed
			movementSpeed = Vector3.Distance(transform.position, positionBeforeMove) * speed;

			//Start moving the objoects facing towards the direction over time
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.fixedDeltaTime);
			      
	        //Check if we are close enough to the next waypoint
	        //If we are, proceed to follow the next waypoint
	        if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
	            currentWaypoint++;
	        }	
		} else {
			//If we have allready reached the target we need only to turn the object to face hes target
			
			//Direction to the player
	        Vector3 dir = (target.transform.position-transform.position).normalized;
			
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.fixedDeltaTime);
			
			movementSpeed = 0;
		}
	}
	
	//Chekcs if there is a need to calculate a new path
	private bool newPathNeeded() {		
		float distanceFromPlayer = Vector3.Distance(transform.position, target.position);
		float distanceRatio = distanceFromPlayer/10;
		float maxInterval = 10f;
		
		if(!atTarget) {
			//If enough time has been gone since the last pathfind we have to do a new one
			if(timeOfLastPathFind + maxInterval > Time.fixedTime) {
				return true;
			}
			//If the player has moved enough related to the distance between this object and him
			//perform a new pathfind to get a more accurate path
			if (Vector3.Distance(oldTargetPosition, target.position) > distanceRatio)  {
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
