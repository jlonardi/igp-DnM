using UnityEngine;
using System.Collections;

// whole purpose of having a separate scene manager, is that since scene manager is not preserved
// during level loading, it's script will always trigger OnLevelWasLoaded
public class SceneManager : MonoBehaviour {
	public GameObject gameManagerPrefab;
	
	void Start(){
	    if (!(GameObject.FindWithTag("Game Manager"))){
			GameObject gameManager = (GameObject)Instantiate(gameManagerPrefab, Vector3.zero, Quaternion.identity);
			gameManager.name = "Game Manager";
		}				
	}
	
	void OnLevelWasLoaded(int level) {
		switch (level){
		case 0:	// main menu
			GameManager.instance.gameState = GameState.MAIN_MENU;
			break;
		default: // others are levels
			if (GameManager.instance.levelState == LevelState.LOADING_NEWGAME){
				GameManager.instance.levelState = LevelState.LOADED;
				GameManager.instance.NewGame();
				
			} else if (GameManager.instance.levelState == LevelState.LOADING_NEXT){
				GameManager.instance.levelState = LevelState.LOADED;

			} else if (GameManager.instance.levelState == LevelState.LOADING_SAVE){
				GameManager.instance.levelState = LevelState.LOADED;
				SaveManager.instance.container.RestoreValues();	
			}

			break;
		}        
    }
}
