using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidBody;
	private SpriteRenderer spriteRenderer;
	private Animator animator;

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

		Vector3 movementDirection = new Vector3(X_axis, 0, Z_axis);

		if (movementDirection.magnitude > 1f)
		{
			movementDirection.Normalize();
		}

		rigidBody.velocity = new Vector3(movementDirection.x * movementSpeed, rigidBody.velocity.y, movementDirection.z * movementSpeed);

		if (movementDirection.x < 0)
		{
			spriteRenderer.flipX = true;
		}
		if(movementDirection.x > 0)
		{
			spriteRenderer.flipX = false;
		}

		if(rigidBody.velocity.x > 0.02f || rigidBody.velocity.x < -0.02f || rigidBody.velocity.z > 0.02f || rigidBody.velocity.z < -0.02f)
		{
			animator.SetBool("isRunning", true);
		}
		else
		{
			animator.SetBool("isRunning", false);
		}
		
	}

}
