using UnityEngine;

namespace UnitySerialization {	
	[System.Serializable]
	public class sRenderer : sComponent {		
		public sBounds bounds;
		public bool castShadows;
		public bool enabled;
		public bool isPartOfStaticBatch;
		public bool isVisible;
		public int lightmapIndex;
		public sVector4 lightmapTilingOffset;
		public sTransform lightProbeAnchor;
		//public sMatrix4x4 localToWorldMatrix;
		public sMaterial material;
		//public sMaterial[] materials;
		public bool receiveShadows;
		public sMaterial sharedMaterial;
		//public sMaterial[] sharedMaterials;
		public bool useLightProbes;
		//public sMatrix4x4	worldToLocalMatrix
		
		public sRenderer() {
		}
		
		public sRenderer(Renderer r) {
		
		}			
	}
}
