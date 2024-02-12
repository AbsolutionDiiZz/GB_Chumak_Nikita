using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class FoldDeck : MonoBehaviour // Управляет колодой карт, используемой в игре для различных целей, таких как добор карт.
{
    [SerializeField]
    private Card[] cardPrefabs; // Префабы карт, из которых будет сформирована колода.

    [SerializeField]
    private TextMeshProUGUI foldDeckText; // UI элемент для отображения количества карт в колоде.

    public List<Card> deck = new List<Card>(); // Список карт в колоде.

    void Start() // Инициализация колоды при старте.
    {
        InitializeDeck(); // Заполняем колоду картами.
        ShuffleDeckCards(); // Перемешиваем колоду.
        UpdateFoldDeckText(); // Обновляем UI с количеством карт.
    }
    private void AddCardCopiesToDeck(Card cardPrefab, int numberOfCopies)
    {
        for (int i = 0; i < numberOfCopies; i++)
        {
            deck.Add(Instantiate(cardPrefab, transform));
        }
    }
    private void InitializeDeck() // Инициализирует колоду, создавая экземпляры карт из префабов.
    {
        foreach (var cardPrefab in cardPrefabs)
        {
            AddCardCopiesToDeck(cardPrefab, 2); // Добавляем две копии каждой карты.
        }
    }
    private void ShuffleDeckCards() // Перемешивает карты в колоде случайным образом.
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int randomIndex = Random.Range(i, deck.Count);
            Card temp = deck[i];
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public Card GetLastCard() // Возвращает последнюю карту из колоды и удаляет ее из колоды.
    {
        if (deck.Count > 0)
        {
            Card lastCard = deck[deck.Count - 1];
            deck.RemoveAt(deck.Count - 1); // Удаляем карту из колоды.
            UpdateFoldDeckText(); // Обновляем UI.
            return lastCard;
        }
        return null;
    }
    private void UpdateFoldDeckText() // Обновляет UI текст, отображающий количество карт в колоде.
    {
        foldDeckText.text = $"Fold Deck: {deck.Count}"; // Отображаем количество карт.
    }
}