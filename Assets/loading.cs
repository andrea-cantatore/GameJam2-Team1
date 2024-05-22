using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;
    private float timer;

    [SerializeField]
    private float loadingScreenDuration = 3.0f;

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            timer = loadingScreenDuration;
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SceneManager.LoadScene(2);
            }
        }
    }
}