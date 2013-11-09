using UnityEngine;
using UnityEditor;
using System.Collections;

public class SceneManager : MonoBehaviour {
	public GameObject gameManagerPrefab;
	
	void Start(){
	    if (!(GameObject.FindWithTag("Game Manager"))){
			GameObject gameManager = (GameObject)Instantiate(gameManagerPrefab, Vector3.zero, Quaternion.identity);
			gameManager.name = "Game Manager";
		}				
	}
	
	void OnLevelWasLoaded(int level) {
		// level 1 = GameLevel.scene
		switch (level){
		case 0:	// main menu
			GameManager.instance.gameState = GameState.MAIN_MENU;
			break;
		case 1: // level 1 = GameLevel.scene
            GameManager.instance.NewGame();
			break;
		}        
    }
}
