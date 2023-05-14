using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerStateMachine))]
public class Player : Character
{
    //IN-Editor Values
    [SerializeField][Range(0, 10)] float m_TurnSmoothTime = 0.15F;
    [SerializeField][Range(0, 10)] float m_SpeedSmoothTime = 0.25F;
    [SerializeField][Range(0, 10)] float m_JumpHeight = 1.5F;
    [SerializeField][Range(0, 1)] float m_AirControlValue = 0.5F;
    [SerializeField][Range(0, -100)] float m_Gravity = -24.0F;

    //Private

    //Automated Values
    [HideInInspector] public float m_TurnSmoothVelocity;
    [HideInInspector] public float m_SpeedSmoothVelocity;
    bool m_IsRotatingToTargetDirection;
    float m_VelocityY;
    Vector3 m_Velocity;

    //Input Based Values
    float m_InputMagnitude;
    float m_TargetSpeed;
    float m_CurrentSpeed;
    float m_TargetRotation;
    bool m_IsRU;
    bool m_IsLU;
    bool m_IsRunning;
    bool m_IsJumping;

    Vector2 m_Input;
    Vector2 m_InputDir;

    //Components
    CharacterController m_Controller;
    CameraController m_CameraController;
    Transform m_MainCamera;

    public float InputMagnitude { get { return m_InputMagnitude; } }
    public float TargetRotation { get { return m_TargetRotation; } set { m_TargetRotation = value; } }
    public float JumpHeight { get { return m_JumpHeight; } }
    public float WalkSpeed { get { return walkSpeed; } }
    public float RunSpeed { get { return runSpeed; } }
    public float TargetSpeed { get { return m_TargetSpeed; } set { m_TargetSpeed = value; } }
    public float CurrentSpeed { get { return m_CurrentSpeed; } set { m_CurrentSpeed = value; } }
    public float SpeedSmoothTime { get { return m_SpeedSmoothTime; } }
    public float VelocityY { get { return m_VelocityY; } set { m_VelocityY = value; } }
    public float TurnSmoothTime { get { return m_TurnSmoothTime; } }
    public float Gravity { get { return m_Gravity; } }
    public Vector3 Velocity { get { return m_Velocity; } set { m_Velocity = value; } }
    public Vector2 InputDir { get { return m_InputDir; } }
    public Vector2 CameraPitchYaw { get { return new Vector2(m_CameraController.Pitch, m_CameraController.yaw); } }
    public bool IsRU { get { return m_IsRU; } set { m_IsRU = value; } }
    public bool IsLU { get { return m_IsLU; } set { m_IsLU = value; } }
    public bool IsRotatingTowardTargetRot { get { return m_IsRotatingToTargetDirection; } set { m_IsRotatingToTargetDirection = value; } }
    public bool IsRunning { get { return m_IsRunning; } }
    
    public bool IsJumping { get { return m_IsJumping; } }
    public CharacterController Controller { get { return m_Controller; } }
    public Transform MainCamera { get { return m_MainCamera; } }

    private void Awake()
    {
        m_Controller = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //since the camera is set up in awake.
        m_MainCamera = Camera.main.transform;
        m_CameraController = FindObjectOfType<CameraController>();
    }

    private void Update()
    {
        HandleInput();
        HandleGroundCheck();
        Animate();
    }

    private void HandleInput()
    {
        m_IsJumping = Input.GetKey(KeyCode.Space);
        m_Input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));
        m_InputDir = m_Input.normalized;
        m_InputMagnitude = m_InputDir.magnitude; //Input Magnitude is any movement input from the player.
        m_IsRunning = Input.GetKey(KeyCode.LeftShift);
    }

    
    public float GetModifiedSmoothTime(float smoothTime)
    {
        if (m_Controller.isGrounded)
            return smoothTime;

        if (m_AirControlValue <= 0F)
            return float.MaxValue;

        return smoothTime / m_AirControlValue;
    }
    private void Animate()
    {
        Animator.SetBool("IsJumping",m_IsJumping);
        Animator.SetBool("IsGrounded", IsGrounded);
    }
}
