using UnityEngine;

namespace Nianyi.UnityPlayground {
	public class TimedPauser : MonoBehaviour {
		#region Serialized fields
		[Min(0)] public float pauseTime = 5.0f;
		#endregion

		#region Fields
		private bool paused = false;
		#endregion

		#region Life cycle
		protected void FixedUpdate() {
			if(!paused) {
				if(Time.timeSinceLevelLoad >= pauseTime) {
					UnityEditor.EditorApplication.isPaused = true;
					paused = true;
				}
			}
		}
		#endregion
	}
}