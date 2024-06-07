using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Nianyi.UnityPlayground.WaterSimulation {
	public class RandomizedObjectSpawner : MonoBehaviour {
		#region Serialized fields
		public List<GameObject> prefabs = new();
		[Min(0)] public float spawnInterval = 0.5f;
		[Min(0)] public int maxCount = (int)1e3;
		[Min(0)] public float maxVelocity = 1.0f;
		[Min(0)] public float maxAngularVelocity = 6.28f;
		public bool makeLogs = false;
		#endregion

		#region Life cycle
		protected void Start() {
			StartCoroutine(SpawningCoroutine());
		}

		protected IEnumerator SpawningCoroutine() {
			while(maxCount == 0 || spawnedObjects.Count < maxCount) {
				Spawn();
				yield return new WaitForSeconds(spawnInterval);
			}
		}

		protected void Update() {
			if(makeLogs) {
				Log.LabelledLine("ObjectSpawner", " ", $"{Time.timeSinceLevelLoad}", $"{spawnedObjects.Count}",  $"{(int)(1.0f / Time.smoothDeltaTime)}");
			}
		}
		#endregion

		#region Fields
		private readonly List<GameObject> spawnedObjects = new();
		#endregion

		#region Functions
		private GameObject Spawn() {
			GameObject prefab = prefabs.Count == 0 ? null : prefabs[Random.Range(0, prefabs.Count)];
			if(prefab == null)
				return null;

			GameObject spawned = Instantiate(prefab, transform.position, transform.rotation);
			spawnedObjects.Add(spawned);

			if(spawned.TryGetComponent(out Rigidbody rb)) {
				rb.velocity = Random.insideUnitSphere * maxVelocity;
				rb.angularVelocity = Random.insideUnitSphere * maxAngularVelocity;
			}

			return spawned;
		}
		#endregion
	}
}