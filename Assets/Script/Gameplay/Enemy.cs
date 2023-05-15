using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Character
{
    private NavMeshAgent agent;
    Player player;

    [SerializeField][Range(1, 100)] float damageAmount = 1;

    public NavMeshAgent Agent { get; private set; }

    [SerializeField][Range(1, 100)] float attackRate = 1F;
    private float nextTimeToAttack;

    public bool IsAttacking { get; private set; }

    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Agent = agent;
        player = FindObjectOfType<Player>();
        m_Animator = GetComponent<Animator>();
        FindPlayer();
    }

    private void FindPlayer()
    {
        if (agent.isStopped)
            agent.isStopped = false;
        agent.SetDestination(player.transform.position);
    }

    float timer = 0;

    private void Update()
    {
        if (!IsDead)
        {
            timer += Time.deltaTime;

            if (timer >= 1)
            {
                if (agent.remainingDistance > agent.stoppingDistance)
                FindPlayer();

                timer = 0;
            }
        }
        else
        {


            Agent.isStopped = true;
            PlayDeathSound();
            EnemyManager.instance.KillEnemy(this);
        }
    }

    private void PlayDeathSound()
    {
        print("Player Death Sound");
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

            agent.isStopped = true;

            if (Time.time >= nextTimeToAttack)
            {
                nextTimeToAttack = Time.time + 1 / attackRate;
                
                Attack(character);
                IsAttacking = true;
            }
            else
            {
                IsAttacking = false;
            }
        }
    }
}