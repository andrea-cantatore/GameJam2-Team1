using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCicle : MonoBehaviour
{
    [SerializeField] private float _dayDuration;
    [SerializeField] private float _nightDuration;
    [SerializeField] private Texture2D skyboxNight;
    [SerializeField] private Texture2D skyboxDay;
    
    [SerializeField] private Gradient graddientDay;
    [SerializeField] private Gradient graddientNight;
 
    [SerializeField] private Light globalLight;
    private float _timer;
    private bool _isDay = true, _isStarted = true;


    private void OnEnable()
    {
        EventManager.StartNextDay += StartDay;
        EventManager.StartNextNight += StartNight;
    }
    
    private void OnDisable()
    {
        EventManager.StartNextDay -= StartDay;
        EventManager.StartNextNight -= StartNight;
    }
    
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_isDay)
        {
            if (_isStarted)
            {
                if (_timer >= _dayDuration)
                {
                    _isDay = false;
                    _timer = 0;
                    _isStarted = false;
                    EventManager.IsNight?.Invoke(true);
                }
            }
        }
        if (!_isDay)
        {
            if(_timer >= _nightDuration)
            {
                if (_isStarted)
                {
                    _isDay = true;
                    _timer = 0;
                    _isStarted = false;
                    EventManager.IsNight?.Invoke(false);
                }
            }
        }
        
    }
    
    private void StartNight()
    {
        StartCoroutine(LerpSkyboxes(skyboxDay, skyboxNight, 10f));
        StartCoroutine(LerpLight(graddientNight, 10f));
        _isStarted = true;
    }

    private void StartDay()
    {
        StartCoroutine(LerpSkyboxes(skyboxNight, skyboxDay, 20f));
        StartCoroutine(LerpLight(graddientDay, 20f));
        _isStarted = true;
    }

    private IEnumerator LerpSkyboxes(Texture2D a, Texture2D b, float time)
    {
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", i / time);
            yield return null;
        }
        RenderSettings.skybox.SetTexture("_Texture1", b);
    }
    
    private IEnumerator LerpLight(Gradient lightGradient, float time)
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            globalLight.color = lightGradient.Evaluate(i / time);
            RenderSettings.fogColor = globalLight.color;
            yield return null;
        }
    }
}
