using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    public GameObject attackHighlight;
    public GameObject offeringHighlight;
    public GameObject sacredEmptyHighlight;
    public GameObject sacredCanUseHighlight;
    public GameObject sacredCantUseHighlight;
    public GameObject handUseHighlight;
    public GameObject storeCanBuyHighlight;
    public GameObject storeCantBuyHighlight;
    public GameObject unitMilitaryCanUseHighlight;
    public GameObject unitMilitaryCantUseHighlight;
    public GameObject unitPeacefulCanUseHighlight; 
    public GameObject unitPeacefulCantUseHighlight;

    private GameObject currentHighlight;
    private GameObject[] currentHighlights;

    public Transform targetPosition;

    public TurnManager turnManagerScript;
    public ClickHandler clickHandlerScript;

    void Update()
    {
        // ��������� ����� �����
        if (Input.GetMouseButtonDown(0)) // �������� ������� ����� ������ ����
        {
            HandleMouseClick();
        }
    }
    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0) && clickHandlerScript.selectedTemple == null && clickHandlerScript.selectedMilitaryUnit == null)
        {
            RemoveAllHighlights();
        }
        if (clickHandlerScript.selectedTemple != null)
        {
            ShowOfferingHighlightForAllRelevantPositions();
        }
        else if (clickHandlerScript.selectedMilitaryUnit != null && clickHandlerScript.selectedMilitaryUnit.unitType == "MilitaryUnit")
        {
            ShowAttackHighlightForEnemyPositions();
        }
    }
    void OnMouseEnter()
    {
        if (currentHighlight == null && targetPosition != null)
        {
            HandleNormalHighlight();
        }
    }

    void OnMouseExit()
    {
        if (currentHighlight != null)
        {
            Destroy(currentHighlight);
            currentHighlight = null;
        }
    }

    private void HandlePlayerHandHighlight()
    {
        BoardPosition boardPosition = targetPosition.GetComponent<BoardPosition>();
        if (boardPosition != null && boardPosition.IsOccupied)
        {
            currentHighlight = Instantiate(handUseHighlight, targetPosition.position, Quaternion.identity);
        }
    }

    private void HandleSacredSiteHighlight()
    {
        // �������� BoardPosition �� targetPosition, � �� �� �������� �������
        BoardPosition boardPosition = targetPosition.GetComponent<BoardPosition>();
        // �������������� prefabToUse ��� null
        GameObject prefabToUse = null;

        if (boardPosition != null)
        {
            // ���������, �������� �� ���� �� ��������� �������
            if (boardPosition.isTempleBuilt)
            {
                Card templeCard = boardPosition.GetCard(); // �������� ����� �����
                if (templeCard != null)
                {
                    // ����������, ����� ������ ������������ �� ������ ��������� � usageCount
                    string message;
                    if (templeCard.usageCount > 0 && templeCard.templeOwner == turnManagerScript.currentTurn.ToString())
                    {
                        prefabToUse = sacredCanUseHighlight; // ����� ������������ ����
                        message = $"���� ����������� {templeCard.templeOwner} � ����� �������������.";
                    }
                    else
                    {
                        prefabToUse = sacredCantUseHighlight; // ������ ������������ ����
                        message = $"���� ����������� {templeCard.templeOwner}. ��� ������������� ��� ���� ����������� ������� ������.";
                    }
                }
            }
            else
            {
                // ���������, ����� �� ����� ��������� ����
                if (ResourcesManager.instance.canBuildTemplePlayer0)
                {
                    prefabToUse = sacredEmptyHighlight; // ���������� ��������� ��� ����������� ���������
                }
            }
        }

        // ������ ���������, ���� ��� ������ ������
        if (prefabToUse != null)
        {
            currentHighlight = Instantiate(prefabToUse, targetPosition.position, Quaternion.identity);
        }
    }

    private void HandleStoreHighlight()
    {
        BoardPosition boardPosition = targetPosition.GetComponent<BoardPosition>();
        if (boardPosition != null && boardPosition.IsOccupied)
        {
            Card cardInPosition = boardPosition.GetCard();
            if (cardInPosition != null)
            {
                GameObject prefabToUse = cardInPosition.cost <= ResourcesManager.instance.player0Gold ? storeCanBuyHighlight : storeCantBuyHighlight;
                currentHighlight = Instantiate(prefabToUse, targetPosition.position, Quaternion.identity);
            }
        }
    }
    private void HandleUnitHighlight()
    {
        BoardPosition boardPosition = targetPosition.GetComponent<BoardPosition>();
        if (boardPosition != null && boardPosition.IsOccupied)
        {
            Card cardInPosition = boardPosition.GetCard();
            if (cardInPosition != null)
            {
                // �����������, ����� ������ ��������� ������������ �� ������ ���� � ��������� �����
                GameObject prefabToUse = null;
                if (cardInPosition.unitType == "MilitaryUnit")
                {
                    prefabToUse = cardInPosition.usageCount > 0 ? unitMilitaryCanUseHighlight : unitMilitaryCantUseHighlight;
                }
                else if (cardInPosition.unitType == "PeacefulUnit")
                {
                    prefabToUse = cardInPosition.usageCount > 0 ? unitPeacefulCanUseHighlight : unitPeacefulCantUseHighlight;
                }

                if (prefabToUse != null)
                {
                    currentHighlight = Instantiate(prefabToUse, targetPosition.position, Quaternion.identity);
                }
            }
        }
    }
    private void HandleNormalHighlight()
    {
        if (currentHighlight == null && targetPosition != null)
        {
            // ��������� ��� ���� � ���� ������
            if (gameObject.name.StartsWith("Player0HandClicker"))
            {
                HandlePlayerHandHighlight();
            }
            // ��������� ��� ��������� ����
            else if (gameObject.name.StartsWith("SacredSiteClicker"))
            {
                HandleSacredSiteHighlight();
            }
            // ��������� ��� ���� ��������
            else if (gameObject.name.StartsWith("StoreClicker"))
            {
                HandleStoreHighlight();
            }
            else if (gameObject.name.StartsWith("Player0UnitClicker"))
            {
                HandleUnitHighlight();
            }
        }
    }
    private void ShowOfferingHighlightForAllRelevantPositions()
    {
        // ������� ���������� ���������, ���� ��� ����
        if (currentHighlights != null)
        {
            foreach (var highlight in currentHighlights)
            {
                Destroy(highlight);
            }
        }

        // ������� ��� ������� ��� ���� � ������ ������
        var playerHandPositions = GameObject.FindGameObjectsWithTag("Player0HandPosition");
        var playerUnitPositions = GameObject.FindGameObjectsWithTag("Player0UnitPosition");

        // ���������� �������
        var allPositions = new GameObject[playerHandPositions.Length + playerUnitPositions.Length];
        playerHandPositions.CopyTo(allPositions, 0);
        playerUnitPositions.CopyTo(allPositions, playerHandPositions.Length);

        // ������� ������ ��� �������� ����� ���������
        var newHighlights = new List<GameObject>();

        foreach (var positionObj in allPositions)
        {
            var boardPosition = positionObj.GetComponent<BoardPosition>();
            if (boardPosition != null && boardPosition.IsOccupied)
            {
                var highlight = Instantiate(offeringHighlight, boardPosition.transform.position, Quaternion.identity);
                newHighlights.Add(highlight);
            }
        }

        // ��������� ����� ���������
        currentHighlights = newHighlights.ToArray();
    }
    private void ShowAttackHighlightForEnemyPositions()
    {
        // ������� ���������� ���������
        RemoveAllHighlights();

        // ������� ��� ������� ������ ����������
        var enemyUnitPositions = GameObject.FindGameObjectsWithTag("Player1UnitPosition");
        var sacredSites = GameObject.FindGameObjectsWithTag("SacredSite");

        // ������ ������ ��� ����� ���������
        var newHighlights = new List<GameObject>();
        bool hasMilitaryUnit = false;

        // ��������� ������� ������� ������ � ����������
        foreach (var positionObj in enemyUnitPositions)
        {
            var boardPosition = positionObj.GetComponent<BoardPosition>();
            if (boardPosition != null && boardPosition.IsOccupied && boardPosition.GetCard().unitType == "MilitaryUnit")
            {
                hasMilitaryUnit = true;
                break;
            }
        }

        // ��������� ������� ������ ����������
        foreach (var positionObj in enemyUnitPositions)
        {
            var boardPosition = positionObj.GetComponent<BoardPosition>();
            if (boardPosition != null && boardPosition.IsOccupied && (!hasMilitaryUnit || boardPosition.GetCard().unitType == "MilitaryUnit"))
            {
                var highlight = Instantiate(attackHighlight, boardPosition.transform.position, Quaternion.identity);
                newHighlights.Add(highlight);
            }
        }

        // ��������� ��������� ���� � ������� ����������, ���� ��� ������� ������
        if (!hasMilitaryUnit)
        {
            foreach (var siteObj in sacredSites)
            {
                var boardPosition = siteObj.GetComponent<BoardPosition>();
                if (boardPosition != null && boardPosition.isTempleBuilt && boardPosition.GetCard().templeOwner.Equals("Player1"))
                {
                    var highlight = Instantiate(attackHighlight, siteObj.transform.position, Quaternion.identity);
                    newHighlights.Add(highlight);
                }
            }
        }

        // ��������� ����� ���������
        currentHighlights = newHighlights.ToArray();
    }
    private void RemoveAllHighlights()
    {
        if (currentHighlights != null)
        {
            foreach (var highlight in currentHighlights)
            {
                Destroy(highlight);
            }
            currentHighlights = new GameObject[0]; // ������� ������ ����� �������� ���������
        }
    }
}
