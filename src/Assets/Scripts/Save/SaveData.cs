using UnityEngine;
using UnitySerialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Reflection;
 
namespace UnitySerialization {
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
				try {
					// get field type
					Type type = Nullable.GetUnderlyingType(field.FieldType) ?? field.FieldType;
					// get field value
					var val = info.GetValue(field.Name, type);
					// convert value to correct type
					object convertedValue = (val == null) ? null : Convert.ChangeType(val, type);
					// set value into game's save container
					field.SetValue(SaveManager.instance.container, convertedValue);			
				} catch {
					Debug.LogWarning("Savegame doesn't contain value for " + field.Name + ", maybe you are loading from older savegame format?");
				}
			}
		}
		
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			// use reflection to save all public variables from save container
			foreach (FieldInfo field in SaveManager.instance.container.GetType().GetFields(bindingFlags)){
				// get field value
				var fieldValue = field.GetValue(SaveManager.instance.container);
	
				// get field type
				Type type = Nullable.GetUnderlyingType(field.FieldType) ?? field.FieldType;
				
				// add value to serialization container
				info.AddValue(field.Name, fieldValue);
			}	
		}
	}
}
