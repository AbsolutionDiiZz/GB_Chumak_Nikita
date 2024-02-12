using UnityEngine;
using System.Collections.Generic;

public class HandManager : MonoBehaviour // ��������� ������������� ���� � ����� �������.
{
    [SerializeField] public Transform[] player0HandPositions; // ������� ��� ���� ���� ������� ������.
    [SerializeField] public Transform[] player1HandPositions; // ������� ��� ���� ���� ������� ������.

    private Dictionary<Transform, Card> player0HandCards = new Dictionary<Transform, Card>(); // ����� �� ����� ������� ������.
    private Dictionary<Transform, Card> player1HandCards = new Dictionary<Transform, Card>(); // ����� �� ����� ������� ������.

    private void Start()
    {
        InitializeHands(); // �������������� ��������� ��������� ��� �������.
    }

    private void InitializeHands() // �������������� ���� �������, ������������ ��� ������� ��� ���������.
    {
        foreach (var position in player0HandPositions)
            player0HandCards[position] = null;
        foreach (var position in player1HandPositions)
            player1HandCards[position] = null;
    }

    public bool AddCardToFirstEmptyPlayer0Position(Card card) // ��������� ����� � ������ ��������� ������� ���� ������� ������.
    {
        foreach (var player0Position in player0HandPositions)
        {
            var boardPosition = player0Position.GetComponent<BoardPosition>();
            if (!boardPosition.IsOccupied)
            {
                card.transform.position = player0Position.position;
                boardPosition.OccupyPosition(card);
                player0HandCards[player0Position] = card;
                return true;
            }
        }
        return false;
    }
    public bool AddCardToFirstEmptyPlayer1Position(Card card) // ��������� ����� � ������ ��������� ������� ���� ������� ������.
    {
        foreach (var player1Position in player1HandPositions)
        {
            var boardPosition = player1Position.GetComponent<BoardPosition>();
            if (!boardPosition.IsOccupied)
            {
                card.transform.position = player1Position.position;
                boardPosition.OccupyPosition(card);
                player1HandCards[player1Position] = card;
                return true;
            }
        }
        return false;
    }
    public void DiscardAllPlayer0Cards(DeckManager deckManager) // ���������� ��� ����� �� ���� ������� ������ � ������ ������.
    {
        foreach (var position in player0HandPositions)
        {
            var boardPosition = position.GetComponent<BoardPosition>();
            if (boardPosition.IsOccupied)
            {
                Card card = player0HandCards[position];
                if (card != null)
                {
                    deckManager.AddCardToPlayer0DiscardDeck(card);
                    boardPosition.VacatePosition();
                    player0HandCards[position] = null;
                }
            }
        }
    }
    public void DiscardAllPlayer1Cards(DeckManager deckManager) // ���������� ��� ����� �� ���� ������� ������ � ������ ������.
    {
        foreach (var position in player1HandPositions)
        {
            var boardPosition = position.GetComponent<BoardPosition>();
            if (boardPosition.IsOccupied)
            {
                Card card = player1HandCards[position];
                if (card != null)
                {
                    deckManager.AddCardToPlayer1DiscardDeck(card);
                    boardPosition.VacatePosition();
                    player1HandCards[position] = null;
                }
            }
        }
    }
    public Card GetCardFromPlayer0Position(int positionIndex) // �������� ����� �� ������������ ������� ���� ������� ������.
    {
        if (positionIndex >= 0 && positionIndex < player0HandPositions.Length)
        {
            var position = player0HandPositions[positionIndex];
            if (player0HandCards.ContainsKey(position))
            {
                return player0HandCards[position];
            }
        }
        return null;
    }
    public Card GetCardFromPlayer1Position(int positionIndex) // �������� ����� �� ������������ ������� ���� ������� ������.
    {
        if (positionIndex >= 0 && positionIndex < player1HandPositions.Length)
        {
            var position = player1HandPositions[positionIndex];
            if (player1HandCards.ContainsKey(position))
            {
                return player1HandCards[position];
            }
        }
        return null;
    }
    public void RemoveCardFromPlayer0Position(int positionIndex) // ������� ����� �� ������������ ������� ���� ������� ������.
    {
        if (positionIndex >= 0 && positionIndex < player0HandPositions.Length)
        {
            var position = player0HandPositions[positionIndex];
            var boardPosition = position.GetComponent<BoardPosition>();
            if (boardPosition.IsOccupied)
            {
                boardPosition.VacatePosition();
                player0HandCards[position] = null;
            }
        }
    }
    public void RemoveCardFromPlayer1Position(int positionIndex) // ������� ����� �� ������������ ������� ���� ������� ������.
    {
        if (positionIndex >= 0 && positionIndex < player1HandPositions.Length)
        {
            var position = player1HandPositions[positionIndex];
            var boardPosition = position.GetComponent<BoardPosition>();
            if (boardPosition.IsOccupied)
            {
                boardPosition.VacatePosition();
                player1HandCards[position] = null;
            }
        }
    }
}