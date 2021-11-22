using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    private Animator animator;
    
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public int health = 100;
    public EnemyHealth healthBar;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        healthBar.SetMaxHealth(health);

    }
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!playerInAttackRange && !playerInSightRange) StartCoroutine(PatrolingStart());
        if (!playerInAttackRange && playerInSightRange) Chasing();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

    }

    IEnumerator PatrolingStart()
    {
        yield return new WaitForSeconds(1.0f);
        Patroling();
    }


    private void Patroling()
    {
        animator.SetTrigger("IsMoving");
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) 
            walkPointSet = true ;
    }
    private void Chasing()
    {
        animator.SetTrigger("IsChasing");
        Vector3 offset = new Vector3(1, 0, 1);
        agent.SetDestination((player.position)-offset);
    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        animator.SetTrigger("IsAttacking");
        if(!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
       
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    public void TakeDamage(int amount)
    {
        /* if healt == 50 then animacja upadku
         * StopChasing();
         * stopchasing()
         * {
         * set destination (0);
         * 
         * waitfor 2 s
         * chasing();
         * }
         * 
         */
        health -= amount;
        if(health<=0)
        {

            //upadek + wait for 2 s then
            animator.SetTrigger("IsFalling");

            StartCoroutine(Died());
        }
        healthBar.SetHealth(health);
    }
    IEnumerator Died()
    {
        yield return new WaitForSeconds(1.2f);
        Die();
    }
    private void Die()
    {
        Destroy(gameObject);
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


}
