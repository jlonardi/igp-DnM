using UnityEngine;
using System.Collections;

public class MenuObjekt : MonoBehaviour 
{
	public bool isQuit =false; //tells what will happen when pressing buttons
	public bool isLoadLevel=false;
	
	private float screen_width=Screen.width;
	private float screen_height=Screen.height; //putting screen size here to optimaze code
	
	void Start()
	{
		renderer.material.color=Color.red; //To get the text to be red in the beginning
	}
	
	void OnMouseEnter()
	{
		renderer.material.color=Color.white;	//when mouse is on text it will become white
	}
	
	void OnMouseExit()
	{
		renderer.material.color= Color.red; //when mouse leaves the text it will go back to red
	}
	
	void OnMouseDown() //if mouse is pressed on text somthing happens
	{
		if(isQuit)
		{
			Application.Quit(); //if quit game button is pressed game quits
		}
		
		else if(isLoadLevel) 
		{
			
			GetComponent<LoadLevelMenu>().showLoadLevelMenu=true;
			
		}
		
		else
		{
			Application.LoadLevel("GameLevel");  // loads main level
		}
	}

				
}

