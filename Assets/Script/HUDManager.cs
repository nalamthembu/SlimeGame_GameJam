using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] Slider playerHealthBar;
    [SerializeField] Slider megaJumpBar;
    [SerializeField] TMP_Text KillCountText;
    [SerializeField] TMP_Text HealthBarText;

    public static HUDManager instance;

    int killCount;
    Player Player;

    private void Awake()
    {
        if (instance is null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Player = FindObjectOfType<Player>();
    }

    private void LateUpdate()
    {
        SetValues();
    }

    private void SetValues()
    {

        playerHealthBar.value = Player.Health;
        HealthBarText.text = "HP : " + Player.Health;
        //Mega Jump Bar
    }

    public void AddToKillCount()
    {
        killCount++;
        KillCountText.text = "" + killCount;
    }
}
