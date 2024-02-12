using UnityEngine;
using UnityEngine.SceneManagement; // ��� ������� � SceneManager
using TMPro; // ��� ������ � TextMeshPro
using System.Collections;
using UnityEditor;

public class MainMenuManager : MonoBehaviour
{
    public static string currentConfession = "Asatru"; // ����������� ���������� ��� �������� ������� ���������

    public TextMeshProUGUI effectsVolumeValueText; // ��������� ���� ��� ��������� ��������
    public TextMeshProUGUI musicVolumeValueText; // ��������� ���� ��� ��������� ������

    // ������ �� ��������� � ������� �����
    public Animator gameMenuAnimator;
    public Animator gameTitleAnimator;
    public Animator settingsMenuAnimator;
    public Animator playMenuAnimator;
    public Animator setBattleMenuAnimator;

    // ������ �� ������� Asatru
    public GameObject asatruMainPicture;
    public GameObject asatruConfessionDescriptionPicture;
    public GameObject asatruLokiGodCanvas;
    public GameObject asatruSifGodCanvas;
    public GameObject asatruConfessionCanvas;
    public GameObject asatruLokiTitleCanvas;
    public GameObject asatruSifTitleCanvas;

    // ������ �� ������� Hellenism
    public GameObject hellenismMainPicture;
    public GameObject hellenismConfessionDescriptionPicture;
    public GameObject hellenismAresGodCanvas;
    public GameObject hellenismHermesGodCanvas;
    public GameObject hellenismConfessionCanvas;
    public GameObject hellenismAresTitleCanvas;
    public GameObject hellenismHermesTitleCanvas;

    private bool isAnimating = false; // ���� ��� ������������ ��������

    IEnumerator ResetAnimationFlag()
    {
        yield return new WaitForSeconds(1);
        isAnimating = false;
    }

    // ������ ��� ��������� ������� �� ������
    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OnSettingsButtonClicked()
    {
        if (isAnimating) return; // ���� ��� ������������� ��������, ������� �� ������
        isAnimating = true; // ������������� ���� � true

        // ������� ������� ��������, ������� ����� �������������
        gameMenuAnimator.ResetTrigger("TriggerGameMenuShow");
        gameTitleAnimator.ResetTrigger("TriggerGameTitleShow");
        settingsMenuAnimator.ResetTrigger("TriggerSettingsMenuHide");

        // ������ ��������� ������ �������� ��� ��������
        gameMenuAnimator.SetTrigger("TriggerGameMenuHide");
        gameTitleAnimator.SetTrigger("TriggerGameTitleHide");
        settingsMenuAnimator.SetTrigger("TriggerSettingsMenuShow");

        StartCoroutine(ResetAnimationFlag());
    }

    public void OnPlayButtonClicked()
    {
        if (isAnimating) return; // ���� ��� ������������� ��������, ������� �� ������
        isAnimating = true; // ������������� ���� � true

        // ������� ������� ��������, ������� ����� �������������
        gameMenuAnimator.ResetTrigger("TriggerGameMenuShow");
        gameTitleAnimator.ResetTrigger("TriggerGameTitleShow");
        playMenuAnimator.ResetTrigger("TriggerPlayMenuHide");

        // ������ ��������� ������ �������� ��� ��������
        gameMenuAnimator.SetTrigger("TriggerGameMenuHide");
        gameTitleAnimator.SetTrigger("TriggerGameTitleHide");
        playMenuAnimator.SetTrigger("TriggerPlayMenuShow");

        StartCoroutine(ResetAnimationFlag());
    }

    public void OnLessMusicVolumeClicked()
    {
        AudioManager.Instance.AdjustMusicVolume(Mathf.Max(0, AudioManager.Instance.musicSource.volume - 0.1f));
        musicVolumeValueText.text = Mathf.Round(AudioManager.Instance.musicSource.volume * 100).ToString();
    }

    public void OnMoreMusicVolumeClicked()
    {
        AudioManager.Instance.AdjustMusicVolume(Mathf.Min(1, AudioManager.Instance.musicSource.volume + 0.1f));
        musicVolumeValueText.text = Mathf.Round(AudioManager.Instance.musicSource.volume * 100).ToString();
    }

    public void OnLessEffectsVolumeClicked()
    {
        AudioManager.Instance.AdjustEffectsVolume(Mathf.Max(0, AudioManager.Instance.effectsSource.volume - 0.1f));
        effectsVolumeValueText.text = Mathf.Round(AudioManager.Instance.effectsSource.volume * 100).ToString();
    }

    public void OnMoreEffectsVolumeClicked()
    {
        AudioManager.Instance.AdjustEffectsVolume(Mathf.Min(1, AudioManager.Instance.effectsSource.volume + 0.1f));
        effectsVolumeValueText.text = Mathf.Round(AudioManager.Instance.effectsSource.volume * 100).ToString();
    }

    public void OnSettingsBackClicked()
    {
        if (isAnimating) return; // ���� ��� ������������� ��������, ������� �� ������
        isAnimating = true; // ������������� ���� � true

        // ����� ���������� ���������
        gameMenuAnimator.ResetTrigger("TriggerGameMenuHide");
        gameTitleAnimator.ResetTrigger("TriggerGameTitleHide");
        settingsMenuAnimator.ResetTrigger("TriggerSettingsMenuShow");

        // ��������� ��������� ��� �������� ��������
        gameMenuAnimator.SetTrigger("TriggerGameMenuShow");
        gameTitleAnimator.SetTrigger("TriggerGameTitleShow");
        settingsMenuAnimator.SetTrigger("TriggerSettingsMenuHide");

        StartCoroutine(ResetAnimationFlag());
    }

    public void OnBattleButtonClicked()
    {
        if (isAnimating) return; // ���� ��� ������������� ��������, ������� �� ������
        isAnimating = true; // ������������� ���� � true

        // ������� ������� ��������, ������� ����� �������������
        playMenuAnimator.ResetTrigger("TriggerPlayMenuShow");
        setBattleMenuAnimator.ResetTrigger("TriggerSetBattleMenuHide");

        // ������ ��������� ������ �������� ��� ��������
        playMenuAnimator.SetTrigger("TriggerPlayMenuHide");
        setBattleMenuAnimator.SetTrigger("TriggerSetBattleMenuShow");

        StartCoroutine(ResetAnimationFlag());
    }

    public void OnPlayBackButtonClicked()
    {
        if (isAnimating) return; // ���� ��� ������������� ��������, ������� �� ������
        isAnimating = true; // ������������� ���� � true

        gameMenuAnimator.ResetTrigger("TriggerGameMenuHide");
        gameTitleAnimator.ResetTrigger("TriggerGameTitleHide");
        playMenuAnimator.ResetTrigger("TriggerPlayMenuShow");

        // ��������� ��������� ��� �������� ��������
        gameMenuAnimator.SetTrigger("TriggerGameMenuShow");
        gameTitleAnimator.SetTrigger("TriggerGameTitleShow");
        playMenuAnimator.SetTrigger("TriggerPlayMenuHide");

        StartCoroutine(ResetAnimationFlag());
    }

    public void OnPreviousConfessionButtonClicked()
    {
        currentConfession = "Asatru";
        // ���������� ������� Asatru
        asatruMainPicture.SetActive(true);
        asatruConfessionDescriptionPicture.SetActive(true);
        asatruLokiGodCanvas.SetActive(true);
        asatruSifGodCanvas.SetActive(true);
        asatruConfessionCanvas.SetActive(true);
        asatruLokiTitleCanvas.SetActive(true);
        asatruSifTitleCanvas.SetActive(true);

        // �������� ������� Hellenism
        hellenismMainPicture.SetActive(false);
        hellenismConfessionDescriptionPicture.SetActive(false);
        hellenismAresGodCanvas.SetActive(false);
        hellenismHermesGodCanvas.SetActive(false);
        hellenismConfessionCanvas.SetActive(false);
        hellenismAresTitleCanvas.SetActive(false);
        hellenismHermesTitleCanvas.SetActive(false);
    }

    public void OnNextConfessionButtonClicked()
    {
        currentConfession = "Hellenism";    
        // �������� ������� Asatru
        asatruMainPicture.SetActive(false);
        asatruConfessionDescriptionPicture.SetActive(false);
        asatruLokiGodCanvas.SetActive(false);
        asatruSifGodCanvas.SetActive(false);
        asatruConfessionCanvas.SetActive(false);
        asatruLokiTitleCanvas.SetActive(false);
        asatruSifTitleCanvas.SetActive(false);

        // ���������� ������� Hellenism
        hellenismMainPicture.SetActive(true);
        hellenismConfessionDescriptionPicture.SetActive(true);
        hellenismAresGodCanvas.SetActive(true);
        hellenismHermesGodCanvas.SetActive(true);
        hellenismConfessionCanvas.SetActive(true);
        hellenismAresTitleCanvas.SetActive(true);
        hellenismHermesTitleCanvas.SetActive(true);
    }

    public void OnBattleBackButtonClicked()
    {
        if (isAnimating) return; // ���� ��� ������������� ��������, ������� �� ������
        isAnimating = true; // ������������� ���� � true

        playMenuAnimator.ResetTrigger("TriggerPlayMenuHide");
        setBattleMenuAnimator.ResetTrigger("TriggerSetBattleMenuShow");

        // ��������� ��������� ��� �������� ��������
        playMenuAnimator.SetTrigger("TriggerPlayMenuShow");
        setBattleMenuAnimator.SetTrigger("TriggerSetBattleMenuHide");

        StartCoroutine(ResetAnimationFlag());
    }

    public void OnStartBattleButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
}
