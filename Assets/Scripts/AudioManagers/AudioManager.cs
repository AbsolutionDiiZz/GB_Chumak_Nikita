using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource effectsSource;

    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip mainTheme;
    [SerializeField] private AudioClip asatruTheme;
    [SerializeField] private AudioClip hellenismTheme;

    private void Awake()
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

    private void PlayGameSceneMusic()
    {
        AudioClip themeToPlay = MainMenuManager.currentConfession == "Hellenism" ? hellenismTheme : asatruTheme;
        PlayBackgroundMusic(themeToPlay);
    }

    public void PlayClickSound()
    {
        effectsSource.PlayOneShot(clickSound);
    }

    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void AdjustMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void AdjustEffectsVolume(float volume)
    {
        effectsSource.volume = volume;
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleEffects()
    {
        effectsSource.mute = !effectsSource.mute;
    }
}