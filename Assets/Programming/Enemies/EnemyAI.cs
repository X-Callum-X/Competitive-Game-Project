using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    private Camera cam;
    public NavMeshAgent agent;
    public ParticleSystem deathEffect;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    private SpawnDuds spawnDuds;

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
    public ParticleSystem explosionEffect;
    private GameObject damageArea;

    [Header("States")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public bool isBigEnemy;

    private void Awake()
    {
        cam = Camera.main;

        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        spawnDuds = GetComponent<SpawnDuds>();

        if (isBigEnemy)
        {
            damageArea = transform.Find("Damage Area").gameObject;

            damageArea.gameObject.SetActive(false);
        }

        healthBar.maxValue = health;

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

        if (!isBigEnemy)
        {
            transform.LookAt(player);
        }

        if (isBigEnemy)
        {
            if (!hasAttacked)
            {
                explosionEffect.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 2.5f, this.transform.position.z);

                Instantiate(explosionEffect, explosionEffect.transform.position, Quaternion.Euler(-90, 0, 0));

                StopAllCoroutines();
                StartCoroutine(TriggerDamageArea());

                hasAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
        else if (!isBigEnemy)
        {
            if (!hasAttacked)
            {
                Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

                rb.AddForce(transform.forward * 20f, ForceMode.Impulse);

                Destroy(rb.gameObject, 3);

                hasAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        UpdateHealthBar();

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        spawnDuds.DropDuds();

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

    private IEnumerator TriggerDamageArea()
    {
        damageArea.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        damageArea.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
