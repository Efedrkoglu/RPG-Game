using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody rigidBody;
	private SpriteRenderer spriteRenderer;
	private Animator animator;
	private Vector3 movementDirection;
	private bool isHit;
	private bool listenInputs;
	private GameObject lastHitEnemy;

	[SerializeField] private float movementSpeed;
	[SerializeField] private GameScreen gameScreen;

	public static event Action<Enemy, int> CombatTriggered;

	private void Start() {
		rigidBody = GetComponent<Rigidbody>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
        listenInputs = true;

		CombatSystem.CombatEnded += OnCombatEnded;
	}

    private void OnDestroy() {
		CombatSystem.CombatEnded -= OnCombatEnded;
    }

    private void Update() {
		float X_axis = 0;
		float Z_axis = 0;
		if(listenInputs) {
            X_axis = Input.GetAxis("Horizontal");
            Z_axis = Input.GetAxis("Vertical");
        }

		movementDirection = new Vector3(X_axis, 0, Z_axis);

		if (movementDirection.magnitude > 1f) {
			movementDirection.Normalize();
		}

		if (movementDirection.x < 0) {
			spriteRenderer.flipX = true;
		}
		if (movementDirection.x > 0) {
			spriteRenderer.flipX = false;
		}

		if (Input.GetKeyDown(KeyCode.Mouse1) && !animator.GetBool("isAttacking")) {
			animator.SetBool("isAttacking", true);
		}
	}

	private void FixedUpdate() {
		if (!animator.GetBool("isAttacking")) {
			rigidBody.velocity = new Vector3(movementDirection.x * movementSpeed, rigidBody.velocity.y, movementDirection.z * movementSpeed);
		}

		if (rigidBody.velocity.x < -.2f || rigidBody.velocity.x > .2f || rigidBody.velocity.z < -.2f || rigidBody.velocity.z > .2f) {
			animator.SetBool("isRunning", true);
		} 
		else {
			animator.SetBool("isRunning", false);
		}
	}
	private IEnumerator CombatTriggeredEvent(Enemy enemy) {
		yield return new WaitForSeconds(.15f);

		if(enemy.CurrentHp <= Player.Instance.Damage) {
			OnCombatEnded(true);
		}
		else {
            listenInputs = false;
            CombatTriggered?.Invoke(enemy, 0);
        }
	}

	public void ExitAttackAnimation() {
		animator.SetBool("isAttacking", false);
	}

	public void CheckAttackHit() {
		Vector3 hitDirection = transform.right;
		if (spriteRenderer.flipX)
			hitDirection *= -1;

		isHit = Physics.SphereCast(transform.position, .05f, hitDirection, out RaycastHit hitInfo, .4f, LayerMask.GetMask("Hitable"));

		if (isHit) {
			if (hitInfo.collider.gameObject.CompareTag("Enemy")) {
				lastHitEnemy = hitInfo.collider.gameObject;
				hitInfo.collider.gameObject.GetComponent<Animator>().SetTrigger("Hurt");
				StartCoroutine(CombatTriggeredEvent(hitInfo.collider.gameObject.GetComponent<Enemy>()));
			}
		}
	}

    private void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Item")) {
			gameScreen.TogglePressEMessage(true, "pick up");
		}
		else if(other.gameObject.CompareTag("LootBag")) {
			gameScreen.TogglePressEMessage(true, "Loot");
			
		}
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Item")) {
            gameScreen.TogglePressEMessage(false);
        } 
		else if (other.gameObject.CompareTag("LootBag")) {
            gameScreen.TogglePressEMessage(false);
        }
    }

    public void OnCombatEnded(bool combatResult) {
		if(combatResult) {
			lastHitEnemy.GetComponent<Enemy>().Die();
			listenInputs = true;
		}
		else {
			Die();
		}
	}

	public void Die() {
		animator.SetTrigger("Die");
		Player.Instance.IsDead = true;
		listenInputs = false;
	}

	public void SetLastHitEnemy(GameObject enemy) {
		lastHitEnemy = enemy;
	}
}


