using UnityEngine;
using System.Collections;

using Pathfinding;

public class GraphManager : MonoBehaviour {
	
	private GridGraph levelGraph;
	private GridGraph playerGraph;
	
	private Transform player;
	
	private float timeOfLastGraphUpdate = 0f;
	
	private float updateInterval = 3f;
	
	void Start () {
		
		GameObject playerObject = GameObject.Find("Player");
		player = playerObject.transform;
	
		init();
		
		// Does a scan to all graphs
		AstarPath.active.Scan();
		
	}
	
	void FixedUpdate () {
		
		updateGraphs();

	}
	
	private void updateGraphs() {
		// Updates the graphs if there has been passed enough time since last update
		if(timeOfLastGraphUpdate + updateInterval < Time.fixedTime) {
			if(levelGraph != null) {
				updateGraph(levelGraph, new Vector3(495, -0.1f, 495));
			}
			if(playerGraph != null) {
				updateGraph(playerGraph, new Vector3(player.position.x, -0.1f, player.position.z));
			}
			timeOfLastGraphUpdate = Time.fixedTime;
			AstarPath.active.Scan();
		}
	}
	
	private void init() {
		
		initGraph(levelGraph, new Vector3(495, -0.1f, 495), 100, 100, 10);
		
		Vector3 playerCenter = player.position;
		playerCenter.y = -0.1f;
		initGraph(playerGraph, playerCenter, 50, 50, 1);
		
		Debug.Log("Initialized graphs");
	}
	
	private void updateGraph(GridGraph g, Vector3 center) {
		if(g != null && (timeOfLastGraphUpdate + 3f < Time.fixedTime || Time.fixedTime < 3f)) {
			g.center = center;
			Matrix4x4 m = g.matrix;
			g.GenerateMatrix();
			g.RelocateNodes (m, g.matrix);
			timeOfLastGraphUpdate = Time.fixedTime;
			return;
		}	
	}
	
	private void initGraph(GridGraph g, Vector3 center, int width, int depth, int nodeSize) {
		
		g = (GridGraph)AstarPath.active.astarData.CreateGraph(typeof(GridGraph));
		
		g.center = center;
		g.width = width;
		g.depth = depth;
		g.nodeSize = nodeSize;
		g.maxSlope = 50;
		g.maxClimb = 0;
		
		g.autoLinkGrids = true;
		
		g.collision.collisionCheck = true;
		g.collision.diameter = 2;
		g.collision.height = 3;
		int obstacleLayer = LayerMask.NameToLayer("Obstacle");
		int obstacleMask = 1 << obstacleLayer;
		g.collision.mask = obstacleMask;
		g.collision.heightCheck = true;
		int groundLayer = LayerMask.NameToLayer("Ground");
		int groundMask = 1 << groundLayer;
		
		
		g.collision.heightMask = groundMask;
		
		g.UpdateSizeFromWidthDepth ();
		AstarPath.active.astarData.AddGraph(g);	
	}
}
