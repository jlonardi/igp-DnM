using UnityEngine;
using System.Collections;

public class MenuObjekt : MonoBehaviour 
{
	public bool isQuit =false; //tells what will happen when pressing buttons
	void OnMouseEnter()
	{
		renderer.material.color=Color.red;	//when mouse is on text it will become red
	}
	void OnMouseExit()
	{
		renderer.material.color= Color.white; //when mouse leaves the text it will go back to white
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

