using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] Slider playerHealthBar;
    [SerializeField] Slider megaJumpBar;
    [SerializeField] TMP_Text KillCountText;
    [SerializeField] TMP_Text HealthBarText;

    Player Player;

    private void Awake()
    {
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

    public void SetKillCount(int count)
    {
        KillCountText.text = "" + count;
    }
}
