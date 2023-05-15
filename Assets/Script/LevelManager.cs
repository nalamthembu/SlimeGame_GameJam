using UnityEngine;
using UnityEngine.UI;
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
        UI_MANAGER.FadeScreenToBlack();
        yield return new WaitForSeconds(2);
        m_LoadingScreen.SetActive(true);
        UI_MANAGER.FadeScreenToClear();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            m_LoadingBar.value = progress;
            yield return null;
        }

        yield return new WaitForSeconds(2);

        UI_MANAGER.CutToBlack();

        m_LoadingScreen.SetActive(false);

        yield return new WaitForSeconds(1);

        UI_MANAGER.FadeScreenToClear();

    }
}
