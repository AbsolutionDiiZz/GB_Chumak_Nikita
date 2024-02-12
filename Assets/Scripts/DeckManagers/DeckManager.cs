using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class DeckManager : MonoBehaviour
{
    // Ссылки на другие менеджеры для взаимодействия с магазином и рукой игрока
    public StoreManager storeManager; 
    public HandManager HandManager; 
    public FoldDeck foldDeckScript;

    // Колоды для игрока и оппонента
    public List<Card> player0DrawDeck = new List<Card>(); 
    public List<Card> player0DiscardDeck = new List<Card>(); 
    public List<Card> player0PlayedHandDeck = new List<Card>(); 

    public List<Card> player1DrawDeck = new List<Card>(); 
    public List<Card> player1DiscardDeck = new List<Card>(); 
    public List<Card> player1PlayedHandDeck = new List<Card>();

    // UI элементы для отображения информации о колодах
    public TextMeshProUGUI player0DiscardDeckText;
    public TextMeshProUGUI player0DrawDeckText;
    public TextMeshProUGUI player0PlayedHandDeckText;

    public TextMeshProUGUI player1DiscardDeckText;
    public TextMeshProUGUI player1DrawDeckText;
    public TextMeshProUGUI player1PlayedHandDeckText;

    // Префабы карт для начальных колод игрока и оппонента
    public Card player0CardGoldCoinPrefab;
    public Card player0CardGod1Prefab;
    public Card player0CardGod2Prefab;

    public Card player1CardGoldCoinPrefab;
    public Card player1CardGod1Prefab;
    public Card player1CardGod2Prefab;

    public GameObject god1TemplePrefab;
    public GameObject god2TemplePrefab;
    public GameObject newGodsTemplePrefab;

    public static DeckManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePlayersDeck();
            ShuffleDeck(player0DrawDeck);
            ShuffleDeck(player1DrawDeck);
        }
    }
    private void Start()
    {
        SetupInitialStore();
        SetupInitialHands();
        UpdateDeckTexts();
    }
    // Инициализирует начальные колоды игроков и добавляет карты в магазин.
    private void SetupInitialStore() // Добавление первых четырех карт из общей колоды в магазин
    {
        for (int i = 0; i < 4; i++)
        {
            Card cardToAdd = foldDeckScript.GetLastCard(); 
            if (cardToAdd != null)
            {
                storeManager.AddCardToFirstEmptyStore(cardToAdd); 
            }
        }
    }
    private void SetupInitialHands() // Распределяет начальные карты в руки игроков.
    {
        for (int i = 0; i < 6 && player0DrawDeck.Count > 0; i++)
        {
            var card = GetLastCardFromPlayer0DrawDeck();
            HandManager.AddCardToFirstEmptyPlayer0Position(card);
        }
        for (int i = 0; i < 6 && player1DrawDeck.Count > 0; i++)
        {
            var card = GetLastCardFromPlayer1DrawDeck();
            HandManager.AddCardToFirstEmptyPlayer1Position(card);
        }
    }
    private void InitializePlayersDeck() // Инициализирует начальные колоды игроков с заданными префабами карт.
    {
        AddCardToPlayer0DrawDeck(player0CardGoldCoinPrefab, 10);
        AddCardToPlayer0DrawDeck(player0CardGod1Prefab, 1);
        AddCardToPlayer0DrawDeck(player0CardGod2Prefab, 1);
        AddCardToPlayer1DrawDeck(player1CardGoldCoinPrefab, 10);
        AddCardToPlayer1DrawDeck(player1CardGod1Prefab, 1);
        AddCardToPlayer1DrawDeck(player1CardGod2Prefab, 1);
    }
    // - AddCardToPlayer0DrawDeck и AddCardToPlayer1DrawDeck добавляют карты в колоду добора.
    private void AddCardToPlayer0DrawDeck(Card cardPrefab, int copies)
    {
        for (int i = 0; i < copies; i++)
        {
            var newCard = Instantiate(cardPrefab, transform);
            player0DrawDeck.Add(newCard);
            UpdateDeckTexts();
        }
    }
    private void AddCardToPlayer1DrawDeck(Card cardPrefab, int copies)
    {
        for (int i = 0; i < copies; i++)
        {
            var newCard = Instantiate(cardPrefab, transform);
            player1DrawDeck.Add(newCard);
            UpdateDeckTexts();
        }
    }  
    public void ShuffleDeck(List<Card> deck) // перемешивает указанную колоду.
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
        UpdateDeckTexts();
    }
    // - GetLastCardFromPlayer0DrawDeck и GetLastCardFromPlayer1DrawDeck извлекают последнюю карту из колоды добора.
    public Card GetLastCardFromPlayer0DrawDeck()
    {
        if (player0DrawDeck.Count > 0)
        {
            var lastCard = player0DrawDeck[player0DrawDeck.Count - 1];
            player0DrawDeck.RemoveAt(player0DrawDeck.Count - 1);
            return lastCard;
        }
        return null;
    }
    public Card GetLastCardFromPlayer1DrawDeck()
    {
        if (player1DrawDeck.Count > 0)
        {
            var lastCard = player1DrawDeck[player1DrawDeck.Count - 1];
            player1DrawDeck.RemoveAt(player1DrawDeck.Count - 1);
            return lastCard;
        }
        return null;
    }
    // - MovePlayer0DiscardToPlayer0DrawDeck и MovePlayer1DiscardToPlayer1DrawDeck перемещают карты из колоды сброса обратно в колоду добора.
    public void MovePlayer0DiscardToPlayer0DrawDeck()
    {
        foreach (var card in player0DiscardDeck)
        {
            player0DrawDeck.Add(card);
        }
        player0DiscardDeck.Clear();
        ShuffleDeck(player0DrawDeck);
        UpdateDeckTexts();
    }
    public void MovePlayer1DiscardToPlayer1DrawDeck()
    {
        foreach (var card in player1DiscardDeck)
        {
            player1DrawDeck.Add(card);
        }
        player1DiscardDeck.Clear();
        ShuffleDeck(player1DrawDeck);
        UpdateDeckTexts();
    }
    // - AddCardToPlayer0DiscardDeck и AddCardToPlayer1DiscardDeck добавляют карту в колоду сброса.
    public void AddCardToPlayer0DiscardDeck(Card card)
    {
        player0DiscardDeck.Add(card);
        HideCard(card.gameObject);
        UpdateDeckTexts();
    }
    public void AddCardToPlayer1DiscardDeck(Card card)
    {
        player1DiscardDeck.Add(card);
        HideCard(card.gameObject);
        UpdateDeckTexts();
    }
    // - AddCardToPlayer0PlayedHandDeck и AddCardToPlayer1PlayedHandDeck добавляют карту в руку игрока.
    public void AddCardToPlayer0PlayedHandDeck(Card card)
    {
        player0PlayedHandDeck.Add(card);
        HideCard(card.gameObject);
        UpdateDeckTexts();
    }
    public void AddCardToPlayer1PlayedHandDeck(Card card)
    {
        player1PlayedHandDeck.Add(card);
        HideCard(card.gameObject);
        UpdateDeckTexts();
    }

    private void HideCard(GameObject card)
    {
        card.transform.position = new Vector3(1000, 1000, 1000); 
    }
    // - MovePlayedCardsToPlayer0DiscardDeck и MovePlayedCardsToPlayer1DiscardDeck перемещают разыгранные карты в колоду сброса.
    public void MovePlayedCardsToPlayer0DiscardDeck()
    {
        foreach (var card in new List<Card>(player0PlayedHandDeck)) 
        {
            if (card.activationType != "Activation")
            {
                player0PlayedHandDeck.Remove(card);
                AddCardToPlayer0DiscardDeck(card);
            }
        }

        player0PlayedHandDeck.Clear();
        UpdateDeckTexts();
    }
    public void MovePlayedCardsToPlayer1DiscardDeck()
    {
        foreach (var card in new List<Card>(player1PlayedHandDeck))
        {
            if (card.activationType != "Activation")
            {
                player1PlayedHandDeck.Remove(card);
                AddCardToPlayer1DiscardDeck(card);
            }
        }

        player1PlayedHandDeck.Clear();
        UpdateDeckTexts();
    }
    public void UpdateDeckTexts() //обновляет UI тексты, отображающие состояние колод.
    {
        if (player0DiscardDeckText != null) player0DiscardDeckText.text = "Discard Deck: " + player0DiscardDeck.Count.ToString();
        if (player0DrawDeckText != null) player0DrawDeckText.text = "Draw Deck: " + player0DrawDeck.Count.ToString();
        if (player0PlayedHandDeckText != null) player0PlayedHandDeckText.text = "Played Deck: " + player0PlayedHandDeck.Count.ToString();
        if (player1DiscardDeckText != null) player1DiscardDeckText.text = "Discard Deck: " + player1DiscardDeck.Count.ToString();
        if (player1DrawDeckText != null) player1DrawDeckText.text = "Draw Deck: " + player1DrawDeck.Count.ToString();
        if (player1PlayedHandDeckText != null) player1PlayedHandDeckText.text = "Played Deck: " + player1PlayedHandDeck.Count.ToString();
    }
}