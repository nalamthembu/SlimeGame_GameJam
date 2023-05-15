using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceEnemyUI : MonoBehaviour
{
    new Camera camera;
    [SerializeField] private Slider worldSpaceHPBar;

    private const string WORLD_SPACE_CANVAS = "CANVAS_WORLD_SPACE";

    public Enemy LinkedEnemy { get; private set; }

    private void Start()
    {
        Transform worldSpaceCanvas = GameObject.Find(WORLD_SPACE_CANVAS).transform;
        transform.SetParent(worldSpaceCanvas);
        camera = Camera.main;
    }

    private void LateUpdate()
    {
        worldSpaceHPBar.value = LinkedEnemy.Health;
        worldSpaceHPBar.transform.position = LinkedEnemy.transform.position + Vector3.up * (LinkedEnemy.Agent.height + 0.25F); //slightly above enemy.
        worldSpaceHPBar.transform.LookAt(camera.transform);
    }

    public void LinkEnemy(Enemy enemy)
    {
        LinkedEnemy = enemy;
    }
}