using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public bool isQuit = false; //tells what will happen when pressing buttons
	public bool isLoadLevel = false;
	
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
		if (GameManager.instance.gameState == GameState.MAIN_MENU){
			renderer.enabled = true;
			//this.enabled = true;
		}
		if (GameManager.instance.gameState == GameState.LOAD_MENU_MAIN){
			renderer.enabled = false;
			//this.enabled = false;
		}
	}
	
	void OnMouseDown(){
		if(isQuit){
			//if quit game button is pressed game quits
			Application.Quit();
			
		} else if(isLoadLevel){
			// load previous save details
			SaveManager.instance.GetSaveInfo();

			//show load menu
			GameManager.instance.gameState = GameState.LOAD_MENU_MAIN;
			
		} else {
			//show story
			GameManager.instance.gameState = GameState.STORY;
			
		}
	}

				
}

