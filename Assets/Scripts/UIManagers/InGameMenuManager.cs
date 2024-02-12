using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    // ������
    public GameObject inGameMenuPanel;
    public GameObject pauseMenuPanel;
    // ScreenLock
    public GameObject pauseMenuScreenLock;

    // ����� ��� ��������� ������� ������ ����� � ������� ����
    public void OnPauseButtonClicked()
    {
        pauseMenuPanel.SetActive(true); // ���������� ������ �����
        inGameMenuPanel.SetActive(false); // �������� ������� ����
        pauseMenuScreenLock.SetActive(true); // ��������� screenlock
    }

    // ����� ��� ��������� ������� ������ ������������� ���� � ���� �����
    public void OnResumeButtonClicked()
    {
        inGameMenuPanel.SetActive(true); // ���������� ������� ����
        pauseMenuPanel.SetActive(false); // �������� ���� �����
        pauseMenuScreenLock.SetActive(false); // ��������� screenlock
    }

    // ����� ��� ��������� ������� ������ ����������� ����
    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ������������ ������� �����
    }

    // ����� ��� ��������� ������� ������ ������ �� ���� (�������� �������� ����)
    public void OnExitButtonClicked()
    {
        SceneManager.LoadScene(0); // �������� ����� � �������� 0
    }
    public void OnEffectsButtonClicked()
    {
        AudioManager.Instance.ToggleEffects(); // ����������� �������� �������
    }

    public void OnMusicButtonClicked()
    {
        AudioManager.Instance.ToggleMusic(); // ����������� ������
    }
}