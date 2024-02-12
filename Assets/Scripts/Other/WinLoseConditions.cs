using UnityEngine;

public class WinLoseConditions : MonoBehaviour
{
    // ������ �� �������� ����������: ���� � ����, ���������� ������, ���� ������ � ���������.
    [SerializeField] private GameObject inGameMenu;
    [SerializeField] private GameObject screenLock;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject loseMenu;

    public static WinLoseConditions Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckWinCondition() // �������� ���������� ������� ������� ������ (����������� ����� ��� ���� ������).
    {
        if (ResourcesManager.instance.player0TemplesBuilt >= 6 || ResourcesManager.instance.player0Zeal >= 40)
        {
            PlayerWins();
        }
        else if (ResourcesManager.instance.player1TemplesBuilt >= 6 || ResourcesManager.instance.player1Zeal >= 40)
        {
            PlayerLoses();
        }
    }

    private void PlayerWins() // ��������� ��������� ������ ������.
    {
        SetGameEndState(true);
    }

    private void PlayerLoses() // ��������� ��������� ��������� ������.
    {
        SetGameEndState(false);
    }
    private void SetGameEndState(bool isWin) // ��������� ��������� ���������� � ����������� �� ������ ����.
    {
        inGameMenu.SetActive(false);
        screenLock.SetActive(true);
        winMenu.SetActive(isWin);
        loseMenu.SetActive(!isWin);
    }
}