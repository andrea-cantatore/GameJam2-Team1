using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    private float timer = 0f;
    private float timeToLoad = 1f;

    public void LoadScene(int SceneIndex)
    {
        StartCoroutine(LoadSceneASync(SceneIndex));
    }
    
    private IEnumerator LoadSceneASync(int SceneIndex)
    {
        if(timer < timeToLoad)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneIndex);
        Debug.Log(operation.progress);
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
