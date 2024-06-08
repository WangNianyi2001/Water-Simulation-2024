using UnityEngine;

namespace Nianyi.UnityPlayground.WaterSimulation {
	[CreateAssetMenu(menuName = "Nianyi/Water Profile")]
	public class WaterProfile : ScriptableObject {
		[Min(0)] public float density = 1.33f;
		[Min(0)] public float form = 0.1f;
		[Min(0)] public float viscosity = 0.1f;
		[Tooltip("Force energy loss.")]
		[Min(0)] public float dissipation = 1f;
	}
}
