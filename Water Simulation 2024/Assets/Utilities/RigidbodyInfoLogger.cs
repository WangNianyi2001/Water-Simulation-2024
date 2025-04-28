using UnityEngine;

namespace Nianyi.UnityPlayground {
	public class RigidbodyInfoLogger : MonoBehaviour {
		#region Serialized fields
		[Min(0)] public float pauseTime = 5.0f;
		#endregion

		#region Fields
		private new Collider collider;
		private Rigidbody body;
		#endregion

		#region Life cycle
		protected void Start() {
			collider = GetComponent<Collider>();
			body = GetComponent<Rigidbody>();
		}

		protected void FixedUpdate() {
			var rot = transform.rotation.eulerAngles;
			Log.LabelledLine($"Rigidbody @{this}: ", " ", new string[] {
				$"{Time.timeSinceLevelLoad}", // t
				$"{body.worldCenterOfMass.y}", // center-of-mass Y
				$"{collider.bounds.min.y}", // lowest Y
				$"{rot.x}", // rotation X
				$"{rot.y}", // rotation Y
				$"{rot.z}", // rotation Z
			});
		}
		#endregion
	}
}
