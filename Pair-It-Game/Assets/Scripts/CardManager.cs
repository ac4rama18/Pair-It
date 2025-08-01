using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PairIt
{

	public class CardManager : MonoBehaviour
	{

		[SerializeField] private BoardController m_BoardController;

		private int m_MatchedPairCount = 0;
		private int m_GeneratedPairCount = 0;
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
		void Start()
		{
			m_MatchedPairCount = 0;
			m_GeneratedPairCount = 4;
			GeneratePairs(m_GeneratedPairCount);
		}
		public void Update()
		{

		}

		public void GeneratePairs(int value)
		{
			if (value > 1)
			{
				List<int> cardIds = new List<int>();
				for (int i = 0; i <= Constants.kMaxCards; i++)
				{
					cardIds.Add(i);
				}

				List<int> selectedCardIds = new List<int>();
				List<int> cardPairsForPlay = new List<int>();
				while (selectedCardIds.Count < value)
				{
					int rendIndex = UnityEngine.Random.Range(0, cardIds.Count);
					int cardId = cardIds[rendIndex];
					if (!selectedCardIds.Contains(cardId))
					{
						selectedCardIds.Add(cardId);

						cardPairsForPlay.Add(cardId);
						cardPairsForPlay.Add(cardId);
					}
				}
				cardPairsForPlay.Shuffle();
				cardPairsForPlay.Shuffle();
				cardPairsForPlay.Shuffle();

				m_BoardController.CreateCellsForCardPairs(cardPairsForPlay);
			}
		}

		public void OnCardMatch(int cardId)
		{
			m_MatchedPairCount += 1;
			ScoreManager.Instance.AddScore(10);

			if (m_GeneratedPairCount == m_MatchedPairCount)
			{
				Debug.Log("Match Complete!");
				StartCoroutine(ShowGameOver());
			}
		}

		private IEnumerator ShowGameOver()
		{
			yield return new WaitForSeconds(2);
			m_BoardController.ShowGameOver();
}

	}
}