using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCicle : MonoBehaviour
{
    [SerializeField] private float _dayDuration;
    [SerializeField] private float _nightDuration;
    public int DayCount = 0;
    [SerializeField] private Texture2D skyboxNight;
    [SerializeField] private Texture2D skyboxDay;

    [SerializeField] private Gradient graddientDay;
    [SerializeField] private Gradient graddientNight;
    public int _tax;
    
    [SerializeField] private Light globalLight;
    private float _timer;
    private bool _isDay = true, _isStarted = true;
    
    [SerializeField] private AudioData _audioData;
    
    
    public static DayNightCicle Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    

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
        if (!_isStarted)
        {
            _timer = 0;
            return;
        }
        if (_isDay && _isStarted)
        {
            if (_timer >= _dayDuration)
            {
                _isDay = false;
                _timer = 0;
                _isStarted = false;
                EventManager.IsNight?.Invoke(true);
            }
            else
            {
                float rotationAngle = Mathf.Lerp(0, 360, _timer / _dayDuration);
                globalLight.transform.rotation = Quaternion.Euler(rotationAngle, 0, 0);
                _timer += Time.deltaTime;
            }
        }
    }

    private void StartNight()
    {
        if (DayCount == 2)
        {
            EventManager.OnWin?.Invoke();
            Time.timeScale = 0;
        }
        StartCoroutine(LerpSkyboxes(skyboxDay, skyboxNight, _nightDuration));
        StartCoroutine(LerpLight(graddientNight, _nightDuration));
        _isStarted = true;
    }

    private void StartDay()
    {
        EventManager.MoneyChanger.Invoke(-(_tax * (DayCount + 1)));
        
        if(UIManager.Instance.MoneyCounterInt <= 0)
        {
            EventManager.OnLose?.Invoke();
            Time.timeScale = 0;
        }
        StartCoroutine(LerpSkyboxes(skyboxNight, skyboxDay, _dayDuration));
        StartCoroutine(LerpLight(graddientDay, _dayDuration));
        _isDay = true;
        _isStarted = true;
        _timer = 0;
        DayCount++;
        EventManager.IsNight?.Invoke(false);
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
