using System.Collections;
using UnityEngine;

public class EnemyTurnManager : MonoBehaviour
{
    public HandManager handManagerScript; // ������ �� �������� ����
    public DeckManager deckManagerScript; // ������ �� �������� ������
    public ClickHandler clickHandlerScript; // ������ �� �������� ������
    public TurnManager turnManager;

    // ����� ��� ������ ���� ����������
    public void StartEnemyTurn()
    {
        StartCoroutine(ActivateAllEnemyHandCards());
    }

    // �������� ��� ��������� ���� ���� � ���� ���������� � ���������
    IEnumerator ActivateAllEnemyHandCards()
    {
        for (int i = 0; i < handManagerScript.player1HandPositions.Length; i++)
        {
            Card cardToActivate = handManagerScript.GetCardFromPlayer1Position(i);
            if (cardToActivate != null)
            {
                // ���������� ������ �����
                cardToActivate.ActivateCardEffect();
                deckManagerScript.AddCardToPlayer1PlayedHandDeck(cardToActivate);

                // ���� ����� �������� ������, �������� ���������� � �� ������� ����� ���������
                if (cardToActivate.unitType != "NonUnit")
                {
                    bool placed = clickHandlerScript.PlaceCardOnPlayer1UnitPosition(cardToActivate);
                    if (!placed)
                    {
                    }
                }
                handManagerScript.RemoveCardFromPlayer1Position(i);

                // �������� ����� ���������� ��������� �����
                yield return new WaitForSeconds(1f/4);
            }
        }

        // ����� ��������� ���� ���� �������� �������� ������ ����� ����� �� ��������
        StartCoroutine(BuyCardsForPlayer1());
    }
    IEnumerator BuyCardsForPlayer1()
    {
        yield return new WaitForSeconds(1f/4);

        bool canBuyMore = true;
        while (canBuyMore)
        {
            int storePosition = Random.Range(0, 4); // ������� ����� ������� �� 0 �� 3
            Card cardToBuy = deckManagerScript.storeManager.GetCardFromStore(storePosition);

            if (cardToBuy != null && ResourcesManager.instance.player1Gold >= cardToBuy.cost)
            {
                ResourcesManager.instance.ChangePlayer1Gold(-cardToBuy.cost);
                deckManagerScript.storeManager.storePositions[storePosition].GetComponent<BoardPosition>().VacatePosition();

                // ���� ����� ���� � ����� "Activation", ���������, ���� �� ��������� ������� ��� ���������� �� ����
                if (cardToBuy.unitType != "NonUnit" && cardToBuy.activationType == "Activation")
                {
                    deckManagerScript.AddCardToPlayer1PlayedHandDeck(cardToBuy);
                    bool placed = clickHandlerScript.PlaceCardOnPlayer1UnitPosition(cardToBuy);
                }
                else if (cardToBuy.activationType == "Activation")
                {
                    // ��� ���� � ����� "Activation", ������� �� �������� �������, ����� ��������� � ������ ����������� ����
                    deckManagerScript.AddCardToPlayer1PlayedHandDeck(cardToBuy);
                }
                else
                {
                    // ��� ��������� ����� ���������� � ������ ������
                    deckManagerScript.AddCardToPlayer1DiscardDeck(cardToBuy);
                }

                clickHandlerScript.AddCardToStore(); // ��������� ������� ����� ������

                yield return new WaitForSeconds(1f);
            }
            else
            {
                canBuyMore = false;
            }
        }

        turnManager.EndEnemyTurn();
    }
}