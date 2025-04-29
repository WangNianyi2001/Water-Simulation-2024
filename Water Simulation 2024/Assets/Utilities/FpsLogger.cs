using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace WaterSimulation
{
	public class FpsLogger : MonoBehaviour
	{
		public Water water;

		public int Count
		{
			get
			{
				if(water && water.isActiveAndEnabled)
					return water.floatingBodies.Count;
				return 0;
			}
		}

		readonly Dictionary<int, float> records = new();
		protected void Update()
		{
			int count = Count;
			float fps = 1f / Time.smoothDeltaTime;
			records[count] = fps;
		}

		protected void OnDestroy()
		{
			Debug.Log(string.Join("\n", records.Select(
				pair => $"{pair.Key},{pair.Value}"
			)));
		}
	}
}
