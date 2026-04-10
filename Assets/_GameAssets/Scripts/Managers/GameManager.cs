using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject gameWinPanel;
    public GameObject TutorialPanel;
    public GameObject gameLosePanel;
    public GameObject gamePausePanel;
    public float WaveStartDelay = 180f;
    public Slider MusicSlider;
    public Slider SFXSlider;
    public Slider MasterSlider;
    public Image TutorialImage;
    public Sprite[] Tutorials;
    public GameObject[] Tutorial_texts;
    private int currentTutorialIndex = 0;

    void Awake()
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
    void Tutorial()
    {
        TutorialPanel.SetActive(true);
        TutorialImage.sprite = Tutorials[0];
        Tutorial_texts[0].SetActive(true);
        Time.timeScale = 0f;
    }
    public void CloseTutorial()
    {
        TutorialPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void NextTutorialButton()
    {
        currentTutorialIndex+=1;
        NextTutorial(currentTutorialIndex);
    }
    void NextTutorial(int index)
    {
        if (index < Tutorials.Length)
        {
            TutorialImage.sprite = Tutorials[index];
            for (int i = 0; i < Tutorial_texts.Length; i++)
            {
                Tutorial_texts[i].SetActive(i == index);
            }
        }
        else
        {
            CloseTutorial();
        }
    }
    
    void Start()
    {
        Tutorial();
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);
        MasterSlider.onValueChanged.AddListener(
            v => SoundManager.Instance.SetVolume(v, "MasterVolume")
        );

        MusicSlider.onValueChanged.AddListener(
            v => SoundManager.Instance.SetVolume(v, "MusicVolume")
        );

        SFXSlider.onValueChanged.AddListener(
            v => SoundManager.Instance.SetVolume(v, "SFXVolume")
        );
        MasterSlider.value = master;
        MusicSlider.value = music;
        SFXSlider.value = sfx;
        SoundManager.Instance.ResumeMusic();
    }
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (gamePausePanel.activeSelf)
            {
                gamePausePanel.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                gamePausePanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }


    public void GameLose()
    {
        SoundManager.Instance.StopMusic();
        gameLosePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void GameWin()
    {
        SoundManager.Instance.StopMusic();
        gameWinPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void GameRestart()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    public void ClosePausePanel()
    {
        gamePausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

}
