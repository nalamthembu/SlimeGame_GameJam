using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceEnemyUI : MonoBehaviour
{
    new Camera camera;
    [SerializeField] private Slider worldSpaceHPBar;

    private Enemy linkedEnemy;

    private const string WORLD_SPACE_CANVAS = "CANVAS_WORLD_SPACE";

    private void Start()
    {
        Transform worldSpaceCanvas = GameObject.Find(WORLD_SPACE_CANVAS).transform;
        transform.SetParent(worldSpaceCanvas);
        camera = Camera.main;
    }

    private void LateUpdate()
    {
        worldSpaceHPBar.value = linkedEnemy.Health;
        worldSpaceHPBar.transform.position = linkedEnemy.transform.position + Vector3.up * (linkedEnemy.Agent.height + 0.25F); //slightly above enemy.
        worldSpaceHPBar.transform.LookAt(camera.transform);
    }

    public void LinkEnemy(Enemy enemy) => linkedEnemy = enemy;
}