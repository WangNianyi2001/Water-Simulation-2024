using UnityEngine;

namespace Nianyi.UnityPlayground.WaterSimulation {
	public class WaterSimulationExperimentRigidbody : MonoBehaviour {
		#region Life cycle
		protected void Start() {
			Initialize();
		}

		protected void FixedUpdate() {
			float t = Time.timeSinceLevelLoad;
			if(t >= 5.0f)
				UnityEditor.EditorApplication.isPaused = true;

			var rot = transform.rotation.eulerAngles;
			Debug.Log(string.Join(" ", new string[] {
				$"LOGLINE", // mark
				$"{t}", // t
				$"{body.worldCenterOfMass.y}", // center-of-mass Y
				$"{GetLowestY()}", // lowest Y
				$"{rot.x}", // rotation X
				$"{rot.y}", // rotation Y
				$"{rot.z}", // rotation Z
			}));
		}
		#endregion

		#region Fields
		private new Collider collider;
		private Rigidbody body;
		#endregion

		#region Functions
		private void Initialize() {
			collider = GetComponent<Collider>();
			body = GetComponent<Rigidbody>();
		}

		private float GetLowestY() {
			return collider.bounds.min.y;
		}
		#endregion
	}
}
