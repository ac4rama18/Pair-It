
using UnityEngine;

namespace PairIt
{
	public delegate void OnScoreUpdated(int score, int value);
	public class ScoreManager : MonoBehaviour
	{
		public static ScoreManager Instance { get; private set; }

		private int m_Score;

		public OnScoreUpdated onScoreUpdated;

		public int Score { get { return m_Score; } }

		public void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Debug.LogWarning("ScoreManager : Multiple Instance initiated...");
			}
		}

		public void Update()
		{

		}

		public void AddScore(int value)
		{
			if (value > 0)
			{
				m_Score += value;
				onScoreUpdated?.Invoke(m_Score, value);
			}
		}
	}
}