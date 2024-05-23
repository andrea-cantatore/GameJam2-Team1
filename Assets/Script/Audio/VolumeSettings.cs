
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer Mixer;
    [SerializeField] Slider MasterSlider;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SFXSlider;

    public const string MIXER_MASTER = "MasterVolume";
    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";

    private void Awake()
    {
        MasterSlider.onValueChanged.AddListener(SetMasterVolume);
        MusicSlider.onValueChanged.AddListener(SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void Start()
    {
        MasterSlider.value = PlayerPrefs.GetFloat(AudioManager.MASTER_KEY, 0.5f);
        MusicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 0.5f);
        SFXSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 0.5f);
    }

    void SetMasterVolume(float value)
    {
        Mixer.SetFloat(MIXER_MASTER, Mathf.Log10(value) * 20);
    }

    void SetMusicVolume(float value)
    {
        Mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }

    void SetSFXVolume(float value)
    {
        Mixer.SetFloat(MIXER_SFX,Mathf.Log10(value) * 20);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MASTER_KEY, MasterSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY,MusicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY,SFXSlider.value);
    }

}
