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

	public static event Action<GameObject, Enemy> CombatTriggered;

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
	private IEnumerator CombatTriggeredEvent(GameObject enemyGO, Enemy enemy) {
		yield return new WaitForSeconds(.15f);
        listenInputs = false;
		CombatTriggered?.Invoke(enemyGO, enemy);
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
				hitInfo.collider.gameObject.GetComponent<Animator>().SetTrigger("hurtTrigger");
				StartCoroutine(CombatTriggeredEvent(hitInfo.collider.gameObject, hitInfo.collider.gameObject.GetComponent<Enemy>()));
			}
		}
	}

	public void OnCombatEnded(bool combatResult) {
		if(combatResult) {
			Destroy(lastHitEnemy);
			listenInputs = true;
		}
		else {
			Destroy(gameObject);
		}
	}

}


