using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace PairIt
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup m_GridUI;
        [SerializeField] private GameObject m_CellPrefab;
        [SerializeField] private GameObject m_GameOverPanel;


        private List<Card> m_SelectedCards;
        private List<Tuple<Card, Card>> m_MatchedCards;


        void Awake()
        {
            int cnt = m_GridUI.transform.childCount;
            for (int i = 0; i < cnt; i++)
            {
                GameObject.Destroy(m_GridUI.transform.GetChild(i).gameObject);
            }
            m_SelectedCards = new List<Card>();
            m_MatchedCards = new List<Tuple<Card, Card>>();

            m_GameOverPanel.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            /* CreateCell(0);
             CreateCell(6);
             CreateCell(4);
             CreateCell(4);
             CreateCell(0);
             CreateCell(6);*/
        }

        public void SetRowCountForGrid(int rowCount)
        {
            m_GridUI.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            m_GridUI.constraintCount = rowCount;
            RectTransform gridTrans = m_GridUI.GetComponent<RectTransform>();
            float gridHeight = gridTrans.rect.height;
            float gridWidth = gridTrans.rect.width;
            Debug.Log("" + gridWidth + " " + gridHeight);


            float calculatedCellHeight = Mathf.FloorToInt((gridHeight - (m_GridUI.spacing.y * (rowCount + 1))) / rowCount);

            float cellPreferredHeight = m_CellPrefab.GetComponent<LayoutElement>().preferredHeight;
            float cellPreferredWidth = m_CellPrefab.GetComponent<LayoutElement>().preferredWidth;
            float calculatedCellWidth = Mathf.FloorToInt((cellPreferredWidth / cellPreferredHeight) * calculatedCellHeight);

            Debug.Log("" + calculatedCellWidth + " " + calculatedCellHeight);

            m_GridUI.cellSize = new Vector2(calculatedCellWidth, calculatedCellHeight);
        }
        public void CreateCellsForCardPairs(List<int> cardIdPairs)
        {
            if (cardIdPairs != null)
            {
                for (int i = 0; i < cardIdPairs.Count; i++)
                {
                    CreateCell(cardIdPairs[i]);
                }
            }
        }
        // Update is called once per frame
        void Update()
        {
            float currentTime = Time.time;
            List<Card> closedCards = new List<Card>();
            for (int i = 0; i < m_SelectedCards.Count; i++)
            {
                Card selectedCard = m_SelectedCards[i];
                if (selectedCard.IsSelected() && !selectedCard.IsMatched)
                {
                    if (currentTime >= selectedCard.LastFlipTime + Constants.kCardRevealTime)
                    {
                        selectedCard.CloseCard();
                        AudioSFXPlayer.Instance.PlayMisMatch();

                        closedCards.Add(selectedCard);
                    }
                }
            }
            for (int i = 0; i < closedCards.Count; i++)
            {
                m_SelectedCards.Remove(closedCards[i]);
            }

            for (int i = 0; i < m_MatchedCards.Count; i++)
            {
                var item = m_MatchedCards[i];
                if (!item.Item1.IsFlipping && !item.Item2.IsFlipping)
                {
                    item.Item1.HighlightCard();
                    item.Item2.HighlightCard();
                    m_MatchedCards.Remove(item);
                    AudioSFXPlayer.Instance.PlayCardPairMatch();
                }
            }
        }

        private void CreateCell(int id)
        {
            GameObject cell = GameObject.Instantiate(m_CellPrefab);
            cell.name = $"Cell_{id}";
            cell.gameObject.transform.SetParent(m_GridUI.transform, false);
            Card card = cell.GetComponent<Card>();
            card.SetCardId(id);
            card.OnCardFlipped += OnCardFlipped;
        }

        private void OnCardFlipped(Card card)
        {
            // Debug.Log($"Card {card.CardId} flipped to {card.CardState}");
            AudioSFXPlayer.Instance.PlayCardFlip();

            Card matchedCard = null;
            for (int i = 0; i < m_SelectedCards.Count; i++)
            {
                if (m_SelectedCards[i].CardId == card.CardId)
                {
                    matchedCard = m_SelectedCards[i];

                    matchedCard.SetMatched();
                    card.SetMatched();

                    m_MatchedCards.Add(new Tuple<Card, Card>(card, matchedCard));

                    CardManager.Instance.OnCardMatch(matchedCard.CardId);
                    break;
                }
            }
            if (matchedCard != null)
            {
                m_SelectedCards.Remove(matchedCard);
            }
            else
            {
                m_SelectedCards.Add(card);
            }
        }

        public void ShowGameOver()
        {
            m_GameOverPanel.SetActive(true);
        }
    }
}