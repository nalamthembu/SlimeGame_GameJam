using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    public void ExitApplication()
    {
#if UNITY_EDITOR

        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }
#endif

        Application.Quit();
    }

    private bool InGame;

    private bool paused;

    private void FixedUpdate()
    {
        //LEVEL ONE IS HARD CODED.
        InGame = SceneManager.GetActiveScene().buildIndex == 1;

        if (InGame)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                paused = !paused;
            }

            if (paused)
            {
                InstanceManager.instance.ShowPauseMenu();
                PauseGame();
            }
            else
            {
                InstanceManager.instance.HidePauseMenu();
                ResumeGame();
            }
        }
        else
        {
            paused = false;
            ResumeGame();
        }
    }

    public void PauseGame() => Time.timeScale = 0;
    public void ResumeGame() => Time.timeScale = 1;
    public void ForceUnPause() => paused = false;
}
