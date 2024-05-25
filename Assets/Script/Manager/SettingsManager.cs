using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement; //ByEma
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private TMP_Dropdown _resolutionSettings;
    private Resolution[] _resolutions;
    
    private void Start()
    {
        _resolutions = Screen.resolutions;
        
        _resolutionSettings.ClearOptions();

        List<string> options = new List<string>();

        int wantedResIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);
            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                wantedResIndex = i;
            }
        }
        
        _resolutionSettings.AddOptions(options);
        _resolutionSettings.value = wantedResIndex;
        _resolutionSettings.RefreshShownValue();
    }
    public void SetResolution(int index)
    {
        Resolution resolution = _resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        _audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetFullscreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }

    //ByEmaStart
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    //ByEmaEnd
}
