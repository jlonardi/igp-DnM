using UnityEngine;

namespace UnitySerialization {	
	[System.Serializable]
	public class sTexture2D : sTexture {
		public TextureFormat format;
		public int mipmapCount;
		
		public sTexture2D(){
		}
		
		public sTexture2D(int width, int height){
			this.width = width;
			this.height = height;
		}
		
		public sTexture2D(Texture2D t){
		}
	}
}
