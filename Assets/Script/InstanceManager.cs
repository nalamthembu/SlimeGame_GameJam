using UnityEngine;

public class InstanceManager : MonoBehaviour
{
    public static InstanceManager instance;

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

    public void PlaySound(string audioName, AudioSource source, bool loop = false, bool randomisePitch = false) => SoundManager.instance.PlaySound(audioName, source, loop, randomisePitch);
    public void PlaySound(string audioName, Vector3 position, bool randomisePitch = true) => SoundManager.instance.PlaySound(audioName, position, randomisePitch);

}