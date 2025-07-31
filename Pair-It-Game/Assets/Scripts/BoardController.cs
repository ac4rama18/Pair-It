using System;
using System.Collections;
using System.Collections.Generic;
using PairIt;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{

    private const int kRevealTime = 2;
    [SerializeField] private GridLayoutGroup m_GridUI;
    [SerializeField] private GameObject m_CellPrefab;


    private List<Card> m_SelectedCards;
    void Awake()
    {
        int cnt = m_GridUI.transform.childCount;
        for (int i = 0; i < cnt; i++)
        {
            GameObject.Destroy(m_GridUI.transform.GetChild(i).gameObject);
        }
        m_SelectedCards = new List<Card>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateCell(0);
        CreateCell(6);
        CreateCell(4);
        CreateCell(4);
        CreateCell(0);
        CreateCell(6);
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
                if (currentTime >= selectedCard.LastFlipTime + kRevealTime)
                {
                    selectedCard.CloseCard();
                    closedCards.Add(selectedCard);
                }
            }
        }
        for (int i = 0; i < closedCards.Count; i++)
        {
            m_SelectedCards.Remove(closedCards[i]);
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
        Debug.Log($"Card {card.CardId} flipped to {card.CardState}");

        Card matchedCard = null;
        for (int i = 0; i < m_SelectedCards.Count; i++)
        {
            if (m_SelectedCards[i].CardId == card.CardId)
            {
                m_SelectedCards[i].SetMatched();
                matchedCard = m_SelectedCards[i];
                card.SetMatched();
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
}
