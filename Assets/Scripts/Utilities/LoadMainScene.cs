using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainScene : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1);
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(1);

        while (!loadScene.isDone)
            yield return null;

        Destroy(gameObject);
    }
}