using UnityEngine;

namespace Nianyi.UnityPlayground.WaterSimulation {
	public class WaterSimulationExperimentRigidbody : MonoBehaviour {
		#region Life cycle
		protected void OnEnterWater(Water water) {
			Debug.Log($"{name} entered {water}.", this);
		}

		protected void OnExitWater(Water water) {
			Debug.Log($"{name} exited {water}.", this);
		}
		#endregion
	}
}
