using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PairIt
{
	[System.Serializable]
	public class GameData
	{
		public int m_MaxCardPairs;
		public int m_Rows;

		public List<int> m_GeneratedCards;
	}


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


			if (PlayerPrefs.HasKey("game_data"))
			{
				string gameDataString = PlayerPrefs.GetString("game_data");
				GameData gameData = JsonUtility.FromJson<GameData>(gameDataString);

				m_GeneratedPairCount = gameData.m_MaxCardPairs;

				m_BoardController.SetRowCountForGrid(gameData.m_Rows);
				m_BoardController.CreateCellsForCardPairs(gameData.m_GeneratedCards);
			}
			else
			{
				GameData gameData = new GameData();

				gameData.m_MaxCardPairs = 6; // should be between 1 to 12 since we have only 12 unique cards
				gameData.m_Rows = 2; // can be in between 2 to 5


				m_BoardController.SetRowCountForGrid(gameData.m_Rows);

				m_GeneratedPairCount = gameData.m_MaxCardPairs;

				gameData.m_GeneratedCards = GeneratePairs(gameData.m_MaxCardPairs);
				m_BoardController.CreateCellsForCardPairs(gameData.m_GeneratedCards);

				string gameDataString = JsonUtility.ToJson(gameData);
				PlayerPrefs.SetString("game_data", gameDataString);
			}

		}

		public void Update()
		{

		}

		public List<int> GeneratePairs(int value)
		{
			List<int> cardPairsForPlay = new List<int>();
			if (value <= 0 || value > Constants.kMaxCards)
			{
				Debug.LogWarning("Out of cards...");
				return cardPairsForPlay;
			}

			if (value > 1)
			{
				List<int> cardIds = new List<int>();
				for (int i = 0; i <= Constants.kMaxCards; i++)
				{
					cardIds.Add(i);
				}

				List<int> selectedCardIds = new List<int>();
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
			}
			return cardPairsForPlay;
		}

		public void OnCardMatch(int cardId)
		{
			m_MatchedPairCount += 1;
			ScoreManager.Instance.AddScore(10);

			if (m_GeneratedPairCount == m_MatchedPairCount)
			{
				Debug.Log("Match Complete!");

				PlayerPrefs.DeleteKey("game_data");

				StartCoroutine(ShowGameOver());
			}
		}

		private IEnumerator ShowGameOver()
		{
			yield return new WaitForSeconds(2);
			AudioSFXPlayer.Instance.PlayGameOver();
			m_BoardController.ShowGameOver();
		}

	}
}