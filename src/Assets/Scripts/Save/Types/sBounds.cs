using UnityEngine;

namespace UnitySerialization {
	[System.Serializable]
	public struct sBounds {
		public sVector3 center;
		public sVector3 extents;
		public sVector3 max;
		public sVector3 min;
		public sVector3 size;
	}
	
}