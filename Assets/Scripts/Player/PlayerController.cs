using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody rigidBody;
	private SpriteRenderer spriteRenderer;
	private Animator animator;
	private Vector3 movementDirection;
	private bool isHit;

	[SerializeField] private Transform hitPoint;
	[SerializeField] private float movementSpeed;

	private void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		float X_axis = Input.GetAxis("Horizontal");
		float Z_axis = Input.GetAxis("Vertical");

		movementDirection = new Vector3(X_axis, 0, Z_axis);

		if (movementDirection.magnitude > 1f)
		{
			movementDirection.Normalize();
		}

		if (movementDirection.x < 0)
		{
			spriteRenderer.flipX = true;
		}
		if (movementDirection.x > 0)
		{
			spriteRenderer.flipX = false;
		}

		if (Input.GetKeyDown(KeyCode.Mouse1) && !animator.GetBool("isAttacking"))
		{
			animator.SetBool("isAttacking", true);

			Vector3 hitDirection = hitPoint.transform.right;
			if (spriteRenderer.flipX)
				hitDirection *= -1;

			isHit = Physics.SphereCast(hitPoint.transform.position, .05f, hitDirection, out RaycastHit hitInfo, .4f, LayerMask.GetMask("Enemy"));
			if (isHit)
			{
				Debug.Log(hitInfo.collider.name);
			}
		}
	}

	private void FixedUpdate()
	{
		if (!animator.GetBool("isAttacking"))
		{
			rigidBody.velocity = new Vector3(movementDirection.x * movementSpeed, rigidBody.velocity.y, movementDirection.z * movementSpeed);
		}

		if(rigidBody.velocity.x < -.2f || rigidBody.velocity.x > .2f || rigidBody.velocity.z < -.2f || rigidBody.velocity.z > .2f)
		{
			animator.SetBool("isRunning", true);
		}
		else
		{
			animator.SetBool("isRunning", false);
		}
	}


	public void ExitAttackAnimation()
	{
		animator.SetBool("isAttacking", false);
	}
}


