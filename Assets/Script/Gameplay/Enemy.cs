using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Character
{
    private NavMeshAgent agent;
    Player player;

    [SerializeField][Range(1, 100)] float damageAmount = 1;

    public NavMeshAgent Agent { get; private set; }

    [SerializeField][Range(1, 100)] float attackRate = 0.1F;
    private float timerToAttack;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>();

        FindPlayer();
    }

    private void FindPlayer() => agent.SetDestination(player.transform.position);

    float timer = 0;
    float attackTimer = 0;

    private void Update()
    {
        if (!IsDead)
        {
            timer += Time.deltaTime;

            if (timer >= 1)
            {
                FindPlayer();

                timer = 0;
            }
        }
    }

    private void Attack(Character characterToAttack)
    {
        if (characterToAttack is Player player)
        {
            player.TakeDamage(damageAmount);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Character character = other.GetComponent<Character>();

            print(character.transform.name);

            timerToAttack += Time.deltaTime;

            if (timerToAttack >= attackRate)
            {
                Attack(character);
                timerToAttack = 0;
            }
        }
    }
}