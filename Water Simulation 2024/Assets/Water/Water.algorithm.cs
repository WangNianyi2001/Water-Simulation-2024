using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace WaterSimulation {
	public partial class Water : MonoBehaviour {
		/// <returns>Depth to the surface, downward is negative.</returns>
		protected float GetDepthAt(Vector3 position) {
			return position.y - transform.position.y;
		}
		protected UnifiedPhysicalEffect CalculateBuoyancy(RigidbodyInfo info, IList<PhysicsUtility.SurfaceSample> samples) {
			if(samples == null)
				return default;

			List<ForceAtPosition> forces = new(samples.Count);
			float totalWeight = samples.Aggregate(0f, (sum, sample) => sum + sample.weight);

			float g = Physics.gravity.magnitude;
			foreach(var sample in samples) {
				float depth = GetDepthAt(sample.position);
				if(depth > 0f)
					continue;

				totalWeight += sample.weight;

				ForceAtPosition force = new(info.body) {
					force = sample.normal * (-1f * profile.density * g * -depth * sample.weight),
					position = sample.position,
				};
				forces.Add(force);
			}

			var totalForce = UnifiedPhysicalEffect.Combine(forces);
			totalForce.force = Vector3.Project(totalForce.force, Physics.gravity);
			return totalForce.Scale(info.surfaceArea / totalWeight);
		}

		protected UnifiedPhysicalEffect CalculateDrag(RigidbodyInfo info, IList<PhysicsUtility.SurfaceSample> samples) {
				if(samples == null)
					return default;

				List<ForceAtPosition> forces = new(samples.Count);
				float totalArea = samples.Aggregate(0f, (sum, sample) => sum + sample.weight);

				var centerOfMass = info.body.GetWorldCenterOfMass();
				foreach(var sample in samples) {
					float depth = GetDepthAt(sample.position);
					if(depth > 0f)
						continue;

					Vector3 sampleVelocity = info.body.velocity;
					// Rotational contribution.
					sampleVelocity += Vector3.Cross(info.body.angularVelocity, sample.position - centerOfMass);

					float dot = Vector3.Dot(sampleVelocity, sample.normal);
					// Not moving towards water.
					if(dot < 0f)
						continue;

					var force = new ForceAtPosition(info.body) {
						force = sample.normal * (-1f * dot * profile.form * sample.weight),
						position = sample.position,
					};
					forces.Add(force);
				}

				return UnifiedPhysicalEffect.Combine(forces).Scale(info.surfaceArea / totalArea);
		}

		protected UnifiedPhysicalEffect CalculateDissipation(RigidbodyInfo info, IList<PhysicsUtility.SurfaceSample> samples) {
			float sinkingRatio = 1;
			if(samples != null && samples.Count != 0)
				sinkingRatio = (float)samples.Count(sample => GetDepthAt(sample.position) < 0f) / samples.Count;
			float dissipationCoefficient = profile.dissipation * sinkingRatio;
			return new(info.body) {
				force = info.body.velocity * (-dissipationCoefficient),
				torque = info.body.angularVelocity * (-dissipationCoefficient),
			};
		}
	}
}