using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] GameObject loadingScreen;
    [SerializeField] Slider loadingBar;

    private void Awake()
    {
        if (instance is null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        UIManager.instance.FadeScreenToBlack();
        yield return new WaitForSeconds(2);
        loadingScreen.SetActive(true);
        UIManager.instance.FadeScreenToClear();
        yield return new WaitForSeconds(2);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingBar.value = progress;
            yield return null;
        }

        yield return new WaitForSeconds(2);

        UIManager.instance.CutToBlack();

        loadingScreen.SetActive(false);

        yield return new WaitForSeconds(1);

        UIManager.instance.FadeScreenToClear();

    }
}
