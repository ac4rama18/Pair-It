using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PairIt
{

	public class UIController : MonoBehaviour
	{
		[SerializeField] private Button m_BackButton;
		[SerializeField] private TextMeshProUGUI m_ScoreText;

		void Awake()
		{
			m_BackButton.onClick.AddListener(BackButtonClicked);
			m_ScoreText.text = "000";
		}


		void Start()
		{
			ScoreManager.Instance.onScoreUpdated += OnScoreUpdatedInGame;
		}

		void OnDestroy()
		{
			m_BackButton.onClick.RemoveListener(BackButtonClicked);
			ScoreManager.Instance.onScoreUpdated -= OnScoreUpdatedInGame;
		}
		private void BackButtonClicked()
		{
			SceneManager.LoadScene("MenuScene");
		}
		private void OnScoreUpdatedInGame(int score, int value)
		{
			m_ScoreText.text = string.Format("{0:000}", score);

			Sequence seq = DOTween.Sequence();

			seq.Append(m_ScoreText.transform.DOScale(1.2f, 0.3f));
			seq.Append(m_ScoreText.transform.DOScale(1f, 0.3f));

			seq.Play();
		}

	}
}