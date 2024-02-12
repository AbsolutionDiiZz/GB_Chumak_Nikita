using UnityEngine;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public EnemyTurnManager enemyTurnManagerScript;
    public DeckManager deckManagerScript;
    public HandManager handManagerScript;
    public ClickHandler clickHandlerScript;

    public enum Turn { Player0, Player1 }
    public Turn currentTurn;

    public void StartGame()
    {
        SetCurrentTurn(Turn.Player0); // Игра начинается с хода игрока
    }

    public void SetCurrentTurn(Turn turn)
    {
        currentTurn = turn;
        switch (currentTurn)
        {
            case Turn.Player0:
                StartPlayer0Turn();
                break;
            case Turn.Player1:
                enemyTurnManagerScript.StartEnemyTurn();
                clickHandlerScript.EnablePlayer0Interactions(false);
                break;
        }
    }

    private void StartPlayer0Turn()
    {
        clickHandlerScript.EnablePlayer0Interactions(true);
    }

    public void EndPlayer0Turn()
    {
        ProcessEndTurnActions("player0");
        SetCurrentTurn(Turn.Player1);
    }

    public void EndEnemyTurn()
    {
        ProcessEndTurnActions("player1");
        SetCurrentTurn(Turn.Player0);
        StartPlayer0Turn(); // Начинаем следующий ход игрока
    }

    private void ProcessEndTurnActions(string playerType)
    {
        clickHandlerScript.ResetSelectedMilitaryUnit();
        Transform[] unitPositions = playerType == "player0" ? clickHandlerScript.player0UnitPositions : clickHandlerScript.player1UnitPositions;
        List<Card> playedHandDeck = playerType == "player0" ? deckManagerScript.player0PlayedHandDeck : deckManagerScript.player1PlayedHandDeck;

        // Логика для юнитов
        foreach (Transform unitPosition in unitPositions)
        {
            var boardPosition = unitPosition.GetComponent<BoardPosition>();
            if (boardPosition.IsOccupied)
            {
                var card = boardPosition.GetCard();
                if (card != null && (card.unitType == "MilitaryUnit" || card.unitType == "PeacefulUnit"))
                {
                    card.SetUsageCount(1); // Устанавливаем usageCount в 1 для всех карт юнитов на поле
                }

                if (playedHandDeck.Contains(card))
                {
                    playedHandDeck.Remove(card);
                }
            }
        }

        // Логика для храмов
        foreach (var sacredSite in SacredSiteManager.GetAllSacredSites()) // Предполагается, что у вас есть метод GetAllSacredSites()
        {
            var boardPosition = sacredSite.linkedPosition;
            if (boardPosition != null && boardPosition.IsOccupied && boardPosition.isTempleBuilt)
            {
                var templeCard = boardPosition.GetCard();
                if (templeCard != null && templeCard.templeType == "Temple")
                {
                    templeCard.SetUsageCount(1); // Устанавливаем usageCount в 1 для карты храма
                }
            }
        }

        if (playerType == "player0")
        {
            deckManagerScript.MovePlayedCardsToPlayer0DiscardDeck();
            handManagerScript.DiscardAllPlayer0Cards(deckManagerScript);
            clickHandlerScript.RefillPlayer0Hand();
            ResourcesManager.instance.ChangePlayer0Gold(-ResourcesManager.instance.player0Gold);
            ResourcesManager.instance.canBuildTemplePlayer0 = true;
        }
        else
        {
            deckManagerScript.MovePlayedCardsToPlayer1DiscardDeck();
            handManagerScript.DiscardAllPlayer1Cards(deckManagerScript);
            clickHandlerScript.RefillPlayer1Hand();
            ResourcesManager.instance.ChangePlayer1Gold(-ResourcesManager.instance.player1Gold);
            ResourcesManager.instance.canBuildTemplePlayer1 = true;
        }

        deckManagerScript.UpdateDeckTexts();
        clickHandlerScript.EnablePlayer0Interactions(playerType == "player0");
    }
}
