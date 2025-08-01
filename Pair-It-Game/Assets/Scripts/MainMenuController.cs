
using System;
using DG.Tweening;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PairIt
{

	public class MainMenuController : MonoBehaviour
	{
		[SerializeField] private Button m_PlayButton;

		void Awake()
		{
			m_PlayButton.onClick.AddListener(PlayButtonClicked);
		}

		void Start()
		{
			m_PlayButton.transform.DOShakeScale(10.0f, 0.1f, 3);
		}

		void OnDestroy()
		{
			m_PlayButton.onClick.RemoveListener(PlayButtonClicked);
		}
		private void PlayButtonClicked()
		{
			SceneManager.LoadScene("GameScene");
		}
	}
}