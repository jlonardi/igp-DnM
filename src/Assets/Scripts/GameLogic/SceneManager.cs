using UnityEngine;
using System.Collections;

// whole purpose of having a separate scene manager, is that since scene manager is not preserved
// during level loading, it's script will always trigger OnLevelWasLoaded
public class SceneManager : MonoBehaviour {
	public GameObject gameManagerPrefab;


}
