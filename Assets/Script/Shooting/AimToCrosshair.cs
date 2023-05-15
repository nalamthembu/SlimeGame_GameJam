using UnityEngine;

public class AimToCrosshair : MonoBehaviour
{
    [SerializeField] private LayerMask aimColliderMask = new LayerMask();
    [SerializeField] private Transform testAiming;

    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private GameObject Slime_Projectile;

    void Update()
    {
        Vector2 ScreenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(ScreenCenter);
        if(Physics.Raycast(ray,out RaycastHit rayCast, 999f, aimColliderMask))
        {
            testAiming.position = rayCast.point;
        }

        if (Input.GetMouseButtonDown(0) && Input.GetMouseButton(1) != true) //IF SHOOTING FROM THE HIP.
        {
            Vector3 aimDir = (testAiming.position - SpawnPoint.position).normalized;
            GameObject projectile = Instantiate(Slime_Projectile, SpawnPoint.position, Quaternion.LookRotation(aimDir, Vector3.down));
            //projectile.GetComponent<Slime_Projectile>().force = aimDir;

        }
    }

    
}
