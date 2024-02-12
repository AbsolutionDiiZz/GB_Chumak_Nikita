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
        // Обработка клика мышью
        if (Input.GetMouseButtonDown(0)) // Проверка нажатия левой кнопки мыши
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
        // Получаем BoardPosition из targetPosition, а не из текущего объекта
        BoardPosition boardPosition = targetPosition.GetComponent<BoardPosition>();
        // Инициализируем prefabToUse как null
        GameObject prefabToUse = null;

        if (boardPosition != null)
        {
            // Проверяем, построен ли храм на связанной позиции
            if (boardPosition.isTempleBuilt)
            {
                Card templeCard = boardPosition.GetCard(); // Получаем карту храма
                if (templeCard != null)
                {
                    // Определяем, какой префаб использовать на основе владельца и usageCount
                    string message;
                    if (templeCard.usageCount > 0 && templeCard.templeOwner == turnManagerScript.currentTurn.ToString())
                    {
                        prefabToUse = sacredCanUseHighlight; // Можно использовать храм
                        message = $"Храм принадлежит {templeCard.templeOwner} и имеет использования.";
                    }
                    else
                    {
                        prefabToUse = sacredCantUseHighlight; // Нельзя использовать храм
                        message = $"Храм принадлежит {templeCard.templeOwner}. Нет использований или храм принадлежит другому игроку.";
                    }
                }
            }
            else
            {
                // Проверяем, может ли игрок построить храм
                if (ResourcesManager.instance.canBuildTemplePlayer0)
                {
                    prefabToUse = sacredEmptyHighlight; // Показываем подсветку для возможности постройки
                }
            }
        }

        // Создаём подсветку, если был выбран префаб
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
                // Определение, какой префаб подсветки использовать на основе типа и состояния юнита
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
            // Подсветка для карт в руке игрока
            if (gameObject.name.StartsWith("Player0HandClicker"))
            {
                HandlePlayerHandHighlight();
            }
            // Подсветка для священных мест
            else if (gameObject.name.StartsWith("SacredSiteClicker"))
            {
                HandleSacredSiteHighlight();
            }
            // Подсветка для карт магазина
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
        // Удаляем предыдущие подсветки, если они есть
        if (currentHighlights != null)
        {
            foreach (var highlight in currentHighlights)
            {
                Destroy(highlight);
            }
        }

        // Находим все позиции для руки и юнитов игрока
        var playerHandPositions = GameObject.FindGameObjectsWithTag("Player0HandPosition");
        var playerUnitPositions = GameObject.FindGameObjectsWithTag("Player0UnitPosition");

        // Объединяем массивы
        var allPositions = new GameObject[playerHandPositions.Length + playerUnitPositions.Length];
        playerHandPositions.CopyTo(allPositions, 0);
        playerUnitPositions.CopyTo(allPositions, playerHandPositions.Length);

        // Создаем список для хранения новых подсветок
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

        // Сохраняем новые подсветки
        currentHighlights = newHighlights.ToArray();
    }
    private void ShowAttackHighlightForEnemyPositions()
    {
        // Удаляем предыдущие подсветки
        RemoveAllHighlights();

        // Находим все позиции юнитов противника
        var enemyUnitPositions = GameObject.FindGameObjectsWithTag("Player1UnitPosition");
        var sacredSites = GameObject.FindGameObjectsWithTag("SacredSite");

        // Создаём список для новых подсветок
        var newHighlights = new List<GameObject>();
        bool hasMilitaryUnit = false;

        // Проверяем наличие военных юнитов у противника
        foreach (var positionObj in enemyUnitPositions)
        {
            var boardPosition = positionObj.GetComponent<BoardPosition>();
            if (boardPosition != null && boardPosition.IsOccupied && boardPosition.GetCard().unitType == "MilitaryUnit")
            {
                hasMilitaryUnit = true;
                break;
            }
        }

        // Подсветка позиций юнитов противника
        foreach (var positionObj in enemyUnitPositions)
        {
            var boardPosition = positionObj.GetComponent<BoardPosition>();
            if (boardPosition != null && boardPosition.IsOccupied && (!hasMilitaryUnit || boardPosition.GetCard().unitType == "MilitaryUnit"))
            {
                var highlight = Instantiate(attackHighlight, boardPosition.transform.position, Quaternion.identity);
                newHighlights.Add(highlight);
            }
        }

        // Подсветка священных мест с храмами противника, если нет военных юнитов
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

        // Сохраняем новые подсветки
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
            currentHighlights = new GameObject[0]; // Очищаем массив после удаления подсветок
        }
    }
}
