using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour // Управляет воспроизведением звуков и музыки в игре.
{
    public static AudioManager Instance;

    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource effectsSource;

    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip mainTheme;
    [SerializeField] private AudioClip asatruTheme;
    [SerializeField] private AudioClip hellenismTheme;

    private void Awake() // Воспроизведение музыки меню при запуске.
    {
        InitializeSingleton();
        PlayBackgroundMusic(mainTheme);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        switch (scene.buildIndex)
        {
            case 1: // Game scene
                PlayGameSceneMusic();
                break;
            case 0: // Menu scene
                PlayBackgroundMusic(mainTheme);
                break;
        }
    }

    private void PlayGameSceneMusic() // Выбор и воспроизведение музыки для игровой сцены в зависимости от конфессии.
    {
        AudioClip themeToPlay = MainMenuManager.currentConfession == "Hellenism" ? hellenismTheme : asatruTheme;
        PlayBackgroundMusic(themeToPlay);
    }

    public void PlayClickSound() // Воспроизведение звука клика.
    {
        effectsSource.PlayOneShot(clickSound);
    }

    public void PlayBackgroundMusic(AudioClip musicClip) // Воспроизведение музыкальной темы.
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void AdjustMusicVolume(float volume) // Регулировка громкости музыки.
    {
        musicSource.volume = volume;
    }

    public void AdjustEffectsVolume(float volume) // Регулировка громкости звуковых эффектов.
    {
        effectsSource.volume = volume;
    }

    public void ToggleMusic() // Переключение включения/выключения музыки.
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleEffects() // Переключение включения/выключения звуковых эффектов.
    {
        effectsSource.mute = !effectsSource.mute;
    }
}
