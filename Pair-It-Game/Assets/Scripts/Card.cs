using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PairIt
{

    public enum CardState
    {
        Closed = 0,
        Open = 1
    }
    public delegate void CardStateCallback(Card card);

    public class Card : MonoBehaviour
    {
        [SerializeField] private CardState m_CardState;
        [SerializeField] private Button m_CardButton;
        [SerializeField] private Image m_CardImage;
        [SerializeField] private Image m_CardBackImage;

        [SerializeField] private int m_CardId;

        public CardStateCallback OnCardFlipped;

        private bool m_IsMatched = false;

        private float m_LastFlipTime;
        public int CardId { get { return m_CardId; } }
        public CardState CardState { get { return m_CardState; } }
        public bool IsMatched { get { return m_IsMatched; } }
        public float LastFlipTime { get { return m_LastFlipTime; } }

        private void Awake()
        {
            m_CardImage.gameObject.SetActive(false);
            m_CardBackImage.gameObject.SetActive(true);
            m_CardState = CardState.Closed;
            m_LastFlipTime = 0;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_CardButton.onClick.AddListener(OnCardButtonClick);

            string spritePath = string.Format("Cards/Card_{0:00}", m_CardId);
            m_CardImage.sprite = Resources.Load<Sprite>(spritePath);

            StartCoroutine(RevealCard());
        }

        void OnDestroy()
        {
            m_CardButton.onClick.RemoveListener(OnCardButtonClick);
        }
        // Update is called once per frame
        void Update()
        {
        }

        private void OnCardButtonClick()
        {
            if (m_CardState == CardState.Closed)
            {
                m_CardState = CardState.Open;
                m_LastFlipTime = Time.time;
                m_CardButton.enabled = false;

                FlipCard(m_CardState);

                OnCardFlipped?.Invoke(this);
            }
        }
        public void SetCardId(int cardId)
        {
            m_CardId = cardId;
        }
        public void FlipCard(CardState state)
        {
            m_CardBackImage.gameObject.SetActive(state == CardState.Closed);
            m_CardImage.gameObject.SetActive(state == CardState.Open);
        }

        public bool IsSelected()
        {
            return m_CardState == CardState.Open;
        }
        public void SetMatched()
        {
            m_IsMatched = true;
            m_LastFlipTime = 0;
        }
        public IEnumerator RevealCard()
        {
            if (m_CardState == CardState.Closed)
            {
                m_CardState = CardState.Open;
                FlipCard(m_CardState);
            }
            yield return new WaitForSeconds(2);

            if (m_CardState == CardState.Open)
            {
                m_CardState = CardState.Closed;
                FlipCard(m_CardState);
            }

            yield return null;
        }
        public void CloseCard()
        {
            if (m_CardState == CardState.Open)
            {
                m_CardState = CardState.Closed;
                m_LastFlipTime = 0;
                m_CardButton.enabled = true;

                FlipCard(m_CardState);
            }
        }
    }
}