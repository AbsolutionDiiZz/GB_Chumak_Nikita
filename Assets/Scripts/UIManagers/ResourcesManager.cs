using UnityEngine;
using TMPro;

// Управляет ресурсами игроков, включая золото, рвение и постройку храмов, а также обновляет UI.
public class ResourcesManager : MonoBehaviour
{
    // Используйте [SerializeField] для приватных полей, если они должны быть доступны в редакторе Unity.
    [SerializeField] private TextMeshProUGUI player0GoldText;
    [SerializeField] private TextMeshProUGUI player0ZealText;
    [SerializeField] public TextMeshProUGUI player0TempleCostText;
    [SerializeField] private TextMeshProUGUI player1GoldText;
    [SerializeField] private TextMeshProUGUI player1ZealText;
    [SerializeField] public TextMeshProUGUI player1TempleCostText;

    // Статический экземпляр для доступа к менеджеру ресурсов из других скриптов.
    public static ResourcesManager instance { get; private set; }

    // Ресурсы и статусы для обоих игроков.
    public int player0Gold = 0, player0Zeal = 0, player0TemplesBuilt = 0;
    public int player1Gold = 0, player1Zeal = 0, player1TemplesBuilt = 0;
    public bool canBuildTemplePlayer0 = true, canBuildTemplePlayer1 = true;

    private void Awake()
    {
        // Инициализация синглтона.
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        // Инициализация UI при старте.
        UpdateUI();
    }

    // Обновляет UI для всех ресурсов.
    private void UpdateUI()
    {
        UpdatePlayer0GoldText();
        UpdatePlayer0ZealText();
        UpdatePlayer1GoldText();
        UpdatePlayer1ZealText();
        UpdatePlayer0TempleCostText();
        UpdatePlayer1TempleCostText();
    }

    // Обновляет текст золота и рвения, а также стоимость храмов.
    private void UpdatePlayer0GoldText() => player0GoldText.text = player0Gold.ToString();
    private void UpdatePlayer1GoldText() => player1GoldText.text = player1Gold.ToString();
    private void UpdatePlayer0ZealText() => player0ZealText.text = player0Zeal.ToString();
    private void UpdatePlayer1ZealText() => player1ZealText.text = player1Zeal.ToString();

    // Методы для изменения ресурсов и автоматического обновления UI.
    public void ChangePlayer0Gold(int amount) { player0Gold += amount; UpdatePlayer0GoldText(); }
    public void ChangePlayer1Gold(int amount) { player1Gold += amount; UpdatePlayer1GoldText(); }
    public void ChangePlayer0Zeal(int amount) { player0Zeal += amount; UpdatePlayer0ZealText(); CheckWinCondition(); }
    public void ChangePlayer1Zeal(int amount) { player1Zeal += amount; UpdatePlayer1ZealText(); CheckWinCondition(); }

    // Проверка условия победы после каждого изменения рвения.
    private void CheckWinCondition() => WinLoseConditions.Instance.CheckWinCondition();

    // Вычисляет и обновляет стоимость храмов.
    public void UpdatePlayer0TempleCostText() => player0TempleCostText.text = $"Temple cost: {CalculatePlayer0TempleCost()} g/z";
    public void UpdatePlayer1TempleCostText() => player1TempleCostText.text = $"Temple cost: {CalculatePlayer1TempleCost()} g/z";
    public int CalculatePlayer0TempleCost() => 2 + (player0TemplesBuilt * 2);
    public int CalculatePlayer1TempleCost() => 2 + (player1TemplesBuilt * 2);
}