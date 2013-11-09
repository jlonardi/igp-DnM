using UnityEngine;
using System;
using System.Collections;

public static class SerializationExtensions {
    
	public static sVector2 Serializable(this Vector2 v2) {
        return new sVector2(v2);
    }

	public static sVector3 Serializable(this Vector3 v3) {
        return new sVector3(v3);
    }
	
    public static sQuaternion Serializable(this Quaternion q) {
        return new sQuaternion(q);
    }
	
    public static sTransform Serializable(this Transform t) {
        return new sTransform(t);
    }

	public static sGameObject Serializable(this GameObject go) {
        return new sGameObject(go);
    }

    public static Quaternion fromSerialized(this Quaternion q, sQuaternion sq) {
        return sq.toQuaternion;
    }

	public static GameObject fromSerialized(this GameObject go, sGameObject sgo) {
        return sgo.toGameObject(ref go);
    }
	
	public static GameObject fromSerializedWithoutChild(this GameObject go, sGameObject sgo) {
        return sgo.toGameObject(ref go, false);
    }	

}