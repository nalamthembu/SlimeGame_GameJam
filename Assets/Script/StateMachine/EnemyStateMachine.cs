using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    Enemy enemy;
    Animator animator;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        animator = enemy.Animator;
    }

    private void Update() => Animate();

    private void Animate()
    {
        animator.SetBool("IsDead", enemy.IsDead);
        animator.SetFloat("Speed", enemy.Agent.velocity.magnitude);
        animator.SetBool("IsAttacking", enemy.IsAttacking);
    }
}
