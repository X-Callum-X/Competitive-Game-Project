using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Camera cam;
    public NavMeshAgent agent;
    public ParticleSystem deathEffect;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public Slider healthBar;
    private float healthbarTarget = 1;
    public float reduceSpeed = 2;

    public float health;

    [Header("Wandering Variables")]
    public Vector3 movePoint;
    bool movePointSet;
    public float movePointRange;

    [Header("Attacking Variables")]
    public float timeBetweenAttacks;
    bool hasAttacked;
    public GameObject projectile;

    [Header("States")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        cam = Camera.main;

        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        healthBar.maxValue = health;

        //healthBar.value = health;

        UpdateHealthBar();
    }

    private void Update()
    {
        healthBar.transform.rotation = Quaternion.LookRotation(healthBar.transform.position - cam.transform.position);
        healthBar.value = Mathf.MoveTowards(healthBar.value, healthbarTarget, reduceSpeed * Time.deltaTime);

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Wander();
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            Chase();
        }

        if (playerInSightRange && playerInAttackRange)
        {
            Attack();
        }
    }

    private void SearchForMovePoint()
    {
        float randomZ = Random.Range(-movePointRange, movePointRange);
        float randomX = Random.Range(-movePointRange, movePointRange);

        movePoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(movePoint, -transform.up, 2f, whatIsGround))
        {
            movePointSet = true;
        }
    }

    private void Wander()
    {
        if (!movePointSet)
        {
            SearchForMovePoint();
        }

        if (movePointSet)
        {
            agent.SetDestination(movePoint);
        }

        Vector3 distanceToMovePoint = transform.position - movePoint;

        if (distanceToMovePoint.magnitude < 1f)
        {
            movePointSet = false;
        }

    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!hasAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 20f, ForceMode.Impulse);

            Destroy(rb.gameObject, 3);

            hasAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        //healthBar.value = health;

        UpdateHealthBar();

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void UpdateHealthBar()
    {
        healthbarTarget = health;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player Projectile"))
        {
            Destroy(other.gameObject);
            TakeDamage(1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
