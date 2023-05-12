using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected float walkSpeed = 2;
    [SerializeField] protected float runSpeed = 2;
    [SerializeField] protected float health = 100;
    [SerializeField] protected float armour = 0;

    protected Animator m_Animator;
    public Animator Animator { get; private set; }

    public bool IsDead
    {
        get
        {
            return health <= 0;
        }
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
    }
}