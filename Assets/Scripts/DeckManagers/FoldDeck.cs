using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class FoldDeck : MonoBehaviour // ��������� ������� ����, ������������ � ���� ��� ��������� �����, ����� ��� ����� ����.
{
    [SerializeField]
    private Card[] cardPrefabs; // ������� ����, �� ������� ����� ������������ ������.

    [SerializeField]
    private TextMeshProUGUI foldDeckText; // UI ������� ��� ����������� ���������� ���� � ������.

    public List<Card> deck = new List<Card>(); // ������ ���� � ������.

    void Start() // ������������� ������ ��� ������.
    {
        InitializeDeck(); // ��������� ������ �������.
        ShuffleDeckCards(); // ������������ ������.
        UpdateFoldDeckText(); // ��������� UI � ����������� ����.
    }
    private void AddCardCopiesToDeck(Card cardPrefab, int numberOfCopies)
    {
        for (int i = 0; i < numberOfCopies; i++)
        {
            deck.Add(Instantiate(cardPrefab, transform));
        }
    }
    private void InitializeDeck() // �������������� ������, �������� ���������� ���� �� ��������.
    {
        foreach (var cardPrefab in cardPrefabs)
        {
            AddCardCopiesToDeck(cardPrefab, 2); // ��������� ��� ����� ������ �����.
        }
    }
    private void ShuffleDeckCards() // ������������ ����� � ������ ��������� �������.
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int randomIndex = Random.Range(i, deck.Count);
            Card temp = deck[i];
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public Card GetLastCard() // ���������� ��������� ����� �� ������ � ������� �� �� ������.
    {
        if (deck.Count > 0)
        {
            Card lastCard = deck[deck.Count - 1];
            deck.RemoveAt(deck.Count - 1); // ������� ����� �� ������.
            UpdateFoldDeckText(); // ��������� UI.
            return lastCard;
        }
        return null;
    }
    private void UpdateFoldDeckText() // ��������� UI �����, ������������ ���������� ���� � ������.
    {
        foldDeckText.text = $"Fold Deck: {deck.Count}"; // ���������� ���������� ����.
    }
}