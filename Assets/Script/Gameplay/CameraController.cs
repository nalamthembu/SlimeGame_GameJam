using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    private Player player;

    [Range(1, 10)]
    public float distanceFromTarget;

    [Range(0, 30)]
    public float mouseSensitivity = 10;

    public Vector2 pitchMinMax = new(-40, 85);

    public Vector2 cameraAimOffset;
    public float cameraAimDistance;

    public float Pitch { get; private set; }
    public float yaw { get; private set; }

    private float modifiedDistanceFromTarget;

    new Camera camera;

    private void HandleInput()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        Pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        Pitch = Mathf.Clamp(Pitch, pitchMinMax.x, pitchMinMax.y);
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        camera = Camera.main;
        modifiedDistanceFromTarget = distanceFromTarget;
    }

    private void Update() => HandleInput();

    private void LateUpdate()
    {
        Vector3 desiredRotation = new(Pitch, yaw);
        transform.eulerAngles = desiredRotation;

        ProcessAiming();

        transform.position = target.position - transform.forward * modifiedDistanceFromTarget;
    }

    private void ProcessAiming()
    {

        camera.transform.localPosition = player.IsAiming ? Vector3.Lerp(camera.transform.localPosition, player.SwappedShoulders ? new (-cameraAimOffset.x, cameraAimOffset.y) : cameraAimOffset, Time.deltaTime * 4f) :
           Vector3.Lerp(camera.transform.localPosition, Vector3.zero, Time.deltaTime * 4);
        modifiedDistanceFromTarget = player.IsAiming ? Mathf.Lerp(modifiedDistanceFromTarget, cameraAimDistance, Time.deltaTime * 4) :
            Mathf.Lerp(modifiedDistanceFromTarget, distanceFromTarget, Time.deltaTime * 4);
    }
}
