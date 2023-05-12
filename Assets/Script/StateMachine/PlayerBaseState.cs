public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateMachine stateMachine);
    public abstract void UpdateState(PlayerStateMachine stateMachine);
    public abstract void ExitState(PlayerStateMachine stateMachine);
    public abstract void CheckStateChange(PlayerStateMachine stateMachine);
}