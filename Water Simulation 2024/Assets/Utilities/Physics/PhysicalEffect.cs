using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace WaterSimulation {
	public interface PhysicalEffect {
		public Rigidbody Body { get; }
		public void Apply();

		public UnifiedPhysicalEffect Unify();
	}

	public partial struct ForceAtPosition : PhysicalEffect {
		private readonly Rigidbody body;
		public readonly Rigidbody Body => body;

		public Vector3 force;
		public Vector3 position;

		public ForceAtPosition(Rigidbody body) {
			this.body = body;
			force = default;
			position = default;
		}

		public readonly void Apply() {
			if(body == null)
				return;

			body.AddForceAtPosition(force, position, ForceMode.Force);
		}

		public readonly ForceAtPosition Scale(float scale) {
			return new ForceAtPosition(body) {
				force = force * scale,
				position = position,
			};
		}

		public readonly UnifiedPhysicalEffect Unify() {
			var arm = position - body.GetWorldCenterOfMass();
			return new(body) {
				force = force,
				torque = Vector3.Cross(arm, force),
			};
		}
	}

	public partial struct UnifiedPhysicalEffect : PhysicalEffect {
		private readonly Rigidbody body;
		public readonly Rigidbody Body => body;

		public Vector3 force;
		public Vector3 torque;

		public UnifiedPhysicalEffect(Rigidbody body) {
			this.body = body;
			force = default;
			torque = default;
		}

		public readonly void Apply() {
			if(body == null)
				return;

			body.AddForce(force, ForceMode.Force);
			body.AddTorque(torque, ForceMode.Force);
		}

		public readonly UnifiedPhysicalEffect Scale(float scale) {
			return new UnifiedPhysicalEffect(body) {
				force = force * scale,
				torque = torque * scale,
			};
		}

		public readonly UnifiedPhysicalEffect Unify() => this;

		public static UnifiedPhysicalEffect Combine(params UnifiedPhysicalEffect[] effects) {
			if(effects.Length == 0)
				return default;

			Vector3 force = default, torque = default;
			foreach(var effect in effects) {
				force += effect.force;
				torque += effect.torque;
			}

			return new(effects[0].Body) {
				force = force,
				torque = torque,
			};
		}
		public static UnifiedPhysicalEffect Combine<T>(params T[] effects) where T : PhysicalEffect {
			return Combine(effects.Select(a => a.Unify()).ToArray());
		}
		public static UnifiedPhysicalEffect Combine<T>(IEnumerable<T> effects) where T : PhysicalEffect {
			return Combine(effects.Select(a => a.Unify()).ToArray());
		}
	}
}
