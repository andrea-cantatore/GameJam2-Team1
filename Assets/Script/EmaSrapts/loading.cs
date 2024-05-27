using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LoaderCallback : MonoBehaviour
{
    private bool _isFirstUpdate = true;
    private float _timer;
    private AsyncOperation _asyncLoad;

    [SerializeField] private float _loadingScreenDuration = 3.0f;

    private void Start()
    {
        _asyncLoad = SceneManager.LoadSceneAsync(2);
        _asyncLoad.allowSceneActivation = false;
    }

    private void Update()
    {
        if (_isFirstUpdate)
        {
            _isFirstUpdate = false;
            _timer = _loadingScreenDuration;
        }

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _asyncLoad.allowSceneActivation = true;
            }
        }
    }
}