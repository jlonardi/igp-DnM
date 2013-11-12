using UnityEngine;

namespace UnitySerialization {
	[System.Serializable]
	public class sLight : sBehaviour {
		public bool alreadyLightmapped;
		public sVector2 areaSize;
		public sColor color;
		public sTexture cookie;
		public int cullingMask;
		public sFlare flare;
		public float intensity;
		public float range;
		public LightRenderMode renderMode;
		public float shadowBias;
		public LightShadows	shadows;
		public float shadowSoftness;
		public float shadowSoftnessFade;
		public float shadowStrength;
		public float spotAngle;
		public LightType type;
				
		public sLight() {
		}
		
		public sLight(Light l) {
		
		}	
	}
}