using UnityEngine;

namespace UnitySerialization {
	[System.Serializable]
	public class sCamera : sBehaviour {
		//public static sCamera[] allCameras;
		//public static sCamera current;
		//public static sCamera main;	
		//public sRenderingPath actualRenderingPath;
		public float aspect;
		public sColor backgroundColor;
		//public sMatrix4x4 cameraToWorldMatrix;
		public CameraClearFlags clearFlags;
		public int cullingMask;
		public float depth;
		public DepthTextureMode depthTextureMode;
		public int eventMask;
		public float farClipPlane;
		public float fieldOfView;
		public bool hdr;
		public float[] layerCullDistances;
		public bool layerCullSpherical;
		public float nearClipPlane;
		public bool orthographic;
		public float orthographicSize;
		public float pixelHeight;
		public Rect pixelRect;
		public float pixelWidth;
		//public sMatrix4x4 projectionMatrix;
		public Rect rect;
		public RenderingPath renderingPath;
		//public sRenderTexture targetTexture; render texture unity pro only
		public TransparencySortMode transparencySortMode;
		public bool useOcclusionCulling;
		public sVector3 velocity;
		//public sMatrix4x4 worldToCameraMatrix;
		
		public sCamera() {
		}
		
		public sCamera(Camera c) {
		
		}
	}
}
