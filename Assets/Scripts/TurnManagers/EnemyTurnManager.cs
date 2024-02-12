using System.Collections;
using UnityEngine;

public class EnemyTurnManager : MonoBehaviour
{
    public HandManager handManagerScript; // Ссылка на менеджер руки
    public DeckManager deckManagerScript; // Ссылка на менеджер колоды
    public ClickHandler clickHandlerScript; // Ссылка на менеджер колоды
    public TurnManager turnManager;

    // Метод для начала хода противника
    public void StartEnemyTurn()
    {
        StartCoroutine(ActivateAllEnemyHandCards());
    }

    // Корутина для активации всех карт в руке противника с задержкой
    IEnumerator ActivateAllEnemyHandCards()
    {
        for (int i = 0; i < handManagerScript.player1HandPositions.Length; i++)
        {
            Card cardToActivate = handManagerScript.GetCardFromPlayer1Position(i);
            if (cardToActivate != null)
            {
                // Активируем эффект карты
                cardToActivate.ActivateCardEffect();
                deckManagerScript.AddCardToPlayer1PlayedHandDeck(cardToActivate);

                // Если карта является юнитом, пытаемся разместить её на позицию юнита оппонента
                if (cardToActivate.unitType != "NonUnit")
                {
                    bool placed = clickHandlerScript.PlaceCardOnPlayer1UnitPosition(cardToActivate);
                    if (!placed)
                    {
                    }
                }
                handManagerScript.RemoveCardFromPlayer1Position(i);

                // Задержка перед активацией следующей карты
                yield return new WaitForSeconds(1f/4);
            }
        }

        // После активации всех карт оппонент пытается купить новые карты из магазина
        StartCoroutine(BuyCardsForPlayer1());
    }
    IEnumerator BuyCardsForPlayer1()
    {
        yield return new WaitForSeconds(1f/4);

        bool canBuyMore = true;
        while (canBuyMore)
        {
            int storePosition = Random.Range(0, 4); // Магазин имеет позиции от 0 до 3
            Card cardToBuy = deckManagerScript.storeManager.GetCardFromStore(storePosition);

            if (cardToBuy != null && ResourcesManager.instance.player1Gold >= cardToBuy.cost)
            {
                ResourcesManager.instance.ChangePlayer1Gold(-cardToBuy.cost);
                deckManagerScript.storeManager.storePositions[storePosition].GetComponent<BoardPosition>().VacatePosition();

                // Если карта юнит с типом "Activation", проверяем, есть ли свободные позиции для размещения на поле
                if (cardToBuy.unitType != "NonUnit" && cardToBuy.activationType == "Activation")
                {
                    deckManagerScript.AddCardToPlayer1PlayedHandDeck(cardToBuy);
                    bool placed = clickHandlerScript.PlaceCardOnPlayer1UnitPosition(cardToBuy);
                }
                else if (cardToBuy.activationType == "Activation")
                {
                    // Для карт с типом "Activation", которые не являются юнитами, также добавляем в колоду разыгранных карт
                    deckManagerScript.AddCardToPlayer1PlayedHandDeck(cardToBuy);
                }
                else
                {
                    // Все остальные карты направляем в колоду сброса
                    deckManagerScript.AddCardToPlayer1DiscardDeck(cardToBuy);
                }

                clickHandlerScript.AddCardToStore(); // Обновляем магазин новой картой

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