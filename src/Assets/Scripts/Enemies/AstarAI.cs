using UnityEngine;
using System.Collections;

using Pathfinding;

public class AstarAI : MonoBehaviour {
	
	public float movementSpeed;
	
	//The point to move to
    public Vector3 targetPosition;
    
    private Seeker seeker;
	
    private CharacterController controller;
	
	private Transform orcMesh;
	
	private Transform compass;
	
	private Transform player;
	private Vector3 oldPlayerPosition;
	
	private float timeOfLatestPathFind = 0f;
	
	private float timeOfLatestGraphUpdate = 0f;
	
	private float timeOfLatestGraphCreate = 4f;
	
	private bool atTarget = false;
 
    //The calculated path
    public Path path;
    
    //The AI's speed per second
    public float speed = 100;
	
	public float turnSpeed = 10f;
    
    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 0.5f;
 
    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;
	
    public void Start () {	
		
		GameObject p = GameObject.Find("Player");
		targetPosition = p.transform.position;
		
        //Get a reference to the Seeker component
        seeker = GetComponent<Seeker>();
		
		//Get a reference to the CharacterController component
		controller = GetComponent<CharacterController>();
		
		orcMesh = transform.FindChild("Sinbad");
		orcMesh.renderer.enabled = false;
		
		compass = transform.FindChild("Compass");
		
		GameObject playerObject = GameObject.Find("Player");
		player = playerObject.transform;
		oldPlayerPosition = player.position;
		
		createGraph();
		startNewPathfinding();
		
		//Start a new path to the targetPosition, return the result to the OnPathComplete function
        seeker.StartPath (transform.position,targetPosition, OnPathComplete);
    }
    
    public void OnPathComplete (Path p) {
        //Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
		if (!p.error) {
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 1;
        }
    }
	
	public void FixedUpdate () {
		
		createGraph();
		
		//updateGraph();
		
        if (path == null) {
            //We have no path to move after yet
            //return;
        } 
		
		if (orcMesh.renderer.enabled == false) {
			orcMesh.renderer.enabled = true;	
		}

		if (path != null && currentWaypoint >= path.vectorPath.Count) {
            Debug.Log ("End Of Path Reached");
			currentWaypoint = 0;
			atTarget = true;
            return;
        }
        
		if(newPathNeeded()) {
			if(eligibleToNewPathfind()) {
				Vector3 playerTerrainPosition = terrainLocation(player.position);
				targetPosition = playerTerrainPosition;
				oldPlayerPosition = playerTerrainPosition;
				startNewPathfinding();
				atTarget = false;
			}
		} 
		
		if(path != null) {
			moveObject();
		}
		
    }
	
	public void OnDisable () {
		
    	seeker.pathCallback -= OnPathComplete;
	} 
	
	private void startNewPathfinding() {
		// Sets the path to start at the terrain level
		Vector3 fixedPosition = new Vector3(transform.position.x,0,transform.position.z);
		fixedPosition.y = transform.position.y - (transform.position.y - terrainLocation(transform.position).y);
		seeker.StartPath (fixedPosition,targetPosition,OnPathComplete);	
	}
	
	// Gets the position at the terrain beneath the object
	private Vector3 terrainLocation(Vector3 objectPosition){
		Ray ray = new Ray(objectPosition, new Vector3(0,-1,0));
		RaycastHit hit;
		Physics.Raycast (ray,out hit);
		return hit.point;
	}
	
	private void moveObject() {
		if(!atTarget) {
			//Direction to the next waypoint
	        Vector3 dir = (path.vectorPath[currentWaypoint]-terrainLocation(transform.position)).normalized;
	        dir *= speed * Time.fixedDeltaTime;
	        controller.SimpleMove (dir);
		
			compass.rotation = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.Lerp(transform.rotation, compass.rotation, turnSpeed * Time.fixedDeltaTime);
			
			//transform.rotation = Quaternion.LookRotation(dir);
	        
	        //Check if we are close enough to the next waypoint
	        //If we are, proceed to follow the next waypoint
	        if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
	            currentWaypoint++;
	        }	
		} else {
			//Direction to the player
	        Vector3 dir = (player.transform.position-transform.position).normalized;
		
			//transform.rotation = Quaternion.LookRotation(dir);
			
			compass.rotation = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.Lerp(transform.rotation, compass.rotation, turnSpeed * Time.fixedDeltaTime);
			
			Debug.Log(compass.rotation);
		}
	}
	
	// Chekcs if there is a need to calculate a new path
	private bool newPathNeeded() {		
		float distanceFromPlayer = Vector3.Distance(transform.position, player.position);
		float distanceRatio = distanceFromPlayer/10;
		float minDistance = 3.0f;
		
		// update movementSpeed for animations
		if(distanceFromPlayer > minDistance) {
			movementSpeed = speed;
		} else {
			movementSpeed = 0.0f;
		}

		if(distanceFromPlayer > minDistance && 
			(Vector3.Distance(oldPlayerPosition, player.position) > distanceRatio || distanceFromPlayer < 20)  ) {
			return true;
		} 
		return false;			
	}
	
	private bool eligibleToNewPathfind() {
		//Debug.Log ("Time of the latest pathfind is " + timeOfLatestPathFind);
		//Debug.Log ("Current fixed time is " + Time.fixedTime);
		float updateInterval = 0.2f;
		if(timeOfLatestPathFind + updateInterval < Time.fixedTime  || Time.fixedTime < 3f) {
			timeOfLatestPathFind = Time.fixedTime;
			return true;
		}
		return false;
	}
	
	private void updateGraph() {
		if(timeOfLatestGraphUpdate + 3f < Time.fixedTime) {		
			Bounds bounds = new Bounds(new Vector3(258.2687f, -0.1f, 301.2399f), new Vector3(100, 0, 100));
			AstarPath.active.UpdateGraphs(bounds);
			timeOfLatestGraphUpdate = Time.fixedTime;
			Debug.Log("Graph updated");
			return;
		}
			
	}
	
	private void createGraph() {
		if(timeOfLatestGraphCreate + 3f < Time.fixedTime || Time.fixedTime < 3f) {
			GridGraph g = AstarPath.active.astarData.gridGraph;
			g.autoLinkGrids = true;
			g.center = transform.position;
			g.center.y = -0.1f;
			//g.maxSlope = 60;
			//g.collision.mask = "Obstacle";
			//g.
			Matrix4x4 m = g.matrix;
			g.GenerateMatrix();
			g.RelocateNodes (m, g.matrix);
			AstarPath.active.Scan();
			timeOfLatestGraphCreate = Time.fixedTime;
			return;
		}
	}
}
