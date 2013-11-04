using UnityEngine;
using System.Collections;

using Pathfinding;

public class GraphManager : MonoBehaviour {
	
	private GridGraph levelGraph;
	private GridGraph playerGraph;
	private GridGraph tresaureGraph;
	
	private Transform player;
	private Transform tresaure;
	
	private float timeOfLastGraphUpdate = 0f;
	
	private float updateInterval = 2f;
	
	void Start () {
		
		GameObject playerObject = GameObject.Find("Player");
		player = playerObject.transform;
		
		GameObject trsaureObject = GameObject.Find("arkku");
		tresaure = trsaureObject.transform;
	
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
			//Debug.Log("Time to update the graphs!");
			if(levelGraph != null) {
				updateGraph(levelGraph, new Vector3(495, -0.1f, 495));
			}
			if(playerGraph != null) {
				//Debug.Log("Updating the player graph!");
				updateGraph(playerGraph, new Vector3(player.position.x, -0.1f, player.position.z));
			}
			if(tresaureGraph != null) {
				//Debug.Log("Updating the player graph!");
				updateGraph(tresaureGraph, new Vector3(tresaure.position.x, -0.1f, tresaure.position.z));
			}
			timeOfLastGraphUpdate = Time.fixedTime;
			AstarPath.active.Scan();
		}
	}
	
	private void init() {
		
		levelGraph = (GridGraph)AstarPath.active.astarData.CreateGraph(typeof(GridGraph));
		initGraph(levelGraph, new Vector3(495, -0.1f, 495), 140, 140, 7);
		
		Vector3 playerCenter = player.position;
		playerCenter.y = -0.1f;
		
		playerGraph = (GridGraph)AstarPath.active.astarData.CreateGraph(typeof(GridGraph));
		initGraph(playerGraph, playerCenter, 50, 50, 1);
		
		Vector3 tresaureCenter = tresaure.position;
		tresaureCenter.y = -0.1f;
		
		//tresaureGraph = (GridGraph)AstarPath.active.astarData.CreateGraph(typeof(GridGraph));
		//initGraph(tresaureGraph, tresaureCenter, 50, 50, 1);
		//Debug.Log("Initialized graphs");
	}
	
	private void updateGraph(GridGraph g, Vector3 center) {
		g.center = center;
		Matrix4x4 m = g.matrix;
		g.GenerateMatrix();
		g.RelocateNodes (m, g.matrix);
		//Debug.Log("Graph updated!");
		return;

	}
	
	private void initGraph(GridGraph g, Vector3 center, int width, int depth, int nodeSize) {
		
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
