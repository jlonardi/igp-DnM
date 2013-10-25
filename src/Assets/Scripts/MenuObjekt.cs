using UnityEngine;
using System.Collections;

public class MenuObjekt : MonoBehaviour 
{
	
	void Start()
	{
		renderer.material.color=Color.red; //To get the text to be red in the beginning
	}
	public bool isQuit =false; //tells what will happen when pressing buttons
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
		else
		{
			Application.LoadLevel(1);  // loads main level
		}
	}
				
}

