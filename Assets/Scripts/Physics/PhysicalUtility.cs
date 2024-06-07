using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nianyi.UnityPlayground {
	public static class PhysicsUtility {
		#region Collider
		public struct SurfaceSample {
			public Vector3 position;
			public Vector3 normal;
			public float area;
		}
		public static IEnumerable<SurfaceSample> SampleSurface(this Collider collider, int count) {
			switch(collider) {
				case BoxCollider box: {
						var boxWorldSize = box.GetWorldSize();
						for(int i = 0; i < count; ++i) {
							int faceChose = Random.Range(0, 6);
							Vector3 localPos = default, localNorm = default;
							float area = 0f;
							switch(faceChose % 3) {
								case 0:
									localPos = new Vector3(1f, Random.value, Random.value);
									localNorm = Vector3.right;
									area = boxWorldSize.y * boxWorldSize.z;
									break;
								case 1:
									localPos = new Vector3(Random.value, 1f, Random.value);
									localNorm = Vector3.up;
									area = boxWorldSize.z * boxWorldSize.x;
									break;
								case 2:
									localPos = new Vector3(Random.value, Random.value, 1f);
									localNorm = Vector3.forward;
									area = boxWorldSize.x * boxWorldSize.y;
									break;
							}
							localPos -= Vector3.one * .5f;
							if(faceChose % 2 == 1) {
								localPos *= -1;
								localNorm *= -1;
							}
							localPos = Vector3.Scale(localPos, box.size);
							localPos += box.center;
							var t = box.transform.localToWorldMatrix;
							var position = t.MultiplyPoint(localPos);
							SurfaceSample sample = new() {
								position = position,
								normal = t.MultiplyVector(localNorm).normalized,
								area = area,
							};
							yield return sample;
						}
						break;
					}
				case MeshCollider meshCollider: {
						var mesh = meshCollider.sharedMesh;
						if(mesh == null)
							yield break;
						var vp = mesh.vertices;
						var vi = mesh.triangles;
						int triangleCount = vi.Length / 3;
						for(int si = 0; si < count; ++si) {
							int i = Random.Range(0, triangleCount) * 3;
							yield return SampleTriangle(vp[vi[i]], vp[vi[i + 1]], vp[vi[i + 2]]);
						}
						break;
					}
				default:
					throw new System.NotImplementedException();
			}
		}
		public static IEnumerable<SurfaceSample> SampleSurface(IEnumerable<Collider> colliders, int count)
			=> colliders.Select(collider => collider.SampleSurface(count)).SelectMany(x => x);

		public static float CalculateSurfaceArea(this Collider collider) {
			switch(collider) {
				case BoxCollider boxCollider: {
						var boxTransform = boxCollider.transform.localToWorldMatrix * Matrix4x4.Scale(boxCollider.size);
						Vector3
							i = boxTransform * Vector3.right,
							j = boxTransform * Vector3.up,
							k = boxTransform * Vector3.forward;
						return (
							Vector3.Cross(i, j).magnitude +
							Vector3.Cross(j, k).magnitude +
							Vector3.Cross(k, i).magnitude
						) * 2f;
					}
				case SphereCollider sphereCollider: {
						return 4f / 3f * Mathf.PI * sphereCollider.radius * sphereCollider.transform.lossyScale.magnitude;
					}
				case MeshCollider meshCollider: {
						var mesh = meshCollider.sharedMesh;
						if(mesh == null)
							return default;
						float sumArea = 0.0f;
						var vp = mesh.vertices;
						var vi = mesh.triangles;
						for(int i = 0; i < vi.Length; i += 3) {
							sumArea += CalculateTriangleArea(vp[vi[i]], vp[vi[i + 1]], vp[vi[i + 2]]);
						}
						return sumArea;
					}
				default: {
						Debug.LogError($"Cannot calculate surface area for {collider.GetType().Name}.", collider);
						throw new System.NotImplementedException();
					}
			}
		}

		private static float CalculateTriangleArea(in Vector3 a, in Vector3 b, in Vector3 c) {
			return Vector3.Cross(b - a, c - a).magnitude * .5f;
		}

		private static SurfaceSample SampleTriangle(in Vector3 a, in Vector3 b, in Vector3 c) {
			Vector3 i = b - a, j = c - a;

			// Randomly sample in triangle.
			// https://stackoverflow.com/a/68493226/15186859
			float s = Random.value, t = Random.value;
			bool inTriangle = s + t <= 1;
			Vector3 position = inTriangle
				? s * i + t * j
				: (1 - s) * i + (1 - t) * j;

			Vector3 cross = Vector3.Cross(i, j);
			return new() {
				position = position,
				normal = cross.normalized,
				area = cross.magnitude * .5f,
			};
		}
		#endregion

		#region Box collider
		public static Vector3 GetWorldSize(this BoxCollider box) {
			return Vector3.Scale(box.size, box.transform.lossyScale);
		}
		#endregion

		#region Rigidbody
		public static Vector3 GetWorldCenterOfMass(this Rigidbody body) {
			return body.transform.localToWorldMatrix.MultiplyPoint(body.centerOfMass);
		}

		public static float GetVolume(this Rigidbody body) {
			var previousMass = body.mass;
			body.SetDensity(1.0f);
			var newMass = body.mass;
			float previousDensity = previousMass / newMass;
			body.SetDensity(previousDensity);
			return previousMass / previousDensity;
		}
		#endregion
	}
}