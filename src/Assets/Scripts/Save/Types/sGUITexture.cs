using UnityEngine;

namespace UnitySerialization {
	[System.Serializable]
	public class sGUITexture : GUIElement {
		public sRectOffset border;
		public sColor color;
		public Rect pixelInset;
		public sTexture texture;

		public sGUITexture() {
		}
		
		public sGUITexture(GUITexture g) {
		
		}	
	}
}
