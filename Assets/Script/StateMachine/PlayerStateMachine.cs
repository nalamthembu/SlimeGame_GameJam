using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public Player Player { get; private set; }

    private PlayerBaseState currentState;
    public PlayerIdleState PlayerIdleState = new();
    public PlayerWalkState PlayerWalkState = new();
    public PlayerRunState PlayerRunState = new();
    public PlayerJumpState PlayerJumpState = new();
    public PlayerFallState PlayerFallState = new();
    public PlayerStopState PlayerStopState = new();

    Transform leftFoot;
    Transform rightFoot;

    private void Awake()
    {
        Player = GetComponent<Player>();
    }

    private void Start()
    {
        currentState = PlayerIdleState;
        currentState.EnterState(this);
    }

    public void DoStateCheck()
    {
        if (Player.IsGrounded)
        {
            if (Player.InputMagnitude <= 0)
            {
                DoSwitchState(PlayerStopState);
            }

            if (currentState is PlayerFallState && Player.InputMagnitude > 0)
            {
                if (Player.IsRunning)
                    DoSwitchState(PlayerRunState);

                if (!Player.IsRunning)
                    DoSwitchState(PlayerWalkState);
            }

            if (Player.InputMagnitude != 0)
            {
                if (Player.IsRunning)
                    DoSwitchState(PlayerRunState);

                if (!Player.IsRunning)
                    DoSwitchState(PlayerWalkState);
            }
        }
        else
        {
            DoSwitchState(PlayerFallState);
        }
    }

    public void DoFootCheck(PlayerStateMachine stateMachine)
    {
        if (leftFoot is null || leftFoot is null)
        {
            leftFoot = stateMachine.Player.Animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            rightFoot = stateMachine.Player.Animator.GetBoneTransform(HumanBodyBones.RightFoot);
        }

        if (leftFoot.forward.z > rightFoot.forward.z)
        {
            stateMachine.Player.IsRU = true;
            stateMachine.Player.IsLU = false;
        }
        else if (rightFoot.forward.z > leftFoot.forward.z)
        {
            stateMachine.Player.IsRU = true;
            stateMachine.Player.IsLU = false;
        }
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void DoSwitchState(PlayerBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void ResetFootCheck() => Player.IsRU = Player.IsLU = false;
    public void HandlePosition()
    {
        float currentSpeed = Player.CurrentSpeed;
        float targetSpeed = Player.TargetSpeed;

        if (Player.IsGrounded)
            Player.VelocityY = 0;

        Player.CurrentSpeed = Mathf.SmoothDamp
            (
                currentSpeed,
                targetSpeed,
                ref Player.m_SpeedSmoothVelocity,
                Player.GetModifiedSmoothTime(Player.SpeedSmoothTime)
            );

        Player.VelocityY += Time.deltaTime * Player.Gravity;
        Player.Velocity = (Player.transform.forward * Player.CurrentSpeed) + Vector3.up * Player.VelocityY;
        Player.Controller.Move(Player.Velocity * Time.deltaTime);
    }
    public void HandleRotation()
    {
        Player.TargetRotation = Mathf.Atan2(Player.InputDir.x, Player.InputDir.y) *
            Mathf.Rad2Deg + Player.MainCamera.eulerAngles.y;

        Player.transform.eulerAngles = Vector3.up *
            Mathf.SmoothDampAngle
            (
                Player.transform.eulerAngles.y,
                Player.TargetRotation,
                ref Player.m_TurnSmoothVelocity,
                Player.GetModifiedSmoothTime(Player.TurnSmoothTime)
            );
    }
}
