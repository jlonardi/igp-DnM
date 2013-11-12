using UnityEngine;

namespace UnitySerialization {	
	[System.Serializable]
	public class sTexture : sObject {
		public int anisoLevel;
		public FilterMode filterMode;
		public float mipMapBias;
		public WrapMode wrapMode;
		protected int internalHeight;
		private int internalWidth;
		
		public sTexture(){
		}
		
		public sTexture(Texture t){
		}
		
		public int height { 
			get {
				return internalHeight;
			} 
			set {
				internalHeight = value;
			}
		}	
		public int width { 
			get {
				return internalWidth;
			} 
			set {
				internalWidth = value;
			}
		}	
		
	}
}
