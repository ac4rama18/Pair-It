using UnityEngine;

namespace PairIt
{

	public class CardManager : MonoBehaviour
	{
		public static CardManager Instance { get; private set; }
		public void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Debug.LogWarning("CardManager : Multiple Instance initiated...");
			}
		}

		public void Update()
		{

		}

		public void GeneratePairs(int value)
		{
			if (value > 1)
			{

			}
		}
	}
}