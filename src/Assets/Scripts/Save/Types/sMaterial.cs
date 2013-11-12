using UnityEngine;

namespace UnitySerialization {	
	[System.Serializable]
	public class sMaterial : sObject {
		public sColor color;
		public sTexture mainTexture;
		public sVector2 mainTextureOffset;
		public sVector2 mainTextureScale;
		//public int passCount; read only
		public int renderQueue;
		//public sShader shader;
		//public string[] shaderKeywords;
				
		public sMaterial() {
		}
		
		public sMaterial(Material m) {
		
		}			
	}			
}
