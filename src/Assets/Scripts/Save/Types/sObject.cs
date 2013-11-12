using UnityEngine;

namespace UnitySerialization {	
	[System.Serializable]
	public class sObject {
		public HideFlags hideFlags;
		public string name;
		public int instanceID;
		
		public sObject(){
		}
		
		public sObject(Object o){
			hideFlags = o.hideFlags;
			name = o.name;
			instanceID = o.GetInstanceID();
		}
		
		//Returns the instance id of the object.
		public int GetInstanceID(){
			return instanceID;
		}
		
		//Returns the name of the game object.
		override public string ToString(){
			return this.name;
		}
		
	/*	//Removes a gameobject, component or asset.
		public static void Destroy(){
		}
		
		//Destroys the object obj immediately. You are strongly recommended to use Destroy instead.
		public static void DestroyImmediate(){
		}
		
		//Makes the object target not be destroyed automatically when loading a new scene.
		public static void DontDestroyOnLoad(){
		}
		
		//Returns the first active loaded object of Type type.
		public static void FindObjectOfType(){
		}
		
		//Returns a list of all active loaded objects of Type type.
		public static void FindObjectsOfType(){
		}
		
		//Clones the object original and returns the clone.
		public static void Instantiate(){
		}
		*/
		
		/* bool - check if null, operator not needed? 
		
		public static bool operator !=(sObject o1, sObject o2){
			if (o1.GetInstanceID() != o2.GetInstanceID()){
				return true;
			}
			return false;
		}
		public static bool operator ==(sObject o1, sObject o2){
			if (o1.GetInstanceID() == o2.GetInstanceID()){
				return true;
			}
			return false;
		}*/	
/*		
		public sGameObject(GameObject go) {
			activeInHierarchy = go.activeInHierarchy;
			activeSelf = go.activeSelf;
			layer = go.layer;
			name = go.name;
			tag = go.tag;
			
			if (go.transform != null){
				transform = new sTransform(go.transform);
			}
			
			// put all transforms into list recursively
	//		storeChildTransforms(go.transform, recursiveTransforms);
		}
			 *//*
		public GameObject toGameObject() { 
			GameObject go = new GameObject();		
			return toGameObject(go);
		}
		
		public GameObject toGameObject(GameObject go) { 
			go.SetActive(activeSelf);
			go.name = name;
			go.tag = tag;
			
			// if parents do not match, find correct parent and change it
			string goParentStr = "";
			string thisParentStr = "";
			if (go.transform.parent !=  null){
				goParentStr = go.transform.parent.name;
			}
			if (transform.parent != null){
				thisParentStr = transform.parent.name;
			}
			if (!goParentStr.Equals(thisParentStr) && !thisParentStr.Equals("")){
				GameObject realParent = GameObject.Find(transform.parent.name);
				if (realParent != null){
					go.transform.parent = realParent.transform;
				}
			}
			
			//set transformation after we made sure we have the correct parent
			transform.toTransform(go);
			restoreChildTransforms(go.transform);
			return go;
		}
		
		public void restoreChildTransforms(Transform parent){
			List<Transform> parentRecursiveTransforms = new List<Transform>();
			List<sTransform> recursiveTransforms = new List<sTransform>();
			
			Transform rParent;
			sTransform bParent;
			string rParentStr, bParentStr;
			
			// put all transforms into list recursively
	//		storeChildTransforms(this.transform, recursiveTransforms);
			
			storeChildTransforms(parent, parentRecursiveTransforms);
			
			foreach (sTransform b in recursiveTransforms){
				foreach(Transform r in parentRecursiveTransforms){
					//get parents too to make sure we have the same exact same object and not only similarly named
					rParent = r.parent;
					bParent = b.parent;
					if (rParent == null){
						rParentStr = "null";
					} else {
						rParentStr = rParent.name;
					}
					if (bParent == null){
						bParentStr = "null";
					} else {
						bParentStr = bParent.name;
					}
					
					if (r.name.Equals(b.name) && rParentStr.Equals(bParentStr)){
						r.rotation = b.rotation.toQuaternion;					
						r.position = b.position.toVector3;
						if (r.rigidbody != null){						
							r.rigidbody.velocity = Vector3.zero;
							r.rigidbody.angularVelocity = Vector3.zero;
							r.rigidbody.isKinematic = true;
						}
						break;
					}
				}
			}		
		}*/
		/*
		public void disableKinematic(Transform parent){
			List<Transform> parentRecursiveTransforms = new List<Transform>();
			List<sTransform> recursiveTransforms = new List<sTransform>();
			
			storeChildTransforms(parent, parentRecursiveTransforms);
			
			foreach (sTransform b in recursiveTransforms){
				foreach(Transform r in parentRecursiveTransforms){
					if (r.name == b.name){
						if (r.rigidbody != null){						
							r.rigidbody.isKinematic = false;
						}
						break;
					}
				}
			}		
		}	*/
		/*
		private void storeChildTransforms(sTransform parent, List<sTransform> list){
			if (parent==null){
				return;
			}
			
			list.Add(parent);
			foreach(sTransform st in parent){
				storeChildTransforms(st,list);
			}
		}
		
		private void storeChildTransforms(Transform parent, List<Transform> list){
			if (parent==null){
				return;
			}
			
			list.Add(parent);
			foreach(Transform t in parent){
				storeChildTransforms(t,list);
			}
		}			*/
		
	}
}