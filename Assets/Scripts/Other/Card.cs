using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class Card : MonoBehaviour // Описывает поведение и характеристики карты.
{
    [Header("Card Properties")]
    public string cardName;
    public string unitType;
    public string templeType;
    public string templeGodType;
    public string templeOwner;
    public string confessionType;
    public string activationType;
    public int cost;
    public int usageCount;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI cardCostText;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardDescriptionText;
    [SerializeField] private TextMeshProUGUI cardConfessionTypeText;
    [SerializeField] private TextMeshProUGUI cardUnitTypeText;
    [SerializeField] private TextMeshProUGUI cardActivationTypeText;


    void Awake()
    {
        SetCardTypes();
        SetCardCost();
        ExtractCardName();
        UpdateCardTextFields();
    }

    private void SetCardTypes() // Устанавливает свойства карты на основе её имени.
    {
        if (gameObject.name.Contains("temple"))
        {
            if (gameObject.name.Contains("Name"))
            {
                Match match = Regex.Match(gameObject.name, "Name(.*?)( |$)");
                if (match.Success)
                {
                    cardName = match.Groups[1].Value;
                    templeGodType = cardName;
                }
            }
            templeType = "Temple";
        }
        else
        {
            templeType = "NonTemple";
        }

        if (gameObject.name.Contains("Asatru"))
        {
            confessionType = "Asatru";
        }
        else if (gameObject.name.Contains("Hellenism"))
        {
            confessionType = "Hellenism";
        }
        else
        {
            confessionType = "Neutral";
        }

        if (gameObject.name.Contains("Activation"))
        {
            activationType = "Activation";
        }
        else
        {
            activationType = "NonActivation";
        }

        if (gameObject.name.Contains("MilitaryUnit"))
        {
            unitType = "MilitaryUnit";
        }
        else if (gameObject.name.Contains("PeacefulUnit"))
        {
            unitType = "PeacefulUnit";
        }
        else
        {
            unitType = "NonUnit";
        }
    }
    private void ExtractCardName()
    {
        if (gameObject.name.Contains("Name"))
        {
            Match match = Regex.Match(gameObject.name, "Name([A-Z][^A-Z]*)");
            if (match.Success)
            {
                cardName = match.Groups[1].Value;
            }
        }
    }
    private void SetCardCost() // Устанавливает свойства карты на основе её имени.
    {
        if (gameObject.name.Contains("Cost"))
        {
            string costStr = System.Text.RegularExpressions.Regex.Match(gameObject.name, "Cost(\\d+)").Groups[1].Value;
            cost = int.Parse(costStr);
        }
        else
        {
            cost = 0;
        }
    }
    public void ActivateCardEffect() // Активирует эффект карты.
    {
        TurnManager turnManager = FindObjectOfType<TurnManager>(); 

        if (this.gameObject.name.Contains("GoldCoin"))
        {
            if (turnManager.currentTurn == TurnManager.Turn.Player0)
            {
                ResourcesManager.instance.ChangePlayer0Gold(1);
            }
            else if (turnManager.currentTurn == TurnManager.Turn.Player1)
            {
                ResourcesManager.instance.ChangePlayer1Gold(1); 
            }
        }
        if (this.gameObject.name.Contains("NameAsatru god1N"))
        {
            if (turnManager.currentTurn == TurnManager.Turn.Player0)
            {
                ResourcesManager.instance.ChangePlayer0Zeal(1);
            }
            else if (turnManager.currentTurn == TurnManager.Turn.Player1)
            {
                ResourcesManager.instance.ChangePlayer1Zeal(1);
            }
        }
        if (this.gameObject.name.Contains("NameAsatru god2N"))
        {
            if (turnManager.currentTurn == TurnManager.Turn.Player0)
            {
                ResourcesManager.instance.ChangePlayer0Zeal(2);
            }
            else if (turnManager.currentTurn == TurnManager.Turn.Player1)
            {
                ResourcesManager.instance.ChangePlayer1Zeal(2);
            }
        }
        if (this.gameObject.name.Contains("NameHellenism god1N"))
        {
            if (turnManager.currentTurn == TurnManager.Turn.Player0)
            {
                ResourcesManager.instance.ChangePlayer0Zeal(1);
            }
            else if (turnManager.currentTurn == TurnManager.Turn.Player1)
            {
                ResourcesManager.instance.ChangePlayer1Zeal(1);
            }
        }
        if (this.gameObject.name.Contains("NameHellenism god2N"))
        {
            if (turnManager.currentTurn == TurnManager.Turn.Player0)
            {
                ResourcesManager.instance.ChangePlayer0Zeal(2);
            }
            else if (turnManager.currentTurn == TurnManager.Turn.Player1)
            {
                ResourcesManager.instance.ChangePlayer1Zeal(2);
            }
        }
    }
    private void UpdateCardTextFields() // Обновляет текстовые поля UI, отображающие информацию о карте.
    {
        if (cardCostText) cardCostText.text = $"{cost}";
        if (cardNameText) cardNameText.text = cardName;
        if (cardDescriptionText) cardDescriptionText.text = "Card description here";
        if (cardConfessionTypeText) cardConfessionTypeText.text = $"{confessionType}";
        if (cardUnitTypeText) cardUnitTypeText.text = $"{unitType}";
        if (cardActivationTypeText) cardActivationTypeText.text = $"{activationType}";
    }
    public void SetUsageCount(int newUsageCount) // Устанавливает количество использований карты.
    {
        usageCount = newUsageCount;
    }
}