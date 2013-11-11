﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Reflection;
 

[Serializable ()]
public class SaveData : ISerializable { 
	BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

	// constructor, do not remove!
	public SaveData(){
	}
	
	public SaveData(SerializationInfo info, StreamingContext ctxt)
	{		
		// use reflection to restore all public variables from save container
		foreach (FieldInfo field in SaveManager.instance.container.GetType().GetFields(bindingFlags)){
			// get field type
			Type type = Nullable.GetUnderlyingType(field.FieldType) ?? field.FieldType;
			// get field value
			var val = info.GetValue(field.Name, type);
			// convert value to correct type
			object safeValue = (val == null) ? null : Convert.ChangeType(val, type);
			// set value into game's save container
			field.SetValue(SaveManager.instance.container, safeValue);
		}	
		
		// when are saved data is stored in save container, load level
		GameManager.instance.levelState = LevelState.LOADING_SAVE;
		Application.LoadLevel(1); //levelNumber							
	}
	
	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		// use reflection to save all public variables from save container
		foreach (FieldInfo field in SaveManager.instance.container.GetType().GetFields(bindingFlags)){
			// get field value
			var fieldValue = field.GetValue(SaveManager.instance.container);
			// add value to serialization container
			info.AddValue(field.Name, fieldValue);
		}	
	}	
}