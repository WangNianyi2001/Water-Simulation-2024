using UnityEngine;

namespace WaterSimulation
{
	public class Pontoon : MonoBehaviour
	{
		[Min(0)] public float radius;

		protected void OnDrawGizmos()
		{
			Color gColor = Color.yellow;
			gColor.a = 0.6f;
			Gizmos.color = gColor;

			Gizmos.DrawSphere(transform.position, radius);
		}
	}
}
