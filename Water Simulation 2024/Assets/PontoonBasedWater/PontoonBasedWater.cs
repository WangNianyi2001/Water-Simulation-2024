using UnityEngine;
using System.Collections.Generic;

namespace WaterSimulation
{
	public class PontoonBasedWater : MonoBehaviour
	{
		public readonly HashSet<Rigidbody> bodies = new();

		public WaterProfile profile;

		protected void FixedUpdate()
		{
			float waterHeight = transform.position.y;
			foreach(var rb in bodies)
			{
				if(!rb)
					continue;

				var pontoons = rb.GetComponentsInChildren<Pontoon>();
				if(pontoons.Length == 0)
					continue;

				foreach(var pontoon in pontoons)
				{
					Vector3 pos = pontoon.transform.position;
					float h = pos.y - waterHeight;
					float volume = GetSubmergedVolume(pontoon.radius, h);
					if(volume <= 0)
						continue;

					Vector3 buoyancyForce = Physics.gravity * -(volume * profile.density);

					rb.AddForceAtPosition(buoyancyForce, pos, ForceMode.Force);
				}

				rb.AddForce(-Square(rb.velocity) * profile.dissipation, ForceMode.Force);
				rb.AddTorque(-Square(rb.angularVelocity) * profile.dissipation, ForceMode.Force);
			}
		}

		static Vector3 Square(Vector3 v)
		{
			return v * v.magnitude;
		}

		const float TRI_PI = Mathf.PI / 3;
		static float GetSubmergedVolume(float radius, float h)
		{
			h = Mathf.Clamp(h / radius, -1, 1);
			return Mathf.Pow(radius, 3) * TRI_PI * (2 + Mathf.Pow(h, 3) - 3 * h);
		}

		protected void OnTriggerEnter(Collider other)
		{
			if(other == null)
				return;
			if(!other.transform.TryGetComponent<Rigidbody>(out var rb))
				return;
			bodies.Add(rb);
		}

		protected void OnTriggerExit(Collider other)
		{
			if(other == null)
				return;
			if(!other.TryGetComponent<Rigidbody>(out var rb))
				return;
			bodies.Remove(rb);
		}
	}
}