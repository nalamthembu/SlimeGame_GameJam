using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected float walkSpeed = 2;
    [SerializeField] protected float runSpeed = 2;
    [SerializeField] protected float health = 100;
    [SerializeField] protected float armour = 0;

    //Input Based Values
    bool m_IsGrounded;

    protected Animator m_Animator;
    public Animator Animator 
    {
        get { return m_Animator; }
    }
    public bool IsGrounded { get { return m_IsGrounded; } }
    public float Health
    {
        get
        {
            return health;
        }
    }

    public bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    public void Teleport(Vector3 position)
    {
        if (this is Player player)
            player.transform.position = position;

        if (this is Enemy enemy)
            enemy.Agent.Warp(position);

        print("Teleported character to : " + position);
    }

    public void TakeDamage(float damage)
    {
        if (IsDead)
        {
            print("Character is dead");
            return;
        }

        damage = Mathf.Abs(damage);

        if (armour > 0)
            armour -= damage;
        else
            health -= damage;

        health = health < 0 ? 0 : health;

        //Play Hurt Sound Effect?
    }

    protected void HandleGroundCheck()
    {
        m_IsGrounded = Physics.Linecast(transform.position + Vector3.up * 0.1F, transform.position + Vector3.down * 5, out RaycastHit hit) && hit.distance <= 0.5F;

        if (m_IsGrounded)
            Debug.DrawLine(transform.position + Vector3.up * 0.1F, hit.point, Color.green);
    }
}