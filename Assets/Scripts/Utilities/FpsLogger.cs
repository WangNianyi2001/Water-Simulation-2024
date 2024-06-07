using UnityEngine;

namespace Nianyi.UnityPlayground {
	public class FpsLogger : MonoBehaviour {
		public enum LogMode { Frame, Physics }

		#region Serialized fields
		public LogMode logMode = LogMode.Physics;
		#endregion

		#region Life cycle
		protected void Start() {
			lastTimestamp = Time.timeSinceLevelLoad;
		}

		protected void Update() {
			if(logMode == LogMode.Frame)
				MakeLog();
		}

		protected void FixedUpdate() {
			if(logMode == LogMode.Physics)
				MakeLog();
		}
		#endregion

		#region Internal
		private float lastTimestamp;

		private void MakeLog() {
			float now = Time.timeSinceLevelLoad;
			float interval = now - lastTimestamp;
			lastTimestamp = now;

			Log.LabelledLine("FPS", " ", $"{(int)(1.0f / interval)}");
		}
		#endregion
	}
}