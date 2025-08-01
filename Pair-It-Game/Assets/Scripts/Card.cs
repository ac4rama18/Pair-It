using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PairIt
{

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

                StartCoroutine(FlipCard(m_CardState));

                OnCardFlipped?.Invoke(this);
            }
        }
        public void SetCardId(int cardId)
        {
            m_CardId = cardId;
        }
        public IEnumerator FlipCard(CardState state)
        {
            RectTransform cardTransForm = this.GetComponent<RectTransform>();
            Quaternion initialRotation = cardTransForm.rotation;


            Sequence rotateSequence = DOTween.Sequence();

            rotateSequence.Append(cardTransForm.DORotate(new Vector3(0, 90, 0), 0.4f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic));
            rotateSequence.Append(DOVirtual.DelayedCall(0.1f, () =>
            {
                m_CardBackImage.gameObject.SetActive(state == CardState.Closed);
                m_CardImage.gameObject.SetActive(state == CardState.Open);
            }));
            rotateSequence.Append(cardTransForm.DORotate(initialRotation.eulerAngles, 0.1f, RotateMode.FastBeyond360).SetEase(Ease.InCubic));

            rotateSequence.Play();

            yield return new WaitForSeconds(1.0f);
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
                yield return FlipCard(m_CardState);
            }
            yield return new WaitForSeconds(Constants.kCardRevealTime);

            if (m_CardState == CardState.Open)
            {
                m_CardState = CardState.Closed;
                yield return FlipCard(m_CardState);
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

                StartCoroutine(FlipCard(m_CardState));
            }
        }
    }
}