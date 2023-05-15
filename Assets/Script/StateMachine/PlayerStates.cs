using UnityEngine;

public class PlayerStates
{
    //Does literally nothing.
}

public class PlayerIdleState : PlayerBaseState
{
    public override void CheckStateChange(PlayerStateMachine stateMachine)
    {
        stateMachine.DoStateCheck();
        return;
    }

    public override void EnterState(PlayerStateMachine stateMachine)
    {
        return;
    }

    public override void ExitState(PlayerStateMachine stateMachine)
    {
        return;
    }

    public override void UpdateState(PlayerStateMachine stateMachine)
    {
        //Rotate when aiming.
        CheckStateChange(stateMachine);
        return;
    }
}

public class PlayerJumpState : PlayerBaseState
{
    Player player;

    public override void CheckStateChange(PlayerStateMachine stateMachine)
    {
        //Check if the player has touched the ground, then move to the right state.
        stateMachine.DoStateCheck();
        return;
    }

    public override void EnterState(PlayerStateMachine stateMachine)
    {
        player = stateMachine.Player;

        player.VelocityY = 0;

        float g = player.Gravity;
        float h = player.JumpHeight;

        player.VelocityY = Mathf.Sqrt(-2 * g * h);

        //Do a check to see which foot the character jumped off.
        stateMachine.DoFootCheck(stateMachine);

    }


    public override void ExitState(PlayerStateMachine stateMachine)
    {
        return;
    }

    public override void UpdateState(PlayerStateMachine stateMachine)
    {
        //Handle Movement  -> Do this here.
        //Handle Rotation -> Do this in stateMachine.
        HandlePosition_DontResetVelocityY();
        stateMachine.HandleRotation();
        CheckStateChange(stateMachine);
    }

    private void HandlePosition_DontResetVelocityY()
    {

        float currentSpeed = player.CurrentSpeed;
        float targetSpeed = player.TargetSpeed;

        player.CurrentSpeed = Mathf.SmoothDamp
            (
                currentSpeed,
                targetSpeed,
                ref player.m_SpeedSmoothVelocity,
                player.GetModifiedSmoothTime(player.SpeedSmoothTime)
            );

        player.VelocityY += Time.deltaTime * player.Gravity;
        player.Velocity = (player.transform.forward * player.CurrentSpeed) + Vector3.up * player.VelocityY;
        player.Controller.Move(player.Velocity * Time.deltaTime);
    }
}

public class PlayerFallState : PlayerBaseState
{
    public override void CheckStateChange(PlayerStateMachine stateMachine)
    {
        //Check what state the state needs to move to after falling, based on input.
        stateMachine.DoStateCheck();
        return;
    }

    public override void EnterState(PlayerStateMachine stateMachine)
    {
        //Idk...falling sound AHHH?
        return;
    }

    public override void ExitState(PlayerStateMachine stateMachine)
    {
        return;
    }

    public override void UpdateState(PlayerStateMachine stateMachine)
    {
        //HandlePosition -> Do that in stateMachine.
        //HandleRotation -> Do that in stateMachine.
        stateMachine.HandlePosition();
        stateMachine.HandleRotation();
        CheckStateChange(stateMachine);
    }
}

public class PlayerWalkState : PlayerBaseState
{
    public override void CheckStateChange(PlayerStateMachine stateMachine)
    {
        stateMachine.DoStateCheck();
    }

    public override void EnterState(PlayerStateMachine stateMachine)
    {
        //Reset footcheck.
        stateMachine.ResetFootCheck();
    }

    public override void ExitState(PlayerStateMachine stateMachine)
    {
        return;
    }

    public override void UpdateState(PlayerStateMachine stateMachine)
    {
        //HandlePosition -> Do this in stateMachine.
        //HandleRotation -> Do this in stateMachine.
        stateMachine.HandlePosition();
        stateMachine.HandleRotation();
        CheckStateChange(stateMachine);
    }
}

public class PlayerLandState : PlayerBaseState
{
    public override void CheckStateChange(PlayerStateMachine stateMachine)
    {
        stateMachine.DoFootCheck(stateMachine);
    }

    public override void EnterState(PlayerStateMachine stateMachine)
    {
        stateMachine.DoFootCheck(stateMachine);
  
    }

    public override void ExitState(PlayerStateMachine stateMachine)
    {
        stateMachine.Player.IsRU = stateMachine.Player.IsLU = false;
    }

    public override void UpdateState(PlayerStateMachine stateMachine)
    {
        CheckStateChange(stateMachine);
    }
}

public class PlayerRunState : PlayerBaseState
{
    public override void CheckStateChange(PlayerStateMachine stateMachine)
    {
        //Check if player should go to another state.
        stateMachine.DoStateCheck();
    }

    public override void EnterState(PlayerStateMachine stateMachine)
    {
        stateMachine.ResetFootCheck();
        stateMachine.Player.TargetSpeed = stateMachine.Player.RunSpeed;
    }

    public override void ExitState(PlayerStateMachine stateMachine)
    {
        return;
    }

    public override void UpdateState(PlayerStateMachine stateMachine)
    {
        stateMachine.HandlePosition();
        stateMachine.HandleRotation();
        CheckStateChange(stateMachine);
    }
}

public class PlayerStopState : PlayerBaseState
{
    Player player;

    float momentumMagnitude;

    public override void CheckStateChange(PlayerStateMachine stateMachine)
    {
        stateMachine.DoStateCheck();
    }

    public override void EnterState(PlayerStateMachine stateMachine)
    {
        player = stateMachine.Player;
        momentumMagnitude = player.Velocity.magnitude;
    }

    public override void ExitState(PlayerStateMachine stateMachine)
    {
        return;
    }

    public override void UpdateState(PlayerStateMachine stateMachine)
    {
        HandleStop();
        stateMachine.DoStateCheck();
    }

    private void HandleStop()
    {
        float currentSpeed = player.CurrentSpeed;
        float targetSpeed = 0;

        player.CurrentSpeed = Mathf.SmoothDamp
            (
                currentSpeed,
                targetSpeed,
                ref player.m_SpeedSmoothVelocity,
                player.GetModifiedSmoothTime(player.SpeedSmoothTime)
            );

        player.Velocity = (player.transform.forward * player.CurrentSpeed);
        player.Controller.Move(player.Velocity * Time.deltaTime);
    }
}