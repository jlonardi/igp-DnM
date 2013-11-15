using UnityEngine;
using System.Collections;

using Pathfinding;

public class GraphManager : MonoBehaviour {
	
	private GridGraph levelGraph;

//	private Transform player;
//	private Transform tresaure;
	
	private float timeOfLastGraphUpdate = 0f;
	
	private float updateInterval = 2f;
	
	void Start () {
		
//		GameObject playerObject = GameObject.Find("Player");
//		player = playerObject.transform;
		
//		tresaure = Treasure.instance.gameObject.transform;
	
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
				updateGraph(levelGraph, new Vector3(160, -0.1f, 150));
			}
			timeOfLastGraphUpdate = Time.fixedTime;
			AstarPath.active.Scan();
		}
	}
	
	private void init() {
		
		levelGraph = (GridGraph)AstarPath.active.astarData.CreateGraph(typeof(GridGraph));
		initGraph(levelGraph, new Vector3(160, -0.1f, 150), 130, 130, 2);
		
//		Vector3 playerCenter = player.position;
//		playerCenter.y = -0.1f;
		
		//playerGraph = (GridGraph)AstarPath.active.astarData.CreateGraph(typeof(GridGraph));
		//initGraph(playerGraph, playerCenter, 50, 50, 1);
		
		//Vector3 tresaureCenter = tresaure.position;
		//tresaureCenter.y = -0.1f;
		
		//tresaureGraph = (GridGraph)AstarPath.active.astarData.CreateGraph(typeof(GridGraph));
		//initGraph(tresaureGraph, tresaureCenter, 10, 10, 1);
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
		g.collision.diameter = 1.5f;
		g.collision.height = 3;
		g.collision.collisionOffset = 2;
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
