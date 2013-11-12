using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace UnitySerialization {
	// This is required to guarantee a fixed serialization assembly name, which Unity likes to randomize on each compile
	public sealed class VersionDeserializationBinder : SerializationBinder
	{
		public override Type BindToType( string assemblyName, string typeName )	{
			if ( !string.IsNullOrEmpty( assemblyName ) && !string.IsNullOrEmpty( typeName ) ){			
				Type typeToDeserialize = null;
				assemblyName = Assembly.GetExecutingAssembly().FullName;
				typeToDeserialize = Type.GetType( String.Format( "{0}, {1}", typeName, assemblyName ) );
				return typeToDeserialize;
			}	 
			return null;
		}
	}
}