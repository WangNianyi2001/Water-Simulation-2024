using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace WaterSimulation
{
	public partial class Water : MonoBehaviour
	{
		#region Serialized Fields
		public WaterProfile profile;
		[Tooltip("Sample count in average per unit surface area per physical frame.")]
		[Min(0)] public int sampleDensity = 20;
		#endregion

		#region Fields
		public readonly Dictionary<Rigidbody, RigidbodyInfo> floatingBodies = new();
		#endregion

		#region Functions
		private void OnBodyEnter(Rigidbody body)
		{
			if(floatingBodies.ContainsKey(body))
				return;

			floatingBodies.Add(body, new(body));
			body.SendMessage("OnEnterWater", this, SendMessageOptions.DontRequireReceiver);
		}

		private void OnBodyExit(Rigidbody body)
		{
			if(!floatingBodies.ContainsKey(body))
				return;

			floatingBodies.Remove(body);
			body.SendMessage("OnExitWater", this, SendMessageOptions.DontRequireReceiver);
		}
		#endregion

		#region Life cycle
		protected void OnTriggerEnter(Collider other)
		{
			if(other == null)
				return;
			if(!other.TryGetComponent<Rigidbody>(out var body))
				return;
			OnBodyEnter(body);
		}

		protected void OnTriggerExit(Collider other)
		{
			if(other == null)
				return;
			if(!other.TryGetComponent<Rigidbody>(out var body))
				return;
			OnBodyExit(body);
		}

		protected void FixedUpdate()
		{
			foreach(var info in floatingBodies.Values)
			{
				if(info.body.isKinematic)
					continue;

				var samples = PhysicsUtility.SampleSurface(info.colliders, Mathf.CeilToInt(sampleDensity * info.surfaceArea)).ToArray();

				UnifiedPhysicalEffect.Combine(new PhysicalEffect[] {
					CalculateBuoyancy(info, samples),
					CalculateDrag(info, samples),
					CalculateDissipation(info, samples),
				}).Apply();
			}
		}
		#endregion
	}
}