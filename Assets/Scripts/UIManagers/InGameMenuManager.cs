using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    // Панели
    public GameObject inGameMenuPanel;
    public GameObject pauseMenuPanel;
    // ScreenLock
    public GameObject pauseMenuScreenLock;

    // Метод для обработки нажатия кнопки паузы в игровом меню
    public void OnPauseButtonClicked()
    {
        pauseMenuPanel.SetActive(true); // Показываем панель паузы
        inGameMenuPanel.SetActive(false); // Скрываем игровое меню
        pauseMenuScreenLock.SetActive(true); // открываем screenlock
    }

    // Метод для обработки нажатия кнопки возобновления игры в меню паузы
    public void OnResumeButtonClicked()
    {
        inGameMenuPanel.SetActive(true); // Показываем игровое меню
        pauseMenuPanel.SetActive(false); // Скрываем меню паузы
        pauseMenuScreenLock.SetActive(false); // закрываем screenlock
    }

    // Метод для обработки нажатия кнопки перезапуска игры
    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Перезагрузка текущей сцены
    }

    // Метод для обработки нажатия кнопки выхода из игры (загрузка главного меню)
    public void OnExitButtonClicked()
    {
        SceneManager.LoadScene(0); // Загрузка сцены с индексом 0
    }
    public void OnEffectsButtonClicked()
    {
        AudioManager.Instance.ToggleEffects(); // Переключаем звуковые эффекты
    }

    public void OnMusicButtonClicked()
    {
        AudioManager.Instance.ToggleMusic(); // Переключаем музыку
    }
}