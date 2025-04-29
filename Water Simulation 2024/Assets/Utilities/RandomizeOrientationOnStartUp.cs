using UnityEngine;

namespace WaterSimulation
{
	public class RandomizeOrientationOnStartUp : MonoBehaviour
	{
		public Quaternion Rotation {
			get => transform.rotation;
			set
			{
				if(TryGetComponent<Rigidbody>(out var rb))
					rb.rotation = value;
				else
					transform.rotation = value;
			}
		}

		protected void Start()
		{
			Rotation = Random.rotationUniform;
		}
	}
}
