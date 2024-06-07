using UnityEngine;
using System.Collections.Generic;

namespace Nianyi.UnityPlayground {
	public static class Log {
		public static void LabelledLine(string label, string separator, params string[] terms) {
			List<string> list = new() { label };
			list.AddRange(terms);
			Debug.Log(string.Join(separator, list));
		}
	}
}