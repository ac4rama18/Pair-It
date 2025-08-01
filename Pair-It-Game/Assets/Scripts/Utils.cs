
using System.Collections.Generic;

namespace PairIt
{
	public static class Utils
	{
		public static void Shuffle<T>(this IList<T> lst)
		{
			var count = lst.Count;
			var last = count - 1;
			for (var i = 0; i < last; ++i)
			{
				var randIndex = UnityEngine.Random.Range(i, count);
				var temp = lst[i];
				lst[i] = lst[randIndex];
				lst[randIndex] = temp;
			}
		}

	}
}