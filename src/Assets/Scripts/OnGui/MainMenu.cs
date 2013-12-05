using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public enum MainMenuButton{
		START,
		STORY,
		LOAD,
		HIGHSCORE,
		QUIT
	}

	public MainMenuButton menuButton;

	void Start() {
		//To get the text to be red in the beginning
		renderer.material.color=Color.red;
	}
	
	void OnMouseEnter() {
		//when mouse is on text it will become white
		renderer.material.color=Color.white;
	}
	
	void OnMouseExit() {
		//when mouse leaves the text it will go back to red
		renderer.material.color= Color.red;
	}
	
	void Update(){
		if (GameManager.instance.gameState == GameState.LOAD_MENU_MAIN || GameManager.instance.gameState == GameState.STORY ||
		    GameManager.instance.gameState == GameState.HIGHSCORE_MAIN){
			renderer.enabled = false;
		} else {
			GameManager.instance.gameState = GameState.MAIN_MENU;
			renderer.enabled = true;
		}

	}
	
	void OnMouseDown(){
		switch(menuButton){
		case MainMenuButton.START:
			//loads first level
			GameManager.instance.levelState = LevelState.LOADING_NEWGAME;
			Application.LoadLevel("GameLevel");
			break;

		case MainMenuButton.STORY:
			//show story
			GameManager.instance.gameState = GameState.STORY;
			break;
			
		case MainMenuButton.HIGHSCORE:
			//show story
			GameManager.instance.gameState = GameState.HIGHSCORE_MAIN;
			break;
			
		case MainMenuButton.LOAD:
			// load previous save details
			SaveManager.instance.GetSaveInfo();

			//show load menu
			GameManager.instance.gameState = GameState.LOAD_MENU_MAIN;
			break;

		case MainMenuButton.QUIT:
			//if quit game button is pressed game quits
			Application.Quit();
			break;
		}
	}

				
}

