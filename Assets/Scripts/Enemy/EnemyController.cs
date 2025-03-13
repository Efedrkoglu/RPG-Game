using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float sightRange, attackRange, walkPointRange;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private bool invertSpriteFlip = false;
    [SerializeField] private Enemy enemyScript;

    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private Animator animator;
    private bool isPlayerInChasingRange, isPlayerInAttackRange, isWalkPointSet, isWaiting, isAttacking;
    private Vector3 walkPoint;

    public static event Action<Enemy, int> CombatTriggered;

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
        if(Player.Instance.IsInCombat || Player.Instance.IsDead) return;

        isPlayerInChasingRange = Physics.CheckSphere(gameObject.transform.position, sightRange, playerMask);
        isPlayerInAttackRange = Physics.CheckSphere(gameObject.transform.position, attackRange, playerMask);

        if(agent.velocity.magnitude != 0) {
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
        float randomNumber = UnityEngine.Random.Range(2, 5);
        yield return new WaitForSeconds(randomNumber);
        isWaiting = false;
    }

    private void setWalkPoint() {
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        bool isHit = Physics.Raycast(walkPoint, Vector3.down, 2f, LayerMask.GetMask("Ground"));

        if(isHit) isWalkPointSet = true;
        else isWalkPointSet = false;
    }

    private void Chase() {
        agent.SetDestination(player.position);
    }

    private void Attack() {
        isAttacking = true;
        agent.SetDestination(transform.position);
        animator.SetBool("isAttacking", true);
    }

    private void CheckHit() {
        if (Player.Instance.IsInCombat || Player.Instance.IsDead) return;

        Vector3 hitDirection = (player.position - transform.position).normalized;

        bool isHit = Physics.SphereCast(transform.position, .1f, hitDirection, out RaycastHit hitInfo, attackRange, playerMask);

        if(isHit) {
            if(Player.Instance.CurrentHp <= enemyScript.Damage) {
                Player.Instance.CurrentHp -= enemyScript.Damage;
                Player.Instance.gameObject.GetComponent<PlayerController>().OnCombatEnded(false);
            }
            else {
                Player.Instance.gameObject.GetComponent<PlayerController>().SetLastHitEnemy(gameObject);
                Player.Instance.gameObject.GetComponent<Animator>().SetTrigger("Hurt");
                CombatTriggered?.Invoke(enemyScript, 1);
            }
        }
    } 

    private void EndAttacking() {
        animator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    private void OnDrawGizmos() {
        if (player == null) return;

        Vector3 hitDirection = (player.position - transform.position).normalized;
        float radius = 0.1f;
        float maxDistance = attackRange;

        //Origin
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);

        //Direction
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + hitDirection * maxDistance);

        if (Physics.SphereCast(transform.position, radius, hitDirection, out RaycastHit hitInfo, maxDistance, playerMask)) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitInfo.point, radius);
        }
    }
}
