using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nianyi.UnityPlayground
{
	public class ObjectSpawner : MonoBehaviour
	{
		public GameObject prefab;
		[Min(0)] public float interval = 1;
		[Min(0)] public int maxCount = 100;
		public bool randomDirection = false;
		public bool randomVelocity = false;
		[Min(0)] public float maxSpeed = 1;

		[NonSerialized] public List<GameObject> spawnObjects = new();
		public Action<GameObject, int> onSpawned;

		protected void Start()
		{
			if(prefab)
				StartCoroutine(nameof(SpawningCoroutine));
		}

		IEnumerator SpawningCoroutine()
		{
			for(; ; )
			{
				yield return new WaitForSeconds(interval);
				SpawnOne();
				if(maxCount != 0 && spawnObjects.Count >= maxCount)
					break;
			}
		}

		void SpawnOne()
		{
			var go = Instantiate(prefab, transform);
			spawnObjects.Add(go);
			go.transform.SetParent(transform.parent, true);

			var rb = go.GetComponent<Rigidbody>();

			if(randomDirection)
			{
				Quaternion rot = UnityEngine.Random.rotationUniform;
				if(rb)
					rb.MoveRotation(rot);
				else
					go.transform.rotation = rot;
			}
			if(rb && randomVelocity)
				rb.velocity = UnityEngine.Random.insideUnitSphere * maxSpeed;

			onSpawned?.Invoke(go, spawnObjects.Count);
		}
	}
}
