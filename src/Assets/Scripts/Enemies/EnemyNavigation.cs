using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Pathfinding;

public class EnemyNavigation : MonoBehaviour {
	
	// AI stops moving below this distance
	public float moveStopDistance = 2.0f;
	
	// AI starts moving above this distance
	public float moveAllowDistance = 2.3f;
	
	public float movementSpeed = 0f;        
	private float prevMovementSpeed = 0f;        
	private List<Vector3> movementPositions = new List<Vector3>();
	
	//Use stuck values to determine if stuck and how long
	private bool stuck = false;
	private float timeStuck = 0f;
	
	//The point to move to
    public Vector3 targetPosition;
    
	//The seeker that makes the pathfinds
    private Seeker seeker;
	
	//Controller to handle the objects movement
    private CharacterController controller;

	//The players transform
	private Transform target;
	
	//The position where the target was previosly
	private Vector3 oldTargetPosition;
	
	//The time when the latest pathfind was made to the object
	private float timeOfLastPathFind = 0f;
	
	//Tells if the object has reached the goal
	public bool atTarget = false;
		
	//Tells the status of the pathfind calculation
	private bool pathCalculationComplete = true;
 
    //The calculated path
    public Path path;
    
    //The speed per second
    public float speed = 200f;
	
	//The turn speed per second
	public float turnSpeed = 3f;
    
    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3f;
 
    //The waypoint we are currently moving towards
    public int currentWaypoint = 0;

	public bool hideRenderer = true;

	public bool onLongDistanceTravel = false;
	public Vector3 longDistanceTarget;
	
    public void OnPathComplete (Path p) {
        //Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
		if (!p.error) {
            path = p;
            pathCalculationComplete = true;
            currentWaypoint = 1;
        }
    }

	private void SetRenderer(GameObject go, bool enableRenderer){
		hideRenderer = !enableRenderer;
		var renderers = go.GetComponentsInChildren<Renderer>();
		foreach (Renderer r in renderers) {
			r.enabled = enableRenderer;
		}					
	}	
	public void init(Transform t) {
		//Get a reference to the Seeker component
        seeker = GetComponent<Seeker>();
		
		//Get a reference to the CharacterController component
		controller = GetComponent<CharacterController>();

		//Turn of the rendered of this objects mesh
		//we don't want to draw it before the first path has been calculated
//		renderer = transform.FindChild<Renderer>();
//		mesh = transform.FindChild("Sinbad");
		SetRenderer(gameObject, false);
//		renderer.enabled = false;
		
/*		target = t;
		targetPosition = target.position;
		oldTargetPosition = target.position;
		
		if(Vector3.Distance(target.position, transform.position) > 150) {
			onLongDistanceTravel = true;
			longDistanceTarget = terrainLocation(target.position);
		}
		
		//Start a new path to the targetPosition, return the result to the OnPathComplete function
		startNewPathfinding();
        //seeker.StartPath (transform.position,targetPosition, OnPathComplete);
        */
	}
	
	public void FixedUpdate () {
		
		if(path == null) {
			return;			
		}
		
		//Just for testing		
		if(Input.GetKeyDown(KeyCode.P)) {
			startNewPathfinding();
		}
			
		//When the path has been found draw the mesh
		if (path != null && hideRenderer == true) {
			SetRenderer(gameObject, true);
		}
		
		atTarget = targetReached();
		
		//If the last waypoint has been reach reset the current way point
		//and mark us so that we are at the target
		if (!atTarget && path != null && currentWaypoint >= path.vectorPath.Count) {
           	//Debug.Log ("End Of Path Reached")
			currentWaypoint = 0;
			if(onLongDistanceTravel) {
				atTarget = false;
				startNewPathfinding();
				return;
			}
			atTarget = true;
			faceTarget();
           	return;
       	}
				
		//if we are at target, always face enemy towards the target
		if(atTarget){
			faceTarget();
		}
		
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
	
	public void setTarget(Transform targetTransform) {
		//Debug.Log ("Setting a new target, new target location is at " + t.position);
		target = targetTransform;
		targetPosition = targetTransform.position;
		startNewPathfinding();
	}
		
	// face enemy towards the target, just don't change y value so enemy will look straight
	private void faceTarget(){
		Vector3 enemyLookTarget = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
	    this.transform.LookAt(enemyLookTarget);
	}
	
	private bool targetReached() {
		
		float distanceFromPlayer = Vector3.Distance(transform.position, target.position);
		
		//Used if the object is doing a long travel
		if(onLongDistanceTravel) {
			if(Vector3.Distance(transform.position, longDistanceTarget) < moveStopDistance) {
				longDistanceTarget = target.position;
			} 
		}
		
		if(distanceFromPlayer >= moveAllowDistance) {
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
		if(onLongDistanceTravel) {
			//Debug.Log("Starting a long travel");
			//Debug.Log("Target location is " + longDistanceTarget);
			seeker.StartPath (fixedPosition,longDistanceTarget,OnPathComplete);	
		} else {
			seeker.StartPath (fixedPosition,targetPosition,OnPathComplete);	
		}
		
		timeOfLastPathFind = Time.fixedTime;
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
			
			//Start moving the objoects facing towards the direction over time
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.fixedDeltaTime);
			      
	        //Check if we are close enough to the next waypoint
	        //If we are, proceed to follow the next waypoint
			//Debug.Log ("Distance is " + Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]));
			//Debug.Log ("nextWaypointDistance = " + nextWaypointDistance);
			//Debug.Log (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance);
	        if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
				//Debug.Log("Moving to next waypoint");
	            currentWaypoint++;
				//Debug.Log("Waypoint addition done");
	        }	
		} else {
			//If we have allready reached the target we need only to turn the object to face hes target
			
			//Direction to the player
	        Vector3 dir = (target.transform.position-transform.position).normalized;
			
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.fixedDeltaTime);
		}

		//calculate current speed for animations
		movementPositions.Add(transform.position);
		if (movementPositions.Count>7){
			prevMovementSpeed = movementSpeed;
			movementSpeed = Vector3.Distance(transform.position, movementPositions[0]) * 30;
			movementPositions.RemoveAt(0);
			//Debug.Log(movementSpeed);
		}
	}
	
	private void setStuck(bool isStuck){
		if (isStuck){
			//Vector3 newWaypoint = terrainLocation(new Vector3(transform.position.x + Random.Range(-1f, 1f),
			//							transform.position.y, transform.position.z + Random.Range(-1f, 1f)));
			
			//Debug.Log("Stuck - speed: " + movementSpeed);
			if (!stuck){
				timeStuck = Time.time;
			}
			stuck = true;
		}
		if (!isStuck){
			//Debug.Log("Free again");
			stuck = false;
		}
	}
	
	//Chekcs if there is a need to calculate a new path
	private bool newPathNeeded() {		
		float distanceFromTarget = Vector3.Distance(transform.position, target.position);
		float distanceRatio = distanceFromTarget/10;
		float maxInterval = 10f;
		
		if(!atTarget) {
						
			//Since not at target and speed is low object is most likely stuck
			if ((stuck && movementSpeed < 0.5f) || prevMovementSpeed>= 0.5f && movementSpeed < 0.5f) {
				setStuck(true);
				if (timeStuck + 1 < Time.time){
					// get a new path since we are still stuck after 1 second
					return true;
				}
			} else if (stuck && movementSpeed > 0.5f) {
				setStuck(false); // unstuck if moving faster again
			}
				
			//If on a long pathfind we get close enogh to the target we need to start a normal pathfinding routine
			
			if(onLongDistanceTravel) {
				if(distanceFromTarget < 50) {
					Debug.Log ("We are close enough to the target, distance is " + Vector3.Distance(transform.position, target.position));
					onLongDistanceTravel = false;
					return true;
				} else {
					return false;	
				}
			}
			
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
			if(distanceFromTarget < 20) {
				return true;	
			}
		} 
		
		if(onLongDistanceTravel && atTarget) {
			return true;
		}
		
		return false;			
	}
	
	//Chekcs if enough time has passed since the last pathfind, too frequent pathfinds causes
	//severe performance issues
	private bool eligibleToNewPathfind() {
		float updateInterval = 0.2f;
		if(timeOfLastPathFind + updateInterval < Time.fixedTime) {
			//Debug.Log ("Eligible to find a new path.");
			return true;
		}
		return false;
	}
}
