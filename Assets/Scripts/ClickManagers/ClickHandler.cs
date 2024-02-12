using System;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private StoreManager storeManagerScript;
    [SerializeField] private DeckManager deckManagerScript;
    [SerializeField] private HandManager handManagerScript;
    [SerializeField] private FoldDeck shufflingDeckScript;
    [SerializeField] private ResourcesManager resourcesManagerScript;
    [SerializeField] private TurnManager turnManagerScript;

    public Card selectedMilitaryUnit;
    public int selectedMilitaryUnitIndex = -1;
    public Card selectedTemple;
    private BoardPosition selectedTemplePosition = null;

    [SerializeField] public Transform[] player0UnitPositions;
    [SerializeField] public Transform[] player1UnitPositions;

    private bool canInteract = true;

    public void EnablePlayer0Interactions(bool enable) => canInteract = enable;

    public void ResetSelectedMilitaryUnit()
    {
        selectedMilitaryUnit = null;
        selectedMilitaryUnitIndex = -1;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canInteract)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                HandleClick(hit.transform.gameObject);
            }
        }
    }

    private void HandleClick(GameObject clickedObject)
    {
        // ���� ���� ��� ��� ������
        if (selectedTemple != null)
        {
            // ���������, ��� �� ������ ���� �� ������ � ���� ��� ������ �� ����
            if (clickedObject.name.StartsWith("Player0HandClicker") || clickedObject.name.StartsWith("Player0UnitClicker"))
            {
                int clickedPosition = int.Parse(clickedObject.name.Substring(clickedObject.name.Length - 1)) - 1;

                if (clickedObject.name.StartsWith("Player0HandClicker"))
                {
                    // ��������� ����� �� ����� � ����
                    Card cardInHand = handManagerScript.GetCardFromPlayer0Position(clickedPosition);
                    if (cardInHand != null)
                    {
                        // ������� ������ �����
                        Destroy(cardInHand.gameObject);
                        // ����������� �������
                        handManagerScript.player0HandPositions[clickedPosition].GetComponent<BoardPosition>().VacatePosition();
                        // ��������� ���������� ������������� � �����
                        selectedTemple.SetUsageCount(selectedTemple.usageCount - 1);
                        // ��������� ���� ���� �� ���������� ����� �� ����
                        ResourcesManager.instance.ChangePlayer0Zeal(1);
                    }
                }
                else if (clickedObject.name.StartsWith("Player0UnitClicker"))
                {
                    BoardPosition unitPosition = player0UnitPositions[clickedPosition].GetComponent<BoardPosition>();
                    if (unitPosition != null && unitPosition.IsOccupied)
                    {
                        RemoveCardFromPlayedDeckIfPresent(unitPosition.GetCard()); // ������� ����� �� ������ ����������� ����
                        Destroy(unitPosition.GetCard().gameObject); // ������ ���������� ������ �����
                        unitPosition.VacatePosition();
                        selectedTemple.SetUsageCount(selectedTemple.usageCount - 1);
                        // ��������� ���� ���� �� ���������� ����� � ���� ������
                        ResourcesManager.instance.ChangePlayer0Zeal(2);
                    }
                }

                // ���������� ��������� ����
                ResetSelectedTemple();
                return;
            }
            else
            {
                // ���� ���� ��� �� �� ������ � ���� � �� �� ������ �� ����, ���������� ��������� ����
                ResetSelectedTemple();
            }
        }
        if (selectedMilitaryUnit != null)
        {
            // ���������, �������� �� ������ ����� ��������� �������� ����� ��� ������
            bool isEnemyUnitOrTempleClick = clickedObject.name.StartsWith("Player1UnitClicker") || clickedObject.name.StartsWith("SacredSiteClicker");
            bool isValidTarget = false;

            if (isEnemyUnitOrTempleClick)
            {
                // ���� ��� ��������� ����, ���������, ������ �� �������
                if (clickedObject.name.StartsWith("Player1UnitClicker"))
                {
                    int positionIndex = int.Parse(clickedObject.name.Substring("Player1UnitClicker".Length)) - 1;
                    if (positionIndex >= 0 && positionIndex < player1UnitPositions.Length)
                    {
                        BoardPosition position = player1UnitPositions[positionIndex].GetComponent<BoardPosition>();
                        if (position != null && position.IsOccupied)
                        {
                            isValidTarget = true; // ������� ������, ������ �������� ���� ��� �����
                        }
                    }
                }
                // ���� ��� ����, ���������, �������� �� ���� � ���� �������
                else if (clickedObject.name.StartsWith("SacredSiteClicker"))
                {
                    var sacredSite = clickedObject.GetComponent<SacredSite>();
                    if (sacredSite != null && sacredSite.linkedPosition.isTempleBuilt)
                    {
                        isValidTarget = true; // ���� ��������, ������ �������� ���� ��� �����
                    }
                }
            }

            // ���� ���� ��� ������, �� ���� ���������, ���������� �����
            if (!isValidTarget)
            {
                ResetSelectedMilitaryUnit();
                return; 
            }
        }
        // ��������� ��������� ������
        switch (clickedObject.name)
        {
            case "EndTurnClicker":
                turnManagerScript.EndPlayer0Turn();
                break;
            case string name when name.StartsWith("Player0UnitClicker"):
                int player0UnitPositionIndex = int.Parse(name.Substring("Player0UnitClicker".Length)) - 1;
                SelectMilitaryUnit(player0UnitPositionIndex);
                break;
            case string name when name.StartsWith("Player1UnitClicker"):
                int player1UnitPositionIndex = int.Parse(name.Substring("Player1UnitClicker".Length)) - 1;
                AttackPlayer1Unit(player1UnitPositionIndex);
                break;
            case string name when name.StartsWith("SacredSiteClicker"):
                var sacredSite = clickedObject.GetComponent<SacredSite>();
                if (selectedMilitaryUnit != null) // ������� ��� ����� ����� ��������� ������� ������
                {
                    int sacredSiteIndex = int.Parse(name.Substring("SacredSiteClicker".Length)) - 1;
                    AttackTemple(sacredSiteIndex); // ����� ������ ����� �����
                    return;
                }
                else if (sacredSite != null && !sacredSite.linkedPosition.isTempleBuilt) // ���������, �� �������� �� ��� ����
                {
                    // �������� �������� ������
                    string currentPlayer = turnManagerScript.currentTurn == TurnManager.Turn.Player0 ? "Player0" : "Player1";

                    // ���������, ����� �� ������� ����� ������� ����
                    bool canBuildTemple = currentPlayer == "Player0"
                        ? ResourcesManager.instance.canBuildTemplePlayer0
                        : ResourcesManager.instance.canBuildTemplePlayer1;

                    if (!canBuildTemple)
                    {
                        return;
                    }

                    int cost = ResourcesManager.instance.CalculatePlayer0TempleCost();
                    if (ResourcesManager.instance.player0Gold >= cost && ResourcesManager.instance.player0Zeal >= cost)
                    {
                        ClickerManager.ToggleAllClickers();
                    }
                    else
                    {
                        return;
                    }
                }
                else 
                {
                    if (sacredSite != null && sacredSite.linkedPosition.isTempleBuilt)
                    {
                        Card templeCard = sacredSite.linkedPosition.GetCard();
                        string currentPlayer = turnManagerScript.currentTurn == TurnManager.Turn.Player0 ? "Player0" : "Player1";

                        // ���������, ����������� �� ���� �������� ������
                        if (templeCard.templeOwner != currentPlayer)
                        {
                        }
                        else
                        {
                            // ���������, ���� �� � ����� ���������� �������������
                            if (templeCard.usageCount > 0)
                            {
                                selectedTemple = templeCard;
                                selectedTemplePosition = sacredSite.linkedPosition;
                            }
                        }
                    }
                }
                break;
            case string name when name.Contains("NameNew gods templeAsatru 1Clicker"):
                PlaceTemple(TempleType.NewGods);
                break;
            case string name when name.Contains("NameGod2 templeAsatruClicker"):
                PlaceTemple(TempleType.God2);
                break;
            case string name when name.Contains("NameGod1 templeAsatruClicker"):
                PlaceTemple(TempleType.God1);
                break;
            default:
                if (clickedObject.name.StartsWith("StoreClicker"))
                {
                    int storePosition = int.Parse(clickedObject.name.Substring("StoreClicker".Length)) - 1;
                    BuyCardFromStore(storePosition);
                }
                else if (clickedObject.name.StartsWith("Player0HandClicker"))
                {
                    int handPosition = int.Parse(clickedObject.name.Substring("Player0HandClicker".Length)) - 1;
                    ActivateCardFromHand(handPosition);
                }
                break;
        }
    }
    private void SelectMilitaryUnit(int positionIndex)
    {
        if (positionIndex >= 0 && positionIndex < player0UnitPositions.Length)
        {
            var boardPosition = player0UnitPositions[positionIndex].GetComponent<BoardPosition>();
            if (boardPosition != null && boardPosition.IsOccupied && boardPosition.GetCard().unitType == "MilitaryUnit")
            {
                var unit = boardPosition.GetCard();
                if (unit.usageCount > 0)
                {
                    selectedMilitaryUnit = unit;
                    selectedMilitaryUnitIndex = positionIndex; // ��������� ������ ���������� �����
                }
            }
        }
    }
    private void AttackPlayer1Unit(int player1UnitPositionIndex)
    {
        // ���������, ��� �� ������ ���� ������
        if (selectedMilitaryUnit == null || selectedMilitaryUnitIndex < 0)
        {
            ResetSelectedMilitaryUnit();
            return;
        }

        // ���������, ����� �� ��������� ���� �����������
        if (selectedMilitaryUnit.usageCount <= 0)
        {
            ResetSelectedMilitaryUnit();
            return;
        }

        // ���������, ���� �� �� ���� ����� ��������� ���� "Military"
        bool hasMilitaryUnit = CheckForMilitaryUnits(player1UnitPositions);

        if (player1UnitPositionIndex >= 0 && player1UnitPositionIndex < player1UnitPositions.Length)
        {
            var player1BoardPosition = player1UnitPositions[player1UnitPositionIndex].GetComponent<BoardPosition>();
            if (player1BoardPosition != null && player1BoardPosition.IsOccupied)
            {
                var player1Unit = player1BoardPosition.GetCard();

                // ���� ���� ����� "Military", �� ��������� ���� �� �������� "Military", ��������� ��������
                if (hasMilitaryUnit && player1Unit.unitType != "MilitaryUnit")
                {
                    ResetSelectedMilitaryUnit();
                    return;
                }

                // ������ ��������� ������������
                if (selectedMilitaryUnit.activationType == "Activation")
                {
                    // ������� ���� ������
                    Destroy(selectedMilitaryUnit.gameObject);
                }
                else
                {
                    // ���������� ���� ������ � ������ ������
                    deckManagerScript.AddCardToPlayer0DiscardDeck(selectedMilitaryUnit);
                }
                player0UnitPositions[selectedMilitaryUnitIndex].GetComponent<BoardPosition>().VacatePosition();
                if (selectedMilitaryUnit != null)
                {
                    selectedMilitaryUnit.usageCount -= 1;
                }
                if (player1Unit.activationType == "Activation")
                {
                    // ������� ���� ����������
                    Destroy(player1Unit.gameObject);
                }
                else
                {
                    // ���������� ���� ���������� � ������ ������
                    deckManagerScript.AddCardToPlayer1DiscardDeck(player1Unit);
                }
                player1BoardPosition.VacatePosition();

                // ���������� ���������� �����
                ResetSelectedMilitaryUnit();
            }
        }
        else
        {
            ResetSelectedMilitaryUnit();
        }
    }
    private void AttackTemple(int sacredSiteIndex)
    {
        BoardPosition sacredSitePosition = SacredSiteManager.SelectedPosition;
        if (sacredSitePosition != null && sacredSitePosition.isTempleBuilt)
        {
            Card templeCard = sacredSitePosition.GetCard();
            if (templeCard != null && templeCard.templeType == "Temple")
            {
                string templeOwner = templeCard.templeOwner;
                string currentPlayer = turnManagerScript.currentTurn == TurnManager.Turn.Player0 ? "Player0" : "Player1";

                // ���������, ���� �� �� ���� ���������� ������� �����
                if (CheckForMilitaryUnits(player1UnitPositions) && templeOwner != currentPlayer)
                {
                    // ���� ���� ������� �����, ����� �� ���� �������������
                    ResetSelectedMilitaryUnit();
                    return;
                }

                if (templeOwner != currentPlayer)
                {

                    // ������� ����
                    Destroy(templeCard.gameObject);
                    sacredSitePosition.VacatePosition();
                    sacredSitePosition.isTempleBuilt = false;

                    // ��������� ���������� ����������� ������ � ��������� ��������� �����
                    if (templeOwner == "Player0")
                    {
                        ResourcesManager.instance.player0TemplesBuilt--;
                        ResourcesManager.instance.UpdatePlayer0TempleCostText();
                        WinLoseConditions.Instance.CheckWinCondition();
                    }
                    else if (templeOwner == "Player1")
                    {
                        ResourcesManager.instance.player1TemplesBuilt--;
                        ResourcesManager.instance.UpdatePlayer1TempleCostText();
                        WinLoseConditions.Instance.CheckWinCondition();
                    }
                    if (selectedMilitaryUnit != null)
                    {
                        selectedMilitaryUnit.usageCount -= 1;
                    }
                    // ��������� �����, ��������������� �����
                    if (selectedMilitaryUnit.activationType == "Activation")
                    {
                        // ������� ���� ������
                        Destroy(selectedMilitaryUnit.gameObject);
                    }
                    else
                    {
                        // ���������� ���� ������ � ������ ������
                        deckManagerScript.AddCardToPlayer0DiscardDeck(selectedMilitaryUnit);
                    }
                    player0UnitPositions[selectedMilitaryUnitIndex].GetComponent<BoardPosition>().VacatePosition();

                    ResetSelectedMilitaryUnit();
                }
                else
                {
                    ResetSelectedMilitaryUnit();
                }
            }
        }
        else
        {
            ResetSelectedMilitaryUnit();
        }
    }
    private bool CheckForMilitaryUnits(Transform[] positions)
    {
        foreach (var position in positions)
        {
            var boardPosition = position.GetComponent<BoardPosition>();
            if (boardPosition != null && boardPosition.IsOccupied && boardPosition.GetCard().unitType == "MilitaryUnit")
            {
                return true;
            }
        }
        return false;
    }
    private void PlaceTemple(TempleType type)
    {
        // �������� �������� ������
        string currentPlayer = turnManagerScript.currentTurn == TurnManager.Turn.Player0 ? "Player0" : "Player1";

        // ���������, ����� �� ������� ����� ������� ����
        bool canBuildTemple = currentPlayer == "Player0"
            ? ResourcesManager.instance.canBuildTemplePlayer0
            : ResourcesManager.instance.canBuildTemplePlayer1;

        if (!canBuildTemple)
        {
            return;
        }

        int cost = ResourcesManager.instance.CalculatePlayer0TempleCost();
        if (ResourcesManager.instance.player0Gold >= cost && ResourcesManager.instance.player0Zeal >= cost)
        {
            ResourcesManager.instance.ChangePlayer0Gold(-cost);
            ResourcesManager.instance.ChangePlayer0Zeal(-cost);
            ResourcesManager.instance.player0TemplesBuilt++;

            if (SacredSiteManager.SelectedPosition != null && !SacredSiteManager.SelectedPosition.isTempleBuilt)
            {
                GameObject templePrefab = GetTemplePrefab(type);
                if (templePrefab != null)
                {
                    GameObject templeInstance = Instantiate(templePrefab, SacredSiteManager.SelectedPosition.transform.position, Quaternion.identity);
                    Card templeCard = templeInstance.GetComponent<Card>();
                    templeCard.templeOwner = currentPlayer;
                    SacredSiteManager.SelectedPosition.OccupyPosition(templeCard);
                    SacredSiteManager.SelectedPosition.isTempleBuilt = true;

                    // ����� ��������� ������������� ����� �������� ����������, ����������� ������� ����� �� ���������� ����
                    if (currentPlayer == "Player0")
                    {
                        ResourcesManager.instance.canBuildTemplePlayer0 = false;
                    }
                    else
                    {
                        ResourcesManager.instance.canBuildTemplePlayer1 = false;
                    }
                }
                ClickerManager.ToggleAllClickers();
            }
            ResourcesManager.instance.UpdatePlayer0TempleCostText();
        }
    }
    private GameObject GetTemplePrefab(TempleType type)
    {
        var templeManager = FindObjectOfType<DeckManager>();
        switch (type)
        {
            case TempleType.God1: return templeManager.god1TemplePrefab;
            case TempleType.God2: return templeManager.god2TemplePrefab;
            case TempleType.NewGods: return templeManager.newGodsTemplePrefab;
            default: return null;
        }
    }

    enum TempleType { God1, God2, NewGods }
    private void ActivateCardFromHand(int handPosition)
    {
        Card cardToActivate = handManagerScript.GetCardFromPlayer0Position(handPosition);
        if (cardToActivate != null)
        {
            cardToActivate.ActivateCardEffect();

            // �������� ����� � ������ PlayedDeck ���������� �� � ����
            deckManagerScript.AddCardToPlayer0PlayedHandDeck(cardToActivate);

            // ���� ����� �������� ������, ������������� �������� ���������� � �� ������� �����
            if (cardToActivate.unitType != "NonUnit" && PlaceCardOnPlayer0UnitPosition(cardToActivate))
            {
                // ����� ������� ��������� �� ������� �����, ������� � �� ����
                handManagerScript.RemoveCardFromPlayer0Position(handPosition);
                return; // ��������� �����, ���� ����� ������� ���������
            }

            // ������� ����� �� ����, ���� ��� �� �������� ������ ��� �� ������� ���������� �� ������� �����
            handManagerScript.RemoveCardFromPlayer0Position(handPosition);
        }
    }

    private bool PlaceCardOnPlayer0UnitPosition(Card card)
    {
        foreach (var position in player0UnitPositions)
        {
            var boardPosition = position.GetComponent<BoardPosition>();
            if (!boardPosition.IsOccupied)
            {
                card.transform.position = position.position;
                boardPosition.OccupyPosition(card);
                return true;
            }
        }
        return false;
    }
    public bool PlaceCardOnPlayer1UnitPosition(Card card)
    {
        foreach (var position in player1UnitPositions)
        {
            var boardPosition = position.GetComponent<BoardPosition>();
            if (!boardPosition.IsOccupied)
            {
                card.transform.position = position.position;
                boardPosition.OccupyPosition(card);
                return true;
            }
        }
        return false;
    }
    private void BuyCardFromStore(int storePosition)
    {
        Card cardToBuy = storeManagerScript.GetCardFromStore(storePosition);
        if (cardToBuy != null && ResourcesManager.instance.player0Gold >= cardToBuy.cost)
        {
            ResourcesManager.instance.ChangePlayer0Gold(-cardToBuy.cost);
            storeManagerScript.storePositions[storePosition].GetComponent<BoardPosition>().VacatePosition();

            // ��������� ����� � PlayedDeck, ���� ��� ����� ��� "Activation"
            if (cardToBuy.activationType == "Activation")
            {
                deckManagerScript.AddCardToPlayer0PlayedHandDeck(cardToBuy);
                // �������� ���������� ���� �� �������, ���� ����� �������� ������
                if (cardToBuy.unitType != "NonUnit")
                {
                    PlaceCardOnPlayer0UnitPosition(cardToBuy);
                }
            }
            else
            {
                // ��� ��������� ����� ���������� � DiscardDeck
                deckManagerScript.AddCardToPlayer0DiscardDeck(cardToBuy);
            }

            AddCardToStore(); // ��������� ������� ����� ������
        }
    }

    public void AddCardToStore()
    {
        if (shufflingDeckScript.deck.Count > 0)
        {
            Card nextCard = shufflingDeckScript.GetLastCard();
            if (storeManagerScript.AddCardToFirstEmptyStore(nextCard))
            {
                shufflingDeckScript.deck.Remove(nextCard);
            }
        }
    }

    public void RefillPlayer0Hand()
    {
        for (int i = 0; i < handManagerScript.player0HandPositions.Length; i++)
        {
            if (!handManagerScript.player0HandPositions[i].GetComponent<BoardPosition>().IsOccupied)
            {
                if (deckManagerScript.player0DrawDeck.Count == 0 && deckManagerScript.player0DiscardDeck.Count > 0)
                {
                    deckManagerScript.MovePlayer0DiscardToPlayer0DrawDeck();
                }
                Player0TakeCard();
            }
        }
    }
    public void RefillPlayer1Hand()
    {
        for (int i = 0; i < handManagerScript.player1HandPositions.Length; i++)
        {
            if (!handManagerScript.player1HandPositions[i].GetComponent<BoardPosition>().IsOccupied)
            {
                if (deckManagerScript.player1DrawDeck.Count == 0 && deckManagerScript.player1DiscardDeck.Count > 0)
                {
                    deckManagerScript.MovePlayer1DiscardToPlayer1DrawDeck();
                }
                player1TakeCard();
            }
        }
    }

    private void Player0TakeCard()
    {
        if (deckManagerScript.player0DrawDeck.Count > 0)
        {
            Card nextCard = deckManagerScript.GetLastCardFromPlayer0DrawDeck();
            if (handManagerScript.AddCardToFirstEmptyPlayer0Position(nextCard))
            {
                deckManagerScript.player0DrawDeck.Remove(nextCard);
            }
        }
    }
    private void player1TakeCard()
    {
        if (deckManagerScript.player1DrawDeck.Count > 0)
        {
            Card nextCard = deckManagerScript.GetLastCardFromPlayer1DrawDeck();
            if (handManagerScript.AddCardToFirstEmptyPlayer1Position(nextCard))
            {
                deckManagerScript.player1DrawDeck.Remove(nextCard);
            }
        }
    }
    public void ResetSelectedTemple()
    {
        selectedTemple = null;
        selectedTemplePosition = null;
    }
    private void RemoveCardFromPlayedDeckIfPresent(Card card)
    {
        // ���������, ���������� �� ����� � ������ ����������� ����
        if (deckManagerScript.player0PlayedHandDeck.Contains(card))
        {
            // ������� ����� �� ������ ����������� ����
            deckManagerScript.player0PlayedHandDeck.Remove(card);
        }
    }
}