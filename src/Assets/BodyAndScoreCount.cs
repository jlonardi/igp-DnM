using UnityEngine;
using System.Collections;

public class BodyAndScoreCount : MonoBehaviour {
	public static BodyAndScoreCount instance;
    public void Awake()
    {
		
        	BodyAndScoreCount.instance=this;
		
    }

	public int score;
	public int bodyCount;
	private float timeOfLastPoint = 0f;
	private float pointIntervall=1f;
	public void Update(){
	 	if(Treasure.instance.onGround){
			if((timeOfLastPoint + pointIntervall)<Time.time){
				score++;
				timeOfLastPoint=Time.time;
			}
		}
	}
	
}
