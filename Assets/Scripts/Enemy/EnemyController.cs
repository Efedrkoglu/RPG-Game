using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float sightRange, attackRange, walkPointRange;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private bool invertSpriteFlip = false;

    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private Animator animator;
    private bool isPlayerInChasingRange, isPlayerInAttackRange, isWalkPointSet, isWaiting, isAttacking;
    private Vector3 walkPoint;

    private void Start() {
        agent = gameObject.GetComponent<NavMeshAgent>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isWalkPointSet = false;
        isWaiting = false;
        isAttacking = false;
    }

    private void Update() {
        isPlayerInChasingRange = Physics.CheckSphere(gameObject.transform.position, sightRange, playerMask);
        isPlayerInAttackRange = Physics.CheckSphere(gameObject.transform.position, attackRange, playerMask);

        if(agent.velocity.x != 0) {
            animator.SetBool("isRunning", true);

            if(!invertSpriteFlip) {
                if (agent.velocity.x < 0f)
                    spriteRenderer.flipX = true;
                else
                    spriteRenderer.flipX = false;
            }
            else {
                if (agent.velocity.x < 0f)
                    spriteRenderer.flipX = false;
                else
                    spriteRenderer.flipX = true;
            }
        }
        else {
            animator.SetBool("isRunning", false);
        }

        if(isAttacking) return;

        if(!isPlayerInChasingRange && !isPlayerInAttackRange) Patrol();

        if(isPlayerInChasingRange && !isPlayerInAttackRange) Chase();

        if(isPlayerInChasingRange && isPlayerInAttackRange) Attack();
    }

    private void Patrol() {
        if (isWaiting) return;

        if (!isWalkPointSet) {
            StartCoroutine(Wait());
            setWalkPoint();
        }   
        else {
            agent.SetDestination(walkPoint);
            
            Vector3 distanceToWalkPoint = transform.position - walkPoint;
            if (distanceToWalkPoint.magnitude < .2f)
                isWalkPointSet = false;
        }
    }

    private IEnumerator Wait() {
        isWaiting = true;
        float randomNumber = Random.Range(2, 5);
        yield return new WaitForSeconds(randomNumber);
        isWaiting = false;
    }

    private void setWalkPoint() {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        isWalkPointSet = true;
    }

    private void Chase() {
        agent.SetDestination(player.position);
    }

    private void Attack() {
        isAttacking = true;
        agent.SetDestination(transform.position);
        animator.SetBool("isAttacking", true);
    }

    private void EndAttacking() {
        animator.SetBool("isAttacking", false);
        isAttacking = false;
    }
}
