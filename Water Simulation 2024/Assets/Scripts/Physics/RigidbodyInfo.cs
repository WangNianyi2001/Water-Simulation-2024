using UnityEngine;
using System.Linq;

namespace Nianyi.UnityPlayground {
	public class RigidbodyInfo {
		public readonly Rigidbody body;
		public readonly Collider[] colliders;
		public readonly float surfaceArea;
		public readonly float volume;

		public RigidbodyInfo(Rigidbody body) {
			this.body = body;
			colliders = body.GetComponents<Collider>();
			surfaceArea = colliders.Select(collider => collider.CalculateSurfaceArea()).Sum();
			volume = body.GetVolume();
		}
	}
}