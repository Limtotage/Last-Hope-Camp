using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    [Header("Mixer")]
    public AudioMixer audioMixer;

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);
        _masterSlider.onValueChanged.AddListener(
            v => SetVolume(v,"MasterVolume")
        );

        _musicSlider.onValueChanged.AddListener(
            v => SetVolume(v,"MusicVolume")
        );

        _sfxSlider.onValueChanged.AddListener(
            v => SetVolume(v,"SFXVolume")
        );
    }
    public void SetVolume(float value, string Type)
    {
        audioMixer.SetFloat(Type, LinearToDB(value));
        PlayerPrefs.SetFloat(Type, value);
    }
    float LinearToDB(float value)
    {
        if (value <= 0.0001f)
            return -80f; // mute
        return Mathf.Log10(value) * 20f;
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void ResumeMusic()
    {
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
