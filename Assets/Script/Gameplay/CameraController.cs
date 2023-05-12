using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    [Range(1, 10)]
    public float distanceFromTarget;

    [Range(0, 30)]
    public float mouseSensitivity = 10;

    public Vector2 pitchMinMax = new(-40, 85);

    public float Pitch { get; private set; }
    public float Yaw { get; private set; }

    private void HandleInput()
    {
        Yaw += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        Pitch -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        Pitch = Mathf.Clamp(Pitch, pitchMinMax.x, pitchMinMax.y);
    }

    private void Update() => HandleInput();

    private void LateUpdate()
    {
        Vector3 desiredRotation = new(Pitch, Yaw);
        transform.eulerAngles = desiredRotation;

        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}
