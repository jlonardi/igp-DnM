using UnityEngine;

namespace UnitySerialization {
	[System.Serializable]
	public class sGUIText : sGUIElement {
		public TextAlignment alignment;
		public TextAnchor anchor;
		public sColor color;
		//public sFont font;
		public int fontSize;
		public FontStyle fontStyle;
		public float lineSpacing;
		//public Material material;
		public sVector2 pixelOffset;
		public bool richText;
		public float tabSize;
		public string text;

		public sGUIText() {
		}
		
		public sGUIText(GUIText g) {
		
		}	
	}
}
