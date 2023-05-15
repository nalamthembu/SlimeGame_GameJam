using UnityEngine;

public class SlimeShooter : MonoBehaviour
{
    [SerializeField] GameObject SlimeBulletPrefab;

    [SerializeField] Transform slimeBulletSpawn;

    [SerializeField] float fireRate = 10;

    new Camera camera;

    float nextTimeToFire = 0;

    private Player Player;

    private void Start()
    {
        camera = Camera.main;
        Player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        //slimeBulletSpawn.LookAt(camera.transform.forward * 1000);

        if (Player.IsAiming && Player.IsShooting && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1 / fireRate;

            Fire();
        }
    }

    private void Fire()
    {
        SlimeBullet bullet = Instantiate(SlimeBulletPrefab, slimeBulletSpawn.position, Quaternion.identity).GetComponent<SlimeBullet>();

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit))
            bullet.transform.LookAt(hit.point);
        else
            bullet.transform.forward = camera.transform.forward;

        bullet.InitialiseBullet(30, 5);

        PlaySlimeShotSound();
    }

    private void PlaySlimeShotSound()
    {
        print("Should be playing slimeshot sound.");
    }
}
