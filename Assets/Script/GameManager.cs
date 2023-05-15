using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
}
