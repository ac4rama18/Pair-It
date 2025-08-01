using UnityEngine;
namespace PairIt
{
	public class AudioSFXPlayer : MonoBehaviour
	{
		[SerializeField] private AudioSource m_AudioSource;
		[SerializeField] private AudioClip m_CardFlip;
		[SerializeField] private AudioClip m_PairMatch;
		[SerializeField] private AudioClip m_MisMatch;
		[SerializeField] private AudioClip m_GameOver;

		public static AudioSFXPlayer Instance { get; private set; }
		public void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Debug.LogWarning("AudioSFXPlayer : Multiple Instance initiated...");
			}
		}

		public void PlayCardFlip()
		{
			m_AudioSource.PlayOneShot(m_CardFlip, 0.7f); // Play with 70% volume
		}
		public void PlayCardPairMatch()
		{
			m_AudioSource.PlayOneShot(m_PairMatch, 0.8f); // Play with 70% volume
		}
		public void PlayMisMatch()
		{
			m_AudioSource.PlayOneShot(m_MisMatch, 0.7f); // Play with 70% volume
		}
		public void PlayGameOver()
		{
			m_AudioSource.PlayOneShot(m_GameOver, 0.9f); // Play with 70% volume
		}
	}
}