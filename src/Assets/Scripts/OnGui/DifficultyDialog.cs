using UnityEngine;
using System.Collections;

public class DifficultyDialog {
	private GameManager game;
	private OnGuiManager gui;
	private int centerX;
	private int centerY;
	
	public void Initialize(){
		game = GameManager.instance;
		gui = OnGuiManager.instance;
		centerX = gui.GetCenterX();
		centerY = gui.GetCenterY();
	}
	
	// Show() gets called from OnGuiManager
	public void Show()
	{
		if (game == null){
			Initialize();
		}

		GUILayout.BeginArea(new Rect(centerX-220, 270,440,600));
		GUILayout.Label("Select Difficulty");
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(centerX-250, 390,500,600));
		if(GUILayout.Button ("Easy")){
			game.saves.difficulty = DifficultySetting.EASY;
			game.saves.levelState = LevelState.LOADING_NEW;
			//loads first level
			Application.LoadLevel("GameLevel");
		}
		if(GUILayout.Button ("Normal")){
			game.saves.difficulty = DifficultySetting.NORMAL;
			game.saves.levelState = LevelState.LOADING_NEW;
			//loads first level
			Application.LoadLevel("GameLevel");
		}
		if(GUILayout.Button ("Hard")){
			game.saves.difficulty = DifficultySetting.HARD;
			game.saves.levelState = LevelState.LOADING_NEW;
			//loads first level
			Application.LoadLevel("GameLevel");
		}
		if(GUILayout.Button ("Epic")){
			game.saves.difficulty = DifficultySetting.NIGHTMARE;
			game.saves.levelState = LevelState.LOADING_NEW;
			//loads first level
			Application.LoadLevel("GameLevel");
		}
		GUILayout.EndArea();
	}		
}

