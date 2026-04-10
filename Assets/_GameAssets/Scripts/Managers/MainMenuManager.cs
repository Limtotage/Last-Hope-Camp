using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _creditsPanel;
    [Header("Sliders")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    [Header("Icons")]
    [SerializeField] private Image _masterIcon;
    [SerializeField] private Image _musicIcon;
    [SerializeField] private Image _sfxIcon;
    [SerializeField] private AudioClip gamemusic;

    [Header("Sprites (Mute / Low / High)")]
    [SerializeField] private List<Sprite> volumeSprites;

    const float MUTED_ALPHA = 160f / 255f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        SoundManager.Instance.PlayMusic(gamemusic);
        _masterSlider.onValueChanged.AddListener(
            v => UpdateVolumeIcon(v, _masterIcon, true)
        );

        _musicSlider.onValueChanged.AddListener(
            v => UpdateVolumeIcon(v, _musicIcon, false)
        );

        _sfxSlider.onValueChanged.AddListener(
            v => UpdateVolumeIcon(v, _sfxIcon, false)
        );
        UpdateVolumeIcon(_masterSlider.value, _masterIcon, true);
        UpdateVolumeIcon(_musicSlider.value, _musicIcon, false);
        UpdateVolumeIcon(_sfxSlider.value, _sfxIcon, false);
    }

    void UpdateVolumeIcon(float value, Image icon, bool hasLevels)
    {
        Color c = icon.color;

        if (value <= 0.001f)
        {
            c.a = MUTED_ALPHA;
            if (hasLevels) icon.sprite = volumeSprites[0];
        }
        else
        {
            c.a = 1f;
            if (hasLevels)
            {
                icon.sprite = value <= 0.5f ? volumeSprites[1] : volumeSprites[2];
            }
        }
        icon.color = c;
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);//Change for your work.
    }
    public void ResumeGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentMaxLevel"));// Change Key or save same Key
    }
    public void OptionsButton()
    {
        _optionsPanel.SetActive(true);
    }
    public void CloseOptionsPanel()
    {
        _optionsPanel.SetActive(false);
    }
    public void CreditsButton()
    {
        _creditsPanel.SetActive(true);
    }
    public void CloseCreditsPanel()
    {
        _creditsPanel.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
