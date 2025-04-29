using UnityEngine;

namespace WaterSimulation
{
	public class Pontoon : MonoBehaviour
	{
		[Min(0)] public float radius;

		protected void OnDrawGizmos()
		{
			Color gColor = Color.white;
			gColor.a = 0.3f;
			Gizmos.color = gColor;

			Gizmos.DrawSphere(transform.position, radius);
		}
	}
}
