using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
 
// all objects to be saved and loaded will be defined in here

// note that GameObjects are only partially serialized into sGameObjects, check SerializationExtensions.cs for details

[Serializable ()]
public class SaveData : ISerializable { 
	private GameObject player = GameObject.Find("Player");
	private List<sGameObject> sEnemies;
	private List<sGameObject> sBodies;	
	private List<sGameObject> sObjects;	
	private List<GameObject> Objects = new List<GameObject>();	
	private string saveName;
	
	// constructor, do not remove!
	public SaveData(){
	}

	public SaveData(string saveName){
		this.saveName = saveName;
	}
	
	public SaveData(SerializationInfo info, StreamingContext ctxt)
	{
		// clear items before load
		BeforeLoad();
						
		// restore general variables
		RestoreVar("savename", ref saveName, info);
		RestoreVar("spawnsLastWave", ref EnemySpawnManager.instance.timeOfLastWave, info);
		RestoreVar("treasureAmount", ref Treasure.instance.amount, info);
		RestoreVar("treasureOnGround", ref Treasure.instance.onGround, info);
		RestoreVar("playerArmor", ref PlayerHealth.instance.armor, info);
		RestoreVar("playerHealth", ref PlayerHealth.instance.health, info);
		RestoreVar("gamePlayTime", ref GameManager.instance.playTime, info);
		RestoreVar("gameWave", ref GameManager.instance.wave, info);
		RestoreVar("gunsCanUseWeapons", ref GunManager.instance.canUseWeapons, info);
		RestoreVar("gunsCurrentGunIndex", ref GunManager.instance.currentGunIndex, info);
		RestoreVar("gun0_rounds", ref GunManager.instance.guns[0].gun.currentRounds, info);
		RestoreVar("gun1_rounds", ref GunManager.instance.guns[1].gun.currentRounds, info);
		RestoreVar("gun0_clips", ref GunManager.instance.guns[0].gun.totalClips, info);
		RestoreVar("gun1_clips", ref GunManager.instance.guns[1].gun.totalClips, info);
		RestoreVar("bodycount", ref BodyAndScoreCount.instance.bodyCount, info);
		RestoreVar("score", ref BodyAndScoreCount.instance.score, info);
		
		// restore serialized GameObject lists
		RestoreVar("miscObjects", ref sObjects, info);
		RestoreVar("ragdolls", ref sBodies, info);
		RestoreVar("enemies", ref sEnemies, info);

		// restore GameObject with child transforms
		RestoreGameObject("playerGO", player, info);
		
		// restore GameObject without child transforms
		RestoreGameObjectWithoutChild("treasureGO", Treasure.instance.gameObject, info);

		// restore Vector2
		RestoreVector2("mouseAbsolute", ref SimpleSmoothMouseLook.instance._mouseAbsolute, info);		

		// restore Vector3
		
		// restore Quaternion
					
		// do afterwork for retrieving all objects
		AfterLoad();
	}
	 
	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		// make lists for saving
		BeforeSave();
	
		// save GameObject, Transform, Vector3 and Quaternion by adding .Serializable() extension
		info.AddValue("playerGO", (player.Serializable()));
		info.AddValue("treasureGO", (Treasure.instance.gameObject.Serializable()));	
//		info.AddValue("mouseRotation", (SimpleSmoothMouseLook.instance.yRotation.Serializable()));	
		info.AddValue("mouseAbsolute", (SimpleSmoothMouseLook.instance._mouseAbsolute.Serializable()));	
		
		// save regular variable here
		info.AddValue("savename", (saveName));
		info.AddValue("spawnsLastWave", (EnemySpawnManager.instance.timeOfLastWave));
		info.AddValue("playerArmor", (PlayerHealth.instance.armor));
		info.AddValue("playerHealth", (PlayerHealth.instance.health));
		info.AddValue("treasureAmount", (Treasure.instance.amount));
		info.AddValue("treasureOnGround", (Treasure.instance.onGround));
		info.AddValue("gamePlayTime", (GameManager.instance.playTime));
		info.AddValue("gameScore", (GameManager.instance.score));
		info.AddValue("gameBodyCount", (GameManager.instance.bodyCount));
		info.AddValue("gameWave", (GameManager.instance.wave));
		info.AddValue("gunsCanUseWeapons", (GunManager.instance.canUseWeapons));
		info.AddValue("gun0_rounds", (GunManager.instance.guns[0].gun.currentRounds));
		info.AddValue("gun1_rounds", (GunManager.instance.guns[1].gun.currentRounds));
		info.AddValue("gun0_clips", (GunManager.instance.guns[0].gun.totalClips));
		info.AddValue("gun1_clips", (GunManager.instance.guns[1].gun.totalClips));
		info.AddValue("gun1", (GunManager.instance.guns[1].gun));
		info.AddValue("gunsCurrentGunIndex", (GunManager.instance.currentGunIndex));
		info.AddValue("bodycount", (BodyAndScoreCount.instance.bodyCount));
		info.AddValue("score", (BodyAndScoreCount.instance.score));
		
		// save lists here
		info.AddValue("enemies", (sEnemies));
		info.AddValue("ragdolls", (sBodies));
		info.AddValue("miscObjects", (sObjects));				
	}	
	

	
	
	//================ Helper methods ==========================================

	// restore GameObject
	public void RestoreGameObject(string value, GameObject go, SerializationInfo info){
		try{
			sGameObject savedObj = (sGameObject)info.GetValue(value, typeof(sGameObject));
			go.fromSerialized(savedObj);
		} catch {
			Debug.LogWarning("Coudn't find value for " + value + ", saved game may be using an older save format");
		}		
    }	
	
	public void RestoreGameObjectWithoutChild(string value, GameObject go, SerializationInfo info){
		try{
			sGameObject savedObj = (sGameObject)info.GetValue(value, typeof(sGameObject));
			go.fromSerializedWithoutChild(savedObj);
		} catch {
			Debug.LogWarning("Coudn't find value for " + value + ", saved game may be using an older save format");
		}		
    }	
	
	// restore Vector2
	public void RestoreVector2(string value, ref Vector2 v2, SerializationInfo info){
		try{
			sVector2 savedV2 = (sVector2)info.GetValue(value, typeof(sVector2));
			v2 = savedV2.toVector2;
		} catch {
			Debug.LogWarning("Coudn't find value for " + value + ", saved game may be using an older save format");
		}		
    }
	// restore Vector3
	private void RestoreVector3(string value, ref Vector3 v3, SerializationInfo info){
		try{
			sVector3 savedV3 = (sVector3)info.GetValue(value, typeof(sVector3));
			v3 = savedV3.toVector3;
		} catch {
			Debug.LogWarning("Coudn't find value for " + value + ", saved game may be using an older save format");
		}		
    }
	// restore Quaternion
	private void RestoreQuaternion(string value, ref Quaternion q, SerializationInfo info){
		try{
			sQuaternion savedQ = (sQuaternion)info.GetValue(value, typeof(sQuaternion));
			q = savedQ.toQuaternion;
		} catch {
			Debug.LogWarning("Coudn't find value for " + value + ", saved game may be using an older save format");
		}		
    }
	// restore any other variable
	private void RestoreVar<T>(string value, ref T variable, SerializationInfo info){
		try{
			T savedVariable = (T)info.GetValue(value, typeof(T));
			variable = savedVariable;
		} catch {
			Debug.LogWarning("Coudn't find value for " + value + ", saved game may be using an older save format");
		}		
    }
	
	private void BeforeSave(){
		// make new lists
		sEnemies = new List<sGameObject>();		
		sBodies	 = new List<sGameObject>();
		sObjects = new List<sGameObject>();

		// save enemies and misc objects by listing them
		foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]){
    		if(go.name.Equals("orc(Clone)")){
    			sEnemies.Add(go.Serializable());
    		}
    		if(go.name.Equals("orc ragdoll(Clone)")){				
    			sBodies.Add(go.Serializable());
    		}
			if(go.name.Equals("1x1x1m Box")){
				sObjects.Add(go.Serializable());
			}				
    	}		
	}
	
	private void BeforeLoad(){
		// clear enemy objects before loading save
		EnemySpawnManager.instance.ClearEnemies();
		RagdollManager.instance.ClearBodies();
	}

	private void AfterLoad(){		
		// always select correct gun after loading savegame
		GunManager.instance.ChangeToGun(GunManager.instance.currentGunIndex);
		// if treasure on ground, make sure animation states are correct by calling SetTreasureOnGround
		if (Treasure.instance.onGround){
			Treasure.instance.SetTreasureOnGround();
		}
		
		foreach (sGameObject sgo in sBodies){
			GameObject go =	RagdollManager.instance.instantiateRagdoll(EnemyType.ORC, 
										sgo.transform.position.toVector3, sgo.transform.rotation.toQuaternion);
//			go.SetActive(false);
			sgo.restoreChildTransforms(go.transform);
//			RecursiveDeepActivation(go.transform);
		}
		
		foreach (sGameObject sgo in sEnemies){
    		if(sgo.name.Equals("orc(Clone)")){
				EnemySpawnManager.instance.instantiateEnemy(EnemyType.ORC, sgo.transform.position.toVector3, 
																sgo.transform.rotation.toQuaternion);
    		}
    	}		
		
		// find misc objects on scene		
		foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]){
			if(go.name.Equals("1x1x1m Box")){
				Objects.Add(go);
			}				
    	}
		
		// failsafe check for object count
		if (Objects.Count != sObjects.Count){
			Debug.Log("ERROR: Object counts do not match!");
			return;			
		}
		
		for (int i=0; i < Objects.Count; i++){
			Objects[i].transform.position = sObjects[i].transform.position.toVector3;
			Objects[i].transform.rotation = sObjects[i].transform.rotation.toQuaternion;
    	}
	}
}