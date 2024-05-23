using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LoaderCallback : MonoBehaviour
{
    private bool _isFirstUpdate = true;
    private float _timer;

    [SerializeField] private float _loadingScreenDuration = 3.0f;

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
                SceneManager.LoadScene(2);
            }
        }
    }
}